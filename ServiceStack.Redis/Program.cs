/**********************************************************************************************************************************************
 * Redis是一个开源，高级的键值存储和一个适用的解决方案，用于构建高性能，可扩展的Web应用程序。
 * Redis有三个主要特点，使它优越于其它键值数据存储系统 -
 *     Redis将其数据库完全保存在内存中，仅使用磁盘进行持久化。
 *     与其它键值数据存储相比，Redis有一组相对丰富的数据类型。
 *     Redis可以将数据复制到任意数量的从机中。
 * Redis的优点
 *     异常快 - Redis非常快，每秒可执行大约110000次的设置(SET)操作，每秒大约可执行81000次的读取/获取(GET)操作。
 *     支持丰富的数据类型 - Redis支持开发人员常用的大多数数据类型，例如列表，集合，排序集和散列等等。这使得Redis很容易被用来解决各种问题，因为我们知道哪些问题可以更好使用地哪些数据类型来处理解决。
 *     操作具有原子性 - 所有Redis操作都是原子操作，这确保如果两个客户端并发访问，Redis服务器能接收更新的值。
 *     多实用工具 - Redis是一个多实用工具，可用于多种用例，如：缓存，消息队列(Redis本地支持发布/订阅)，应用程序中的任何短期数据，例如，web应用程序中的会话，网页命中计数等。
 * Redis与其他键值存储系统
 *     Redis是键值数据库系统的不同进化路线，它的值可以包含更复杂的数据类型，可在这些数据类型上定义原子操作。
 *     Redis是一个内存数据库，但在磁盘数据库上是持久的，因此它代表了一个不同的权衡，在这种情况下，在不能大于存储器(内存)的数据集的限制下实现非常高的写和读速度。
 *     内存数据库的另一个优点是，它与磁盘上的相同数据结构相比，复杂数据结构在内存中存储表示更容易操作。 因此，Redis可以做很少的内部复杂性。
 * Redis 数据类型
 *     Redis支持五种数据类型：string（字符串），hash（哈希），list（列表），set（集合）及zset(sorted set：有序集合)。
 * Window 下安装
 *      下载地址：https://github.com/MSOpenTech/redis/releases
 *      Redis 支持 32 位和 64 位。这个需要根据你系统平台的实际情况选择，这里我们下载 Redis-x64-xxx.zip压缩包到 C 盘，解压后，将文件夹重新命名为 redis。
 * C#连接
 * NuGet包 Install-Package StackExchange.Redis
 * 由于Redis一般是用来作为缓存的，也就是一般我们把一些不经常改变的数据通过Redis缓存起来，之后用户的请求就不需要再访问数据库，而可以直接从Redis缓
 * 存中直接获取，这样就可以减轻数据库服务器的压力以及加快响应速度。既然是用来做缓存的，也就是通过指定key值来把对应Value保存起来，之后再根据key值
 * 来获得之前缓存的值。
 * 
 * 如何要想查看自己操作是否成功，也可以像MongoDB那样下载一个客户端工具，这里推荐一款Redis Desktop Manager。这个工具就相当于SQL Server的客户端工具
 * 一样。通过这款工具可以查看Redis服务器中保存的数据和对应格式。其使用也非常简单，只需要添加一个Redis服务连接即可。
 * Redis Desktop Manager下载地址：http://pan.baidu.com/s/1sjp55Ul
 * Redis 命令大全：http://doc.redisfans.com/
**********************************************************************************************************************************************/
using StackExchange.Redis;
using System;
using System.Threading;
using System.Text;
using System.Text.Json;

namespace Redis
{
    class Program
    {
        static ConnectionMultiplexer connection;
        static void Main(string[] args)
        {
            //在Redis中存储常用的5种数据类型：String,Hash,List,SetSorted set
            connection = ConnectionMultiplexer.Connect("localhost:6379,Password=123456,ConnectTimeout=1000,ConnectRetry=1,SyncTimeout=10000");
            var client = connection.GetDatabase(0);
            AddString(client);
            AddHash(client);
            AddList(client);
            AddSet(client);
            AddSetSorted(client);
            Console.ReadLine();
        }

        /// <summary>
        /// 添加字符串
        /// <para>string是redis最基本的类型，你可以理解成与Memcached一模一样的类型，一个key对应一个value。</para>
        /// <para>string类型是二进制安全的。意思是redis的string可以包含任何数据。比如jpg图片或者序列化的对象 。</para>
        /// <para>string类型是Redis最基本的数据类型，一个键最大能存储512MB。</para>
        /// </summary>
        /// <param name="client"></param>
        private static void AddString(IDatabase client)
        {
            var timeOut = new TimeSpan(0, 0, 0, 10);
            client.StringSet("Test", "Learninghard", timeOut);
            while (true)
            {
                if (client.KeyExists("Test"))
                {
                    Console.WriteLine("String Key: Test -Value: {0}, 当前时间: {1}", client.StringGet("Test"), DateTime.Now);
                    Thread.Sleep(3000);
                }
                else
                {
                    Console.WriteLine("Value 已经过期了，当前时间：{0}", DateTime.Now);
                    break;
                }
            }

            var person = new Person() { Name = "Learninghard", Age = 26 };
            client.StringSet("lh", JsonSerializer.Serialize(person));
            var cachePerson = JsonSerializer.Deserialize<Person>(client.StringGet("lh"));
            Console.WriteLine("Person's Name is : {0}, Age: {1}", cachePerson.Name, cachePerson.Age);
        }

