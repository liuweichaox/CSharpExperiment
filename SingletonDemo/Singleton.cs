/*
 Net设计模式实例之单例模式( Singleton Pattern)

 一 ： 单例模式的简介：(Brief Introduction)

单例模式（Singleton Pattern）,保证一个类只有一个实例，并提供一个访问它的全局访问点。单例模式因为Singleton封装它的唯一实例，它就可以严格地控制客户怎样访问它以及何时访问它。简单说就是单一模式：仅能有你一个人访问；

二、解决的问题（What To Solve）

 当一个类只允许创建一个实例时，可以考虑使用单例模式。    

 三．单例模式分析（Analysis）

Singleton类，定义一个私有变量instance;私有构造方法Singleton()和方法GetInstance();

私有变量instance:

单例模式结构：

private static Singleton instance;

私有构造方法Singleton(),外界不能使用new关键字来创建此类的实例了。

private Singleton()

{

}

方法GetInstance(), 此方法是本类实例的唯一全局访问点。

public static Singleton GetInstance()

{

    //如实例不存在，则New一个新实例，否则返回已有实例

    if (instance == null)

    {

        instance = new Singleton();

    }

    return instance;

}
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SingletonDemo
{
    public class Singleton

    {

        private static Singleton instance;



        /// <summary>

        /// 程序运行时，创建一个静态只读的进程辅助对象

        /// </summary>

        private static readonly object _object = new object();



        /// <summary>

        /// 构造方法私有，外键不能通过New类实例化此类

        /// </summary>

        private Singleton()

        {

        }

        /// <summary>

        /// 此方法是本类实例的唯一全局访问点

        /// （双重加锁 Double-Check Locking）

        /// </summary>

        /// <returns></returns>

        public static Singleton GetInstance()

        {

            //先判断实例是否存在，不存在再加锁处理

            if (instance == null)

            {

                //在同一时刻加了锁的那部分程序只有一个线程可以进入，

                lock (_object)

                {

                    //如实例不存在，则New一个新实例，否则返回已有实例

                    if (instance == null)

                    {

                        instance = new Singleton();

                    }

                }

            }

            return instance;

        }

        /*
         单例模式也可以不用枷锁，例如在一个类中使用三层技术调用类：

          #region  获取自身的单例模式：UserInfo 代表了Bll层中的类文件

                private static UserInfo instance;

                public static UserInfo GetInstance()

                {

                    if (instance==null)

                    {

                        instance = new UserInfo ();

                    }

                    return instance;

                }

                #endregion

        在表示层调用：

        .BLL . UserInfo   bll = BLL. UserInfo.GetInstance();
         */

    }
}
/*
 * 使用Lazy实现单例模式
 public sealed class Singleton
 {
     private static readonly Lazy<Singleton> lazy =
         new Lazy<Singleton>(() => new Singleton());
      
     public static Singleton Instance { get { return lazy.Value; } }
 
     private Singleton()
     {
     }
 } 
     */
