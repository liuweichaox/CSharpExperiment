using System;

namespace OperatorDemo
{
    class Program
    {
        class Box
        {
            private double length;      // 长度
            private double breadth;     // 宽度
            private double height;      // 高度

            public double getVolume()
            {
                return length * breadth * height;
            }
            public void setLength(double len)
            {
                length = len;
            }

            public void setBreadth(double bre)
            {
                breadth = bre;
            }

            public void setHeight(double hei)
            {
                height = hei;
            }
            // 重载 + 运算符来把两个 Box 对象相加
            public static Box operator +(Box b, Box c)
            {
                Box box = new Box();
                box.length = b.length + c.length;
                box.breadth = b.breadth + c.breadth;
                box.height = b.height + c.height;
                return box;
            }

        }

        class Tester
        {
            static void Main(string[] args)
            {
                Box Box1 = new Box();         // 声明 Box1，类型为 Box
                Box Box2 = new Box();         // 声明 Box2，类型为 Box
                Box Box3 = new Box();         // 声明 Box3，类型为 Box
                double volume = 0.0;          // 体积

                // Box1 详述
                Box1.setLength(6.0);
                Box1.setBreadth(7.0);
                Box1.setHeight(5.0);

                // Box2 详述
                Box2.setLength(12.0);
                Box2.setBreadth(13.0);
                Box2.setHeight(10.0);

                // Box1 的体积
                volume = Box1.getVolume();
                Console.WriteLine("Box1 的体积： {0}", volume);

                // Box2 的体积
                volume = Box2.getVolume();
                Console.WriteLine("Box2 的体积： {0}", volume);

                // 把两个对象相加
                Box3 = Box1 + Box2;

                // Box3 的体积
                volume = Box3.getVolume();
                Console.WriteLine("Box3 的体积： {0}", volume);
                Console.ReadKey();
            }
        }

    }
}
