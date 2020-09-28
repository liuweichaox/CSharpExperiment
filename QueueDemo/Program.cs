/*
 队列（Queue）代表了一个先进先出的对象集合。当您需要对各项进行先进先出的访问时，则使用队列。当您在列表中添加一项，称为入队，当您从列表中移除一项时，称为出队。
Queue 类的方法和属性
下表列出了 Queue 类的一些常用的 属性：

属性	描述
Count	获取 Queue 中包含的元素个数。

下表列出了 Queue 类的一些常用的 方法：

序号	方法名                                       描述
1	    public virtual void Clear(); 从 Queue        中移除所有的元素。
2	    public virtual bool Contains( object obj );  判断某个元素是否在 Queue 中。
3	    public virtual object Dequeue();             移除并返回在 Queue 的开头的对象。
4	    public virtual void Enqueue( object obj );   向 Queue 的末尾添加一个对象。
5	    public virtual object[] ToArray();           复制 Queue 到一个新的数组中。
6	    public virtual void TrimToSize();            设置容量为 Queue 中元素的实际个数。
 */
 using System;
using System.Collections;

namespace MyNetDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue q = new Queue();

            q.Enqueue('A');
            q.Enqueue('M');
            q.Enqueue('G');
            q.Enqueue('W');

            Console.WriteLine("Current queue: ");
            foreach (char c in q)
                Console.Write(c + " ");
            Console.WriteLine();
            q.Enqueue('V');
            q.Enqueue('H');
            Console.WriteLine("Current queue: ");
            foreach (char c in q)
                Console.Write(c + " ");
            Console.WriteLine();
            Console.WriteLine("Removing some values ");
            char ch = (char)q.Dequeue();
            Console.WriteLine("The removed value: {0}", ch);
            ch = (char)q.Dequeue();
            Console.WriteLine("The removed value: {0}", ch);
            Console.ReadKey();
        }
    }
}
