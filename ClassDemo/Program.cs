/*
 C# 中的构造函数
类的 构造函数 是类的一个特殊的成员函数，当创建类的新对象时执行。
构造函数的名称与类的名称完全相同，它没有任何返回类型。
默认的构造函数没有任何参数。但是如果你需要一个带有参数的构造函数可以有参数，这种构造函数叫做参数化构造函数。

C# 中的析构函数
类的 析构函数 是类的一个特殊的成员函数，当类的对象超出范围时执行。
析构函数的名称是在类的名称前加上一个波浪形（~）作为前缀，它不返回值，也不带任何参数。
析构函数用于在结束程序（比如关闭文件、释放内存等）之前释放资源。析构函数不能继承或重载。
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace MyNetDemo
{
    class Line
    {
        private static int count;

        private double length;   // 线条的长度
        /// <summary>
        /// 构造函数在类被实例化的时候执行
        /// </summary>
        public Line()  // 构造函数
        {
            count++;
            Console.WriteLine("对象已创建");
        }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static Line()
        {
            Console.WriteLine("静态构造函数");
            count++;
        }
        ~Line() //析构函数
        {
            Console.WriteLine("对象已删除");
        }

        public void setLength(double len)
        {
            length = len;
        }
        public double getLength()
        {
            return length;
        }

        static void Main(string[] args)

        {
            Line line = new Line();
            Line line1 = new Line();
            //在这里调用count,值为3，静态构造函数时为1，实例化两次，每次值+1（静态字段直接通过类调用，不需要实例化）
            Console.WriteLine(Line.count);
            // 设置线条长度
            line.setLength(6.0);
            Console.WriteLine("线条的长度： {0}", line.getLength());
            foreach (var item in MyFun())
            {
                Console.WriteLine(item);
            }
            //实例化父类
            ParentClass parentClass = new ParentClass();
            parentClass.WriteStr();
            parentClass.MyVirual();
            parentClass.MyIFun();
            parentClass.MyabstractFun();
            parentClass.MyBaseClassFun();

            Console.WriteLine(GetDrinkMany(100));
            Console.WriteLine(run(100,0));
            Console.ReadKey();
        }

        /// <summary>
        /// 一瓶啤酒3块钱，两个瓶盖可以兑换一瓶啤酒，N块钱可以喝多少瓶啤酒
        /// </summary>
        /// <param name="money">钱</param>
        /// <returns></returns>
        public static int GetDrinkMany(int money)
        {
            var unitPrice = 3 / 2.0;//一个瓶盖值多少钱
            var count = money / 3;//直接可以买多少瓶
            var useMany = count;//喝过多少瓶
            if (count > 1)
            {
                var exchangeCount = (count / 2);//可以兑换多少个瓶子
                useMany += exchangeCount;//喝过的瓶子累加
                count = count - (exchangeCount * 2) + exchangeCount;//酒瓶的数量要减去兑换的啤酒的消耗的数量，再加上兑换到的酒瓶数量
                if (count > 1)
                {
                    useMany += GetDrinkMany((int)(count * unitPrice));
                }

            }
            return useMany;
        }

        /// <summary>
        /// 精简版案例
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int run(int m,int n)
        {
            var a = (int)(m / 3); //瓶数
            var b = a > 1 ? (int)(a % 2) : 0; //兑换后剩余瓶数
            var c = (a - b + n) / 2 * 3; //可兑换瓶数转换成对应钱

            return a + (a + n > 1 ? run(c, b) : 0);
        }

        /// <summary>
        ///yield return
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<int> MyFun()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return i;
            }
        }
    }

    /// <summary>
    /// 基类
    /// </summary>
    public class BaseClass
    {
        /// <summary>
        /// 基类实体方法[外部调用此方法，必须加public访问修饰符]
        /// </summary>
        public void MyBaseClassFun()
        {
            Console.WriteLine("我是来自BaseClass，MyBaseClassFun");
        }
    }
    /// <summary>
    /// 接口
    /// </summary>
    public interface IBaseinterface
    {
        void MyIFun();
    }

    /// <summary>
    ///抽象类[可以继承类、接口]
    /// </summary>
    public abstract class Myabstract : BaseClass, IBaseinterface
    {
        /// <summary>
        /// 抽象方法[抽象方法必须写在抽象类中]
        /// </summary>
        public abstract void MyabstractFun();

        /// <summary>
        /// 抽象类中的实体方法
        /// </summary>
        public void WriteStr()
        {
            Console.WriteLine("我是来自Myabstract，WriteStr");
        }

        /// <summary>
        /// 虚方法[可以被重写]
        /// </summary>
        public virtual void MyVirual()
        {
            Console.WriteLine("我是来自Myabstract，MyVirual");
        }

        /// <summary>
        ///抽象类中对接口实现
        /// </summary>
        public void MyIFun()
        {
            Console.WriteLine("我是来自Myabstract，我是接口的实现");
        }

    }
    /// <summary>
    /// 父类[继承抽象类]
    /// </summary>
    public class ParentClass : Myabstract
    {

        /// <summary>
        /// 实现抽象方法
        /// </summary>
        public override void MyabstractFun()
        {
            Console.WriteLine("我是来自ParentClass，MyabstractFun");
        }

        /// <summary>
        /// 重写虚方法
        /// </summary>
        public override void MyVirual()
        {
            Console.WriteLine("我是来自ParentClass，我是被重写的虚方法");
        }
    }
}
