/*
 * Elasticsearch官网：https://www.elastic.co/guide/cn/elasticsearch/guide/current/index.html
 * Elasticsearch下载：https://www.elastic.co/downloads/elasticsearch
 * Elasticsearch C#:https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/index.html
 * C# NuGet Nest
 */
using Nest;
using System;

namespace Elasticsearch
{
    class Program
    {

        static void Main(string[] args)
        {

            var settings = new ConnectionSettings(new Uri("http://127.0.0.1:9200")).DefaultIndex("person").DefaultTypeName("people");

            ElasticClient client = new ElasticClient(settings);

            IIndexState indexState = new IndexState()
            {
                Settings = new IndexSettings()
                {
                    NumberOfReplicas = 1,//副本数
                    NumberOfShards = 5//分片数
                }
            };

            var r1 = client.DeleteIndex("person");
            //创建索引 先不maping 
            client.CreateIndex("person", p => p.InitializeUsing(indexState));

            //创建并Mapping
            var r2 = client.CreateIndex("person", p => p.InitializeUsing(indexState).Mappings(m => m.Map<Person>(mp => mp.AutoMap())));

            //var r3 = client.IndexExists("person");

            var r4_1 = client.Index(new Person() { Id = 2, LastName = "dada", FirstName = "liu", Age = 23 }, s => s.Index("person").Type("people"));
            var r4_2 = client.Index(new Person() { Id = 1, LastName = "weichao", FirstName = "liu", Age = 22 }, s => s.Index("person").Type("people"));
            var r4_3 = client.Index(new Person() { Id = 3, LastName = "dada", FirstName = "xi", Age = 23 }, s => s.Index("person").Type("people"));

            client.Refresh(new RefreshRequest("person"));

            var r5 = client.Search<Person>(s => s.Index("person").Type("people").Query(q => q.MatchAll()).Size(0).Aggregations(aggs => aggs.Terms("group_by", te => te.Field(f => f.FirstName.Suffix("keyword")).Size(10)
            .Aggregations(agg => agg.TopHits("top", top => top.Size(1))))));

            var r6 = client.Search<Person>(s => s.Size(0).Aggregations(ag => ag.Terms("group", t => t.Field(fd => fd.Age).Size(100))));

            var r7 = client.Search<Person>(s => s.Query(q => q.MatchAll()));

            Console.ReadKey();
        }
    }
}
