using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MongoDB
{
    /// <summary>
    /// MongoDbCsharpHelper
    /// <para>MongoDb基于C#语言操作帮助类</para>
    /// <para>Author:刘大大</para>
    /// <para>Date:2018/5/30</para>
    /// </summary>
    public class MongoDbCsharpHelper
    {
        private readonly string connectionString = null;
        private readonly string databaseName = null;
        private readonly bool autoCreateDb = false;
        private readonly bool autoCreateCollection = false;
        private IMongoDatabase database = null;

        static MongoDbCsharpHelper()
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
        }

        public MongoDbCsharpHelper(string mongoConnStr, string dbName, bool autoCreateDb = false, bool autoCreateCollection = false)
        {
            this.connectionString = mongoConnStr;
            this.databaseName = dbName;
            this.autoCreateDb = autoCreateDb;
            this.autoCreateCollection = autoCreateCollection;
        }

        #region 私有方法

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        private MongoClient CreateMongoClient()
        {
            return new MongoClient(connectionString);
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <returns></returns>
        private IMongoDatabase GetMongoDatabase()
        {
            if (database == null)
            {
                var client = CreateMongoClient();
                if (!DatabaseExists(client, databaseName) && !autoCreateDb)
                {
                    throw new KeyNotFoundException("此MongoDB名称不存在：" + databaseName);
                }

                database = CreateMongoClient().GetDatabase(databaseName);
            }

            return database;
        }

        /// <summary>
        /// 判断数据库是否存在
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        private bool DatabaseExists(MongoClient client, string dbName)
        {
            try
            {
                var dbNames = client.ListDatabases().ToList().Select(db => db.GetValue("name").AsString);
                return dbNames.Contains(dbName);
            }
            catch //如果连接的账号不能枚举出所有DB会报错，则默认为true
            {
                return true;
            }

        }

        /// <summary>
        /// 判断集合是否存在
        /// </summary>
        /// <param name="database"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        private bool CollectionExists(IMongoDatabase database, string collectionName)
        {
            var options = new ListCollectionsOptions
            {
                Filter = Builders<BsonDocument>.Filter.Eq("name", collectionName)
            };

            return database.ListCollections(options).ToEnumerable().Any();
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="name"></param>
        /// <param name="settings"></param>
        /// <returns></returns>

        private IMongoCollection<TDoc> GetMongoCollection<TDoc>(string name, MongoCollectionSettings settings = null)
        {
            var mongoDatabase = GetMongoDatabase();

            if (!CollectionExists(mongoDatabase, name) && !autoCreateCollection)
            {
                throw new KeyNotFoundException("此Collection名称不存在：" + name);
            }

            return mongoDatabase.GetCollection<TDoc>(name, settings);
        }

        /// <summary>
        /// 构建更新操作定义
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="doc"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private List<UpdateDefinition<TDoc>> BuildUpdateDefinition<TDoc>(object doc, string parent)
        {
            var updateList = new List<UpdateDefinition<TDoc>>();
            foreach (var property in typeof(TDoc).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var key = parent == null ? property.Name : string.Format("{0}.{1}", parent, property.Name);
                //非空的复杂类型
                if ((property.PropertyType.IsClass || property.PropertyType.IsInterface) && property.PropertyType != typeof(string) && property.GetValue(doc) != null)
                {
                    if (typeof(IList).IsAssignableFrom(property.PropertyType))
                    {
                        #region 集合类型
                        int i = 0;
                        var subObj = property.GetValue(doc);
                        foreach (var item in subObj as IList)
                        {
                            if (item.GetType().IsClass || item.GetType().IsInterface)
                            {
                                updateList.AddRange(BuildUpdateDefinition<TDoc>(doc, string.Format("{0}.{1}", key, i)));
                            }
                            else
                            {
                                updateList.Add(Builders<TDoc>.Update.Set(string.Format("{0}.{1}", key, i), item));
                            }
                            i++;
                        }
                        #endregion
                    }
                    else
                    {
                        #region 实体类型
                        //复杂类型，导航属性，类对象和集合对象 
                        var subObj = property.GetValue(doc);
                        foreach (var sub in property.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            updateList.Add(Builders<TDoc>.Update.Set(string.Format("{0}.{1}", key, sub.Name), sub.GetValue(subObj)));
                        }
                        #endregion
                    }
                }
                else //简单类型
                {
                    updateList.Add(Builders<TDoc>.Update.Set(key, property.GetValue(doc)));
                }
            }

            return updateList;
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="col"></param>
        /// <param name="indexFields"></param>
        /// <param name="options"></param>
        private void CreateIndex<TDoc>(IMongoCollection<TDoc> col, string[] indexFields, CreateIndexOptions options = null)
        {
            if (indexFields == null)
            {
                return;
            }
            var indexKeys = Builders<TDoc>.IndexKeys;
            IndexKeysDefinition<TDoc> keys = null;
            if (indexFields.Length > 0)
            {
                keys = indexKeys.Descending(indexFields[0]);
            }
            for (var i = 1; i < indexFields.Length; i++)
            {
                var strIndex = indexFields[i];
                keys = keys.Descending(strIndex);
            }

            if (keys != null)
            {
                col.Indexes.CreateOne(keys, options);
            }

        }

        #endregion

        /// <summary>
        /// 创建集合索引
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="indexFields"></param>
        /// <param name="options"></param>
        public void CreateCollectionIndex<TDoc>(string collectionName, string[] indexFields, CreateIndexOptions options = null)
        {
            CreateIndex(GetMongoCollection<TDoc>(collectionName), indexFields, options);
        }

        /// <summary>
        /// 创建集合
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="indexFields"></param>
        /// <param name="options"></param>
        public void CreateCollection<TDoc>(string[] indexFields = null, CreateIndexOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            CreateCollection<TDoc>(collectionName, indexFields, options);
        }

        /// <summary>
        /// 创建集合
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="indexFields"></param>
        /// <param name="options"></param>
        public void CreateCollection<TDoc>(string collectionName, string[] indexFields = null, CreateIndexOptions options = null)
        {
            var mongoDatabase = GetMongoDatabase();
            mongoDatabase.CreateCollection(collectionName);
            CreateIndex(GetMongoCollection<TDoc>(collectionName), indexFields, options);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<TDoc> Find<TDoc>(Expression<Func<TDoc, bool>> filter, FindOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            return Find<TDoc>(collectionName, filter, options);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<TDoc> Find<TDoc>(string collectionName, Expression<Func<TDoc, bool>> filter, FindOptions options = null)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            return colleciton.Find(filter, options).ToList();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="filter"></param>
        /// <param name="keySelector"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rsCount"></param>
        /// <returns></returns>
        public List<TDoc> FindByPage<TDoc, TResult>(Expression<Func<TDoc, bool>> filter, Expression<Func<TDoc, TResult>> keySelector, int pageIndex, int pageSize, out int rsCount)
        {
            string collectionName = typeof(TDoc).Name;
            return FindByPage<TDoc, TResult>(collectionName, filter, keySelector, pageIndex, pageSize, out rsCount);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="keySelector"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rsCount"></param>
        /// <returns></returns>
        public List<TDoc> FindByPage<TDoc, TResult>(string collectionName, Expression<Func<TDoc, bool>> filter, Expression<Func<TDoc, TResult>> keySelector, int pageIndex, int pageSize, out int rsCount)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            rsCount = colleciton.AsQueryable().Where(filter).Count();

            int pageCount = rsCount / pageSize + ((rsCount % pageSize) > 0 ? 1 : 0);
            if (pageIndex > pageCount) pageIndex = pageCount;
            if (pageIndex <= 0) pageIndex = 1;

            return colleciton.AsQueryable(new AggregateOptions { AllowDiskUse = true }).Where(filter).OrderByDescending(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// 添加单个实体
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="doc"></param>
        /// <param name="options"></param>
        public void Insert<TDoc>(TDoc doc, InsertOneOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            Insert<TDoc>(collectionName, doc, options);
        }

        public void Insert<TDoc>(string collectionName, TDoc doc, InsertOneOptions options = null)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            colleciton.InsertOne(doc, options);
        }

        /// <summary>
        /// 添加单个实体
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="docs"></param>
        /// <param name="options"></param>
        public void InsertMany<TDoc>(IEnumerable<TDoc> docs, InsertManyOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            InsertMany<TDoc>(collectionName, docs, options);
        }

        /// <summary>
        /// 添加多个实体
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="docs"></param>
        /// <param name="options"></param>
        public void InsertMany<TDoc>(string collectionName, IEnumerable<TDoc> docs, InsertManyOptions options = null)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            colleciton.InsertMany(docs, options);
        }

        /// <summary>
        /// 更新个实体
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        public void Update<TDoc>(Expression<Func<TDoc, bool>> filter, UpdateDefinition<TDoc> update, UpdateOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            Update<TDoc>(collectionName, filter, update, options);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        public void Update<TDoc>(string collectionName, Expression<Func<TDoc, bool>> filter, UpdateDefinition<TDoc> update, UpdateOptions options = null)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            colleciton.UpdateOne(filter, update, options);
        }

        /// <summary>
        /// 更新多个实体
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        public void UpdateMany<TDoc>(Expression<Func<TDoc, bool>> filter, UpdateDefinition<TDoc> update, UpdateOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            UpdateMany<TDoc>(collectionName,filter, update, options);
        }

        /// <summary>
        /// 更新多个实体
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        public void UpdateMany<TDoc>(string collectionName, Expression<Func<TDoc, bool>> filter, UpdateDefinition<TDoc> update, UpdateOptions options = null)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            colleciton.UpdateMany(filter, update, options);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        public void Delete<TDoc>(Expression<Func<TDoc, bool>> filter, DeleteOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            Delete<TDoc>(collectionName, filter, options);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        public void Delete<TDoc>(string collectionName, Expression<Func<TDoc, bool>> filter, DeleteOptions options = null)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            colleciton.DeleteOne(filter, options);
        }

        /// <summary>
        /// 删除多个实体
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        public void DeleteMany<TDoc>(Expression<Func<TDoc, bool>> filter, DeleteOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            DeleteMany<TDoc>(collectionName, filter, options);
        }

        /// <summary>
        /// 删除多个实体
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        public void DeleteMany<TDoc>(string collectionName, Expression<Func<TDoc, bool>> filter, DeleteOptions options = null)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            colleciton.DeleteMany(filter, options);
        }

        /// <summary>
        /// 清空集合
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="collectionName"></param>
        public void ClearCollection<TDoc>(string collectionName)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            var inddexs = colleciton.Indexes.List();
            List<IEnumerable<BsonDocument>> docIndexs = new List<IEnumerable<BsonDocument>>();
            while (inddexs.MoveNext())
            {
                docIndexs.Add(inddexs.Current);
            }
            var mongoDatabase = GetMongoDatabase();
            mongoDatabase.DropCollection(collectionName);

            if (!CollectionExists(mongoDatabase, collectionName))
            {
                CreateCollection<TDoc>(collectionName);
            }

            if (docIndexs.Count > 0)
            {
                colleciton = mongoDatabase.GetCollection<TDoc>(collectionName);
                foreach (var index in docIndexs)
                {
                    foreach (IndexKeysDefinition<TDoc> indexItem in index)
                    {
                        try
                        {
                            colleciton.Indexes.CreateOne(indexItem);
                        }
                        catch
                        { }
                    }
                }
            }

        }
    }
}