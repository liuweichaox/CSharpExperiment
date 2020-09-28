using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JsonDemo
{
    /// <summary>  
    /// 学生信息实体  
    /// </summary>  
    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Class Class { get; set; }
    }
    /// <summary>  
    /// 学生班级实体  
    /// </summary>  
    public class Class
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Newtonsoft.Json，是.Net中开源的Json序列化和反序列化工具，官方地址：http://www.newtonsoft.com/json
             * 序列化，反序列化 实体对象，实体集合，匿名对象
             */
            Student stu = new Student();
            stu.ID = 1;
            stu.Name = "张三";
            stu.Class = new Class() { ID = 0121, Name = "CS0121" };

            //使用方法1  
            //实体序列化、反序列化  
            //结果：{"ID":1,"Name":"张三","Class":{"ID":121,"Name":"CS0121"}}  
            string json1 = JsonConvert.SerializeObject(stu);
            Console.WriteLine(json1);
            Student stu2 = JsonConvert.DeserializeObject<Student>(json1);
            Console.WriteLine(stu2.Name + "---" + stu2.Class.Name);

            //实体集合，序列化和反序列化  
            List<Student> stuList = new List<Student>() { stu, stu2 };
            string json2 = JsonConvert.SerializeObject(stuList);
            Console.WriteLine(json2);
            List<Student> stuList2 = JsonConvert.DeserializeObject<List<Student>>(json2);
            foreach (var item in stuList2)
            {
                Console.WriteLine(item.Name + "----" + item.Class.Name);
            }

            //匿名对象的解析,  
            //匿名独享的类型  obj.GetType().Name： "<>f__AnonymousType0`2"  
            var obj = new { ID = 2, Name = "李四" };
            string json3 = JsonConvert.SerializeObject(obj);
            Console.WriteLine(json3);
            object obj2 = JsonConvert.DeserializeAnonymousType(json3, obj);
            Console.WriteLine(obj2.GetType().GetProperty("ID").GetValue(obj2));
            object obj3 = JsonConvert.DeserializeAnonymousType(json3, new { ID = default(int), Name = default(string) });
            Console.WriteLine(obj3.GetType().GetProperty("ID").GetValue(obj3));
            //匿名对象解析，可以传入现有类型，进行转换  
            Student stu3 = new Student();
            stu3 = JsonConvert.DeserializeAnonymousType(json3, new Student());
            Console.WriteLine(stu3.Name);
            /*
             * 控制字符串的序列化
             */
            var students = new { sno = "101", sname = "李军", sex = "男", sbirthday = DateTime.Now, _class = "95033" };
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd";
            string str = JsonConvert.SerializeObject(students, settings);
            Console.WriteLine(str);
            //[javascript] view plain copy
            //{"sno":"101","sname":"李军","ssex":"男","sbirthday":"1976-02-20","_class":"95033"}

            /*
             * 一、LINQ to JSON 常用实例
             */
            JObject o = JObject.Parse
                        (@"{  'CPU': 'Intel',  
                      'Drives': [  
                      'DVD read/writer',  
                      '500 gigabyte hard drive']  }");
            string cpu = (string)o["CPU"];
            Console.WriteLine(cpu);//CPU  
            string firstDrive = (string)o["Drives"][0];
            Console.WriteLine(firstDrive);// DVD read/writer  
            List<string> allDrives = o["Drives"].Select(t => (string)t).ToList();
            foreach (var item in allDrives)
            {
                Console.WriteLine(item); // DVD read/writer  
                                         //500 gigabyte hard drive  
            }
            /*
             *  二、Parsing JSON 将json 字符串转换成对象
             */
            string json = @"{  
                        'CPU': 'Intel',  
                        'Drives': [  
                        'DVD read/writer',  
                        '500 gigabyte hard drive'  
                        ],  
                        'Mouses':{  
                        'one':'小米',  
                        'two':'戴尔'  
                        }  
                        }";
            JObject computer = JObject.Parse(json);
            Console.WriteLine(computer.First.ToString());//"CPU": "Intel"  
            Console.WriteLine(computer.Last.ToString()); //"Mouses": { "one": "小米","two": "戴尔"}  
            string one = computer["Mouses"]["one"].ToString(); //小米  
            Console.WriteLine(one);

            //2.转化数组  
            string json_2 = @"['张三','李四','王五']";
            JArray array2 = JArray.Parse(json_2);
            Console.WriteLine(array2.Type);//Array  
            Console.WriteLine(array2[1]); //李四  

            string json_3 = @"[{name:'张三'},{name:'李四'}]";
            JArray array3 = JArray.Parse(json_3);
            Console.WriteLine(array3.Type);//Array  
            Console.WriteLine(array3[1]);//{ "name": "李四" }  


          
            var str1 = Environment.CurrentDirectory; //取得或设置当前工作目录的完整限定路径
            var str2 = AppDomain.CurrentDomain.BaseDirectory; //获取基目录，它由程序集冲突解决程序用来探测程序集
            var str3 = Environment.CurrentDirectory.ToString();//获取或设置当前工作目录的完全限定路径
            var str4 = Directory.GetCurrentDirectory();//获取应用程序的当前工作目录

            Console.WriteLine(str1);
            Console.WriteLine(str2);
            Console.WriteLine(str3);
            Console.WriteLine(str4);

            //3.转化Json 文件  
            using (StreamReader sr = File.OpenText(str4 + @"\json1.json"))
            {
                JObject ob = (JObject)JToken.ReadFrom(new JsonTextReader(sr));
                Console.WriteLine(ob["CPU"]);//Intel  
                                             //如果没有找到抛出异常  
                                             //举例说明，只是获取 ob["CPU"] 为空不会抛出异常，  
                                             //ob["CPU"]["one"] 如果ob["CPU"] 为空就会抛出异常  
                string result = (string)ob["Mouses"]["one"]; //小米  
                Console.WriteLine(result);
            }

            /*
             *三、Creating JSON 将对象属性转换成json 字符串
             */
            // 动态生成json字符串  
            //1.创建数组  
            JArray array = new JArray();
            array.Add(new JValue("张三"));
            array.Add(new JValue("李四"));
            string json_one = array.ToString();
            Console.WriteLine(json_one);
            //[  
            //  "张三",  
            //  "李四"  
            //]  

            //2.创建对象  
            JObject jobject = new JObject();
            jobject.Add(new JProperty("qq", "893703953"));
            jobject.Add("sex", "女");
            jobject.Add("name", new JValue("张三"));
            jobject.Add("age", new JValue("20"));
            string json_two = jobject.ToString();
            Console.WriteLine(json_two);
            //{  
            //"name": "张三",  
            //  "age": "20"  
            //}  

            //3.创建对象方式2,使用匿名对象  
            JObject obj_three = JObject.FromObject(new { name = "李四", Birthday = DateTime.Now });
            string json_three = obj_three.ToString();
            Console.WriteLine(json_three);
            //{  
            //"name": "李四",  
            //  "Birthday": "2016-09-09T14:53:23.0307889+08:00"  
            //}  
            /*
             * 四、Querying JSON with LINQ 解析获取json 字符串中的内容
             */
            //Querying with LINQ  
            string _json = @"{  
                        school:{  
                        name:'实验高中',  
                        students:[  
                            {name:'张三',age:18},  
                            {name:'李四',age:19}  
                        ],  
                        sites:['济南','聊城']  
                        }  
                    }";
            JObject oo = JObject.Parse(_json);
            string schname = (string)oo["school"]["name"];
            Console.WriteLine(schname); //实验高中  
            string stuname = (string)oo["school"]["students"][1]["name"];
            Console.WriteLine(stuname);//李四  
            JArray sites = (JArray)oo["school"]["sites"];
            foreach (var item in sites)
            {
                Console.WriteLine(item); //济南  //聊城  
            }
            IList<string> siteList = sites.Select(q => (string)q).ToList();
            Console.WriteLine(sites.Count);//2 
                                           /*
                                           * 五、Querying JSON width SelectToken 使用SelectToken 方式获取json 字符串中的内容
                                           */
                                           //  Querying JSON with SelectToken  
            string _json_ = @"{  
                        school:{  
                        name:'实验高中',  
                        students:[  
                            {name:'张三',age:18},  
                            {name:'李四',age:19}  
                        ],  
                        sites:['济南','聊城']  
                        }  
                    }";
            JObject ooo = JObject.Parse(_json_);
            //SelectToken 方法使用  
            string schname1 = (string)ooo.SelectToken("school.name");
            Console.WriteLine(schname1); //实验高中  
            string stuname1 = (string)ooo.SelectToken("school.students[1].name");
            Console.WriteLine(stuname1); //李四  
                                         //SelectToken with JSONPath  
            JToken stu1 = ooo.SelectToken("$.school.students[?(@.name=='张三')]");
            Console.WriteLine(stu1); //{"name": "张三","age": 18}  
            Console.WriteLine(stu1["age"]); //18  
            IEnumerable<JToken> stus = ooo.SelectTokens("$..students[?(@.age>15)]");
            foreach (var item in stus)
            {
                Console.WriteLine(item); //{"name": "张三",   "age": 18 }    
                                         //{ "name": "李四",   "age": 19}  
            }
            //SelectToken with LINQ  
            // $...name  意思是从当前接口文档的1,2,3级中查找name，并返回结果  
            IList<string> names = ooo.SelectTokens("$...name").Select(q => (string)q).ToList();
            Console.WriteLine(string.Join(",", names)); //实验高中,张三,李四  

        }
    }
}
