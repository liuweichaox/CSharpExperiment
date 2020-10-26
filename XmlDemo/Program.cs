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
            // Read and write purchase orders.
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
            OrderedItem[] items = { i1 };
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
            // Creates an instance of the XmlSerializer class;
            // specifies the type of object to be deserialized.
            XmlSerializer serializer = new XmlSerializer(typeof(PurchaseOrder));
            // If the XML document has been altered with unknown
            // nodes or attributes, handles them with the
            // UnknownNode and UnknownAttribute events.
            serializer.UnknownNode += new
            XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new
            XmlAttributeEventHandler(serializer_UnknownAttribute);

            // A FileStream is needed to read the XML document.
            FileStream fs = new FileStream(filename, FileMode.Open);
            // Declares an object variable of the type to be deserialized.
            PurchaseOrder po;
            // Uses the Deserialize method to restore the object's state
            // with data from the XML document. */
            po = (PurchaseOrder)serializer.Deserialize(fs);
            // Reads the order date.
            Console.WriteLine("OrderDate: " + po.OrderDate);

            // Reads the shipping address.
            Address shipTo = po.ShipTo;
            ReadAddress(shipTo, "Ship To:");
            // Reads the list of ordered items.
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
            // Reads the subtotal, shipping cost, and total cost.
            Console.WriteLine(
            "\n\t\t\t\t\t Subtotal\t" + po.SubTotal +
            "\n\t\t\t\t\t Shipping\t" + po.ShipCost +
            "\n\t\t\t\t\t Total\t\t" + po.TotalCost
            );
        }

        protected void ReadAddress(Address a, string label)
        {
            // Reads the fields of the Address.
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
            // Create the XmlSerializer.
            XmlSerializer s = new XmlSerializer(typeof(Group));

            // To write the file, a TextWriter is required.
            TextWriter writer = new StreamWriter(filename);

            /* Create an instance of the group to serialize, and set
               its properties. */
            Group group = new Group();
            group.GroupID = 10.089f;
            group.IsActive = false;

            group.HexBytes = new byte[1] { Convert.ToByte(100) };

            Employee x = new Employee();
            Employee y = new Employee();

            x.Name = "Jack";
            y.Name = "Jill";

            group.Employees = new Employee[2] { x, y };

            Manager mgr = new Manager();
            mgr.Name = "Sara";
            mgr.Level = 4;
            group.Manager = mgr;

            /* Add a number and a string to the
            ArrayList returned by the ExtraInfo property. */
            group.ExtraInfo = new ArrayList();
            group.ExtraInfo.Add(42);
            group.ExtraInfo.Add("Answer");

            // Serialize the object, and close the TextWriter.
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
            foreach (Employee e in g.Employees)
            {
                Console.WriteLine(e.Name);
            }
        }
    }
    
}
