using EF_Core_DbFirst_Demo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
namespace EF_Core_DbFirst_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var i = new Program().TestAsync().Result;
            Console.WriteLine(i);
            vanfjContext vanfj = new vanfjContext();
            var user = vanfj.Userentity.FirstOrDefault();
            user.Age = 666;
            var user_ = vanfj.Userentity.AsNoTracking().FirstOrDefault(x => x.Id == user.Id);
            vanfj.SaveChanges();

        }

        /// <summary>
        /// 异步操作数据库测试
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<int> TestAsync()
        {
            int k = 0;
            using (vanfjContext context = new vanfjContext())
            {
                //事务
                using (var db = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        List<Userentity> userentities = new List<Userentity>();
                        for (int i = 0; i < 1000; i++)
                        {
                            Userentity userentity = new Userentity()
                            {
                                Age = 23,
                                Phone = "18771506573",
                                Qq = "893703953",
                                UserId = "admin",
                                UserName = "Liu Weichao"
                            };
                            userentities.Add(userentity);
                        }
                        await context.Userentity.AddRangeAsync(userentities);
                        k = await context.SaveChangesAsync();
                        //提交
                        db.Commit();

                    }
                    catch (Exception e)
                    {
                        //回滚
                        db.Rollback();
                    }
                }

            }
            return k;
        }
    }
}
