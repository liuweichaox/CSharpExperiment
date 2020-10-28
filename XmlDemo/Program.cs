using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

namespace XmlDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //读写采购订单。
            Program t = new Program();
            t.CreatePO("po.xml");
            t.ReadPO("po.xml");

            t.SerializeObject("FirstDoc.xml");
            t.DeserializeObject("FirstDoc.xml");
            Console.ReadKey();
        }

        private void CreatePO(string filename)
        {
            //创建XmlSerializer类的实例；
            //指定要序列化的对象的类型。
            XmlSerializer serializer = new XmlSerializer(typeof(PurchaseOrder));
            TextWriter writer = new StreamWriter(filename);
            PurchaseOrder po = new PurchaseOrder();

            //创建要寄送并开票的地址。
            Address billAddress = new Address();
            billAddress.Name = "Teresa Atkinson";
            billAddress.Line1 = "1 Main St.";
            billAddress.City = "AnyTown";
            billAddress.State = "WA";
            billAddress.Zip = "00000";
            //将“发件人”和“发单人”设置为同一收件人。
            po.ShipTo = billAddress;
            po.OrderDate = System.DateTime.Now.ToLongDateString();

            //创建一个OrderedItem。
            OrderedItem i1 = new OrderedItem();
            i1.ItemName = "Widget S";
            i1.Description = "Small widget";
            i1.UnitPrice = (decimal)5.23;
            i1.Quantity = 3;
            i1.Calculate();

            //将项目插入数组。
            OrderedItem[] items = { i1, i1 };
            po.OrderedItems = items;
            //计算总费用。
            decimal subTotal = new decimal();
            foreach (OrderedItem oi in items)
            {
                subTotal += oi.LineTotal;
            }
            po.SubTotal = subTotal;
            po.ShipCost = (decimal)12.51;
            po.TotalCost = po.SubTotal + po.ShipCost;
            //序列化采购订单，并关闭TextWriter。
            serializer.Serialize(writer, po);
            writer.Close();
        }

        protected void ReadPO(string filename)
        {
            //创建XmlSerializer类的实例；
            //指定要反序列化的对象的类型。
            XmlSerializer serializer = new XmlSerializer(typeof(PurchaseOrder));
            //如果XML文档已被更改为未知
            //节点或属性，使用
            // UnknownNode和UnknownAttribute事件。
            serializer.UnknownNode += new
            XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new
            XmlAttributeEventHandler(serializer_UnknownAttribute);

            //需要FileStream来读取XML文档。
            FileStream fs = new FileStream(filename, FileMode.Open);
            //声明要反序列化类型的对象变量。
            PurchaseOrder po;
            //使用反序列化方法还原对象的状态
            //来自XML文档的数据。 * /
            po = (PurchaseOrder)serializer.Deserialize(fs);
            //读取订单日期。
            Console.WriteLine("OrderDate: " + po.OrderDate);

            //读取送货地址。
            Address shipTo = po.ShipTo;
            ReadAddress(shipTo, "Ship To:");
            //读取订购商品列表。
            OrderedItem[] items = po.OrderedItems;
            Console.WriteLine("Items to be shipped:");
            foreach (OrderedItem oi in items)
            {
                Console.WriteLine("\t" +
                oi.ItemName + "\t" +
                oi.Description + "\t" +
                oi.UnitPrice + "\t" +
                oi.Quantity + "\t" +
                oi.LineTotal);
            }
            //读取小计，运输成本和总成本。
            Console.WriteLine(
            "\n\t\t\t\t\t Subtotal\t" + po.SubTotal +
            "\n\t\t\t\t\t Shipping\t" + po.ShipCost +
            "\n\t\t\t\t\t Total\t\t" + po.TotalCost
            );
        }

        protected void ReadAddress(Address a, string label)
        {
            //读取地址的字段。
            Console.WriteLine(label);
            Console.Write("\t" +
            a.Name + "\n\t" +
            a.Line1 + "\n\t" +
            a.City + "\t" +
            a.State + "\n\t" +
            a.Zip + "\n");
        }

        protected void serializer_UnknownNode
        (object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        protected void serializer_UnknownAttribute
        (object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }

        public void SerializeObject(string filename)
        {
            //创建XmlSerializer。
            XmlSerializer s = new XmlSerializer(typeof(Group));

            //要写入文件，需要一个TextWriter。
            TextWriter writer = new StreamWriter(filename);

            /*创建要序列化的组的实例，并进行设置
                其属性。 */
            Group group = new Group();
            group.GroupID = 10.089f;
            group.IsActive = false;

            group.HexBytes = new byte[2] { Convert.ToByte(100), Convert.ToByte(200) };

            Employee x = new Employee();
            Employee y = new Employee();

            x.Name = "Jack";
            y.Name = "Jill";

            group.Employees = new Employee[2] { x, y };

            Manager mgr = new Manager();
            mgr.Name = "Sara";
            mgr.Level = 4;
            group.Manager = mgr;

            /* 将数字和字符串添加到
             ArrayList由ExtraInfo属性返回。 */
            group.ExtraInfo = new ArrayList();
            group.ExtraInfo.Add(42);
            group.ExtraInfo.Add("Answer");

            //序列化对象，然后关闭TextWriter。
            s.Serialize(writer, group);
            writer.Close();
        }

        public void DeserializeObject(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);
            XmlSerializer x = new XmlSerializer(typeof(Group));
            Group g = (Group)x.Deserialize(fs);
            Console.WriteLine(g.Manager.Name);
            Console.WriteLine(g.GroupID);
            Console.WriteLine(g.HexBytes[0]);
            Console.WriteLine(g.HexBytes[1]);
            foreach (Employee e in g.Employees)
            {
                Console.WriteLine(e.Name);
            }
        }
    }

}