        /// <summary>
        /// 添加哈希
        /// <para>Redis hash 是一个键值(key=>value)对集合。</para>
        /// <para>Redis hash是一个string类型的field和value的映射表，hash特别适合用于存储对象。</para>       
        /// </summary>
        /// <param name="client"></param>        
        private static void AddHash(IDatabase client)
        {
            if (client == null) throw new ArgumentNullException("client");

            client.HashSet("HashId", "Name", "Learninghard");
            client.HashSet("HashId", "Age", "26");
            client.HashSet("HashId", "Sex", "男");

            var hashKeys = client.HashGetAll("HashId");
            foreach (var key in hashKeys)
            {
                Console.WriteLine("HashId--Key:{0}", key);
            }

            var haskValues = client.HashValues("HashId");
            foreach (var value in haskValues)
            {
                Console.WriteLine("HashId--Value:{0}", value);
            }

            var allKeys = connection.GetServer("localhost:6379").Keys(0); //获取所有的key。
            foreach (var key in allKeys)
            {
                Console.WriteLine("AllKey--Key:{0}", key);
            }
        }

        /// <summary>
        /// 添加列表
        /// <para>Redis 列表是简单的字符串列表，按照插入顺序排序。你可以添加一个元素到列表的头部（左边）或者尾部（右边）。</para>
        /// </summary>
        /// <param name="client"></param>
        private static void AddList(IDatabase client)
        {
            if (client == null) throw new ArgumentNullException("client");

            client.ListRightPush("QueueListId", "1.Learnghard");  //入队
            client.ListRightPush("QueueListId", "2.张三");
            client.ListRightPush("QueueListId", "3.李四");
            client.ListRightPush("QueueListId", "4.王五");
            var queueCount = client.ListRange("QueueListId");
            foreach (var item in queueCount)
            {
                Console.WriteLine("QueueListId出队值：{0}", client.ListLeftPop("QueueListId"));   //出队(队列先进先出)
            }

            client.ListRightPush("StackListId", "1.Learninghard");  //入栈
            client.ListRightPush("StackListId", "2.张三");
            client.ListRightPush("StackListId", "3.李四");
            client.ListRightPush("StackListId", "4.王五");

            var stackCount = client.ListRange("StackListId");
            foreach (var item in queueCount)
            {
                Console.WriteLine("StackListId出栈值：{0}", client.ListRightPop("StackListId"));   //出栈(栈先进后出)
            }
        }

        /// <summary>
        /// 添加集合
        /// <para>它是string类型的无序集合。set是通过hash table实现的，添加，删除和查找,对集合我们可以取并集，交集，差集</para>
        /// </summary>
        /// <param name="client"></param>
        private static void AddSet(IDatabase client)
        {
            if (client == null) throw new ArgumentNullException("client");

            client.SetAdd("Set1001", "A");
            client.SetAdd("Set1001", "B");
            client.SetAdd("Set1001", "C");
            client.SetAdd("Set1001", "D");
            var hastset1 = client.SetMembers("Set1001");
            foreach (var item in hastset1)
            {
                Console.WriteLine("Set无序集合Value:{0}", item); //出来的结果是无序的
            }

            client.SetAdd("Set1002", "K");
            client.SetAdd("Set1002", "C");
            client.SetAdd("Set1002", "A");
            client.SetAdd("Set1002", "J");
            var hastset2 = client.SetMembers("Set1002");
            foreach (var item in hastset2)
            {
                Console.WriteLine("Set无序集合ValueB:{0}", item); //出来的结果是无序的
            }

            var hashUnion = client.SetCombine(SetOperation.Union, "Set1001", "Set1002");
            foreach (var item in hashUnion)
            {
                Console.WriteLine("求Set1001和Set1002的并集:{0}", item); //并集
            }

            var hashG = client.SetCombine(SetOperation.Intersect, "Set1001", "Set1002");
            foreach (var item in hashG)
            {
                Console.WriteLine("求Set1001和Set1002的交集:{0}", item);  //交集
            }

            var hashD = client.SetCombine(SetOperation.Difference, "Set1001", "Set1002"); //[返回存在于第一个集合，但是不存在于其他集合的数据。差集]
            foreach (var item in hashD)
            {
                Console.WriteLine("求Set1001和Set1002的差集:{0}", item);  //差集
            }

        }

        /// <summary>  
        /// 添加有序集合
        /// <para>sorted set 是set的一个升级版本，它在set的基础上增加了一个顺序的属性，这一属性在添加修改.元素的时候可以指定，</para>
        /// <para>每次指定后，zset(表示有序集合)会自动重新按新的值调整顺序。可以理解为有列的表，一列存 value,一列存顺序。操作中key理解为zset的名字.</para>
        /// </summary>
        /// <param name="client"></param>
        private static void AddSetSorted(IDatabase client)
        {
            if (client == null) throw new ArgumentNullException("client");

            client.SortedSetAdd("SetSorted1001", "A", 1);
            client.SortedSetAdd("SetSorted1001", "B", 1);
            client.SortedSetAdd("SetSorted1001", "C", 1);
            var listSetSorted = client.SortedSetRangeByRank("SetSorted1001");
            foreach (var item in listSetSorted)
            {
                Console.WriteLine("SetSorted有序集合{0}", item);
            }

            client.SortedSetAdd("SetSorted1002", "A", 400);
            client.SortedSetAdd("SetSorted1002", "D", 200);
            client.SortedSetAdd("SetSorted1002", "B", 300);

            // 升序获取第一个值:"D"
            var list = client.SortedSetRangeByRank("SetSorted1002", 0, 0);

            foreach (var item in list)
            {
                Console.WriteLine(item);
            }

            //降序获取第一个值:"A"
            list = client.SortedSetRangeByRank("SetSorted1002", 0, 0, Order.Descending);

            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }
    }

    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
