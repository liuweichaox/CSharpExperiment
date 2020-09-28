using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MyNetDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            DriveInfo[] driveInfo = DriveInfo.GetDrives();
            foreach (var item in driveInfo)
            {
                //使用自定义遍历方法
                var list = ForeachClass.ForeachClassProperties<DriveInfo>(item);
                foreach (var str in list)
                {
                    Console.WriteLine("key : " + str.Key + "\t\tvalue : " + str.Value);
                }
            }
            Console.ReadKey();
        }
    }
    /// <summary>
    /// C#反射遍历对象类
    /// </summary>
    public class ForeachClass
    {
        /// <summary>
        /// C#反射遍历对象属性
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="model">对象</param>
        public static Dictionary<string, string> ForeachClassProperties<T>(T model)
        {
            //List<string> list = new List<string>();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Type t = model.GetType();
            PropertyInfo[] PropertyList = t.GetProperties();
            foreach (PropertyInfo item in PropertyList)
            {
                var name = item.Name;
                var value = item.GetValue(model).ToString();
                //list.Add(value);
                dictionary.Add(name, value);
            }
            //return list;
            return dictionary;
        }
    }
}
