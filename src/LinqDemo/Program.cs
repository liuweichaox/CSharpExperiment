
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNetDemo
{
    public class Test
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
    }
    class PetOwner
    {
        public string Name { get; set; }
        public List<String> Pets { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Enumerable.SelectMany 方法
            一个序列的每个元素投影 IEnumerable<T> 并将合并为一个序列将结果序列。
             */
            PetOwner[] petOwners ={
                new PetOwner { Name="Higa, Sidney",Pets = new List<string>{ "Scruffy", "Sam" } },
                new PetOwner { Name="Ashkenazi, Ronen",Pets = new List<string>{ "Walker", "Sugar" } },
                new PetOwner { Name="Price, Vernette",Pets = new List<string>{ "Scratches", "Diesel" } } };

            // Query using SelectMany().
            IEnumerable<string> query1 = petOwners.SelectMany(petOwner => petOwner.Pets);

            Console.WriteLine("Using SelectMany():");

            // Only one foreach loop is required to iterate 
            // through the results since it is a
            // one-dimensional collection.
            foreach (string pet in query1)
            {
                Console.WriteLine(pet);
            }

            // This code shows how to use Select() 
            // instead of SelectMany().
            IEnumerable<List<String>> query2 = petOwners.Select(petOwner => petOwner.Pets);

            Console.WriteLine("\nUsing Select():");

            // Notice that two foreach loops are required to 
            // iterate through the results
            // because the query returns a collection of arrays.
            foreach (List<String> petList in query2)
            {
                foreach (string pet in petList)
                {
                    Console.WriteLine(pet);
                }
                Console.WriteLine();
            }

            List<Test> list = new List<Test>() {
                new Test() { ID = 1, Name = "aaa", ParentID = 0 },
                new Test() { ID = 2, Name = "aaa", ParentID = 0 },
                new Test() { ID = 3, Name = "aaa", ParentID = 0 },
                new Test() { ID = 4, Name = "aaa", ParentID = 1 },
                new Test() { ID = 5, Name = "aaa", ParentID = 1 },
                new Test() { ID = 6, Name = "aaa", ParentID = 2 },
                new Test() { ID = 7, Name = "aaa", ParentID = 2 },
                new Test() { ID = 8, Name = "aaa", ParentID = 3 },
                new Test() { ID = 9, Name = "aaa", ParentID = 4 },
                new Test() { ID = 10, Name = "aaa", ParentID = 6 },
                new Test() { ID = 11, Name = "aaa", ParentID = 7 },
                new Test() { ID = 12, Name = "aaa", ParentID = 8 },
                new Test() { ID = 13, Name = "aaa", ParentID = 9 },
                new Test() { ID = 14, Name = "aaa", ParentID = 10 },
                new Test() { ID = 15, Name = "aaa", ParentID = 11 },
                new Test() { ID = 16, Name = "aaa", ParentID = 12 },
            };
            string strJson = ForecahList(list, 0);
            Console.WriteLine(strJson);
            List<Test> lst = new List<Test>();
            var result = GetChild(list, 1).ToList();
            foreach (var item in result)
            {
                Console.WriteLine(item.ID);
            }

        }

        static string ForecahList(List<Test> list, int id)
        {
            JArray array = new JArray();
            array.Add(new JValue("张三"));
            array.Add(new JValue("李四"));
            //var result = list.Where(x => x.ParentID == id);
            //if (result != null)
            //{
            //    foreach (var item in result)
            //    {
            //        JObject jObject = new JObject();
            //        jObject.Add("ID", new JValue(item.ID.ToString()));
            //        //var queryJosn = ForecahList(list, item.ID);
            //        //JArray jArray = new JArray();
            //        //jArray.Add(queryJosn.ToString());
            //        //jObject.Add("Node",jArray);
            //        array.Add(new JValue(jObject.ToString()));
            //    }
            //}
            return array.ToString();
        }

        /* 递归获取指定父ID下所有的子集*/
        static IEnumerable<Test> GetChild(List<Test> lst, int id)
        {
            return lst.Where(x => x.ParentID == id).Union(lst.Where(x => x.ParentID == id).SelectMany(y => GetChild(lst, y.ID)));
        }
    }
}
