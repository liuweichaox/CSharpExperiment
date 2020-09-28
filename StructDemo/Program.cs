/*
 在 C# 中，结构体是值类型数据结构。它使得一个单一变量可以存储各种数据类型的相关数据。struct 关键字用于创建结构体。

 C# 结构的特点
在 C# 中的结构与传统的 C 或 C++ 中的结构不同。C# 中的结构有以下特点：

结构可带有方法、字段、索引、属性、运算符方法和事件。
结构可定义构造函数，但不能定义析构函数。但是，您不能为结构定义默认的构造函数。默认的构造函数是自动定义的，且不能被改变。
与类不同，结构不能继承其他的结构或类。
结构不能作为其他结构或类的基础结构。
结构可实现一个或多个接口。
结构成员不能指定为 abstract、virtual 或 protected。
当您使用 New 操作符创建一个结构对象时，会调用适当的构造函数来创建结构。与类不同，结构可以不使用 New 操作符即可被实例化。
如果不使用 New 操作符，只有在所有的字段都被初始化之后，字段才被赋值，对象才被使用。
 */using System;

namespace MyNetDemo
{
    class Program
    {
        struct Books
        {
            private string title;
            private string author;
            private string subject;
            private int book_id;
            public void getValues(string t, string a, string s, int id)
            {
                title = t;
                author = a;
                subject = s;
                book_id = id;
            }
            public void display()
            {
                Console.WriteLine("Title : {0}", title);
                Console.WriteLine("Author : {0}", author);
                Console.WriteLine("Subject : {0}", subject);
                Console.WriteLine("Book_id :{0}", book_id);
            }

        };

        public class testStructure
        {
            public static void Main(string[] args)
            {

                Books Book1 = new Books(); /* 声明 Book1，类型为 Book */
                Books Book2 = new Books(); /* 声明 Book2，类型为 Book */

                /* book 1 详述 */
                Book1.getValues("C Programming",
                "Nuha Ali", "C Programming Tutorial", 6495407);

                /* book 2 详述 */
                Book2.getValues("Telecom Billing",
                "Zara Ali", "Telecom Billing Tutorial", 6495700);

                /* 打印 Book1 信息 */
                Book1.display();

                /* 打印 Book2 信息 */
                Book2.display();

                Console.ReadKey();

            }
        }
    }
}
