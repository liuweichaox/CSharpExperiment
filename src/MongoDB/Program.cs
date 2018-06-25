/* MongoDB 中文官网：http://www.mongodb.org.cn/
 * MongoDB 下载地址： http://dl.mongodb.org/dl/win32/x86_64
 * 创建数据目录：MongoDB将数据目录存储在 db 目录下。C：\data\db
 * 创建日志目录：MongoDB将数据目录存储在 log 目录下。C：\data\log
 * 从 MongoDB 目录的 bin 目录中执行 mongod.exe 文件运行 MongoDB 服务器
 * C# NuGet包
 * MongoDB.Driver
 * MongoDB.Driver.Core
 * C#.Net文档：https://docs.mongodb.com/ecosystem/drivers/csharp/
 */
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoDB
{
    public class UserInfo
    {
        /// <summary>
        /// MongoDB默认Id为主键，BsonId，数据库自动生成
        /// </summary>
        [BsonId]
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public bool Sex { get; set; }

        public List<RoleInfo> Roles { get; set; }

        public AuthInfo Auth { get; set; }
    }
    public class RoleInfo
    {
        public string RoleName { get; set; }
    }

    public class AuthInfo
    {
        public string AuthName { get; set; }
    }
    class Program
    {

        static void Main(string[] args)
        {
            //===============================================================================================================
            //                                            建立连接
            //该MongoClient实例实际上代表了一个到数据库的连接池; 即使有多个线程，您也只需要MongoClient类的一个实例。
            //===============================================================================================================
            MongoClient client = new MongoClient("mongodb://localhost:27017");

            //===============================================================================================================
            //                                            获取数据库
            //如果数据库尚不存在，那也没问题。它将在第一次使用时创建。
            //===============================================================================================================
            var database = client.GetDatabase("mongo");

            //===============================================================================================================
            //                                            获取集合
            //如果该集合尚不存在，那也没关系。它将在第一次使用时创建。
            //===============================================================================================================
            var collection = database.GetCollection<UserInfo>("UserInfo");

            List<UserInfo> users = new List<UserInfo>()
            {
                new UserInfo()
                {
                    UserName="刘大大",
                    Age =23,
                    Sex =true,
                    Email ="893703953@qq.com",
                    Phone ="18771506573",
                    Auth =new AuthInfo()
                    {
                        AuthName ="这里是一个对象"
                    },
                    Roles =new List<RoleInfo>()
                    {
                        new RoleInfo()
                        {
                            RoleName ="管理员"
                        },
                        new RoleInfo()
                        {
                            RoleName ="超级管理员"
                        }
                    }
                },
                new UserInfo()
                {
                    UserName="青睐",
                    Age =23,
                    Sex =true,
                    Email ="123456@qq.com",
                    Phone ="13125005035",
                    Auth =new AuthInfo()
                    {
                        AuthName ="这里也是一个对象"
                    },
                    Roles =new List<RoleInfo>()
                    {
                        new RoleInfo()
                        {
                            RoleName ="最高权限"
                        },
                        new RoleInfo()
                        {
                            RoleName ="主席"
                        }
                    }
                }
            };
            //新增数据
            collection.InsertMany(users);
            //查询数据
            var findall = collection.AsQueryable().ToList();
            //修改数据
            collection.UpdateMany(x => x.Age == 23, Builders<UserInfo>.Update.Set("UserName", "习大大"));
            //删除数据
            collection.DeleteMany<UserInfo>(x => x.Sex == true);

            //===============================================================================================================
            //                                            MongoDbCsharpHelper
            //===============================================================================================================
            string connectionString = "mongodb://localhost:27017";
            string databaseName = "mongo";

            MongoDbCsharpHelper csharpHelper = new MongoDbCsharpHelper(connectionString, databaseName, true, true);
            csharpHelper.InsertMany<UserInfo>(users);
            csharpHelper.Find<UserInfo>(x => x.Id != null);
            csharpHelper.UpdateMany<UserInfo>(x => x.Age == 23,Builders<UserInfo>.Update.Set(x=>x.UserName,"习大大"));
            csharpHelper.DeleteMany<UserInfo>(x => x.Age == 23);
            Console.ReadKey();
        }
    }
}
