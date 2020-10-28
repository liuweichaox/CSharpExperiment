using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace XmlDemo
{

    public class Group
    {
        /* 设置XML元素的元素名称和名称空间。
         通过将XmlElementAttribute应用于数组，您可以指示
         XmlSerializer将数组序列化为一系列XML
         元素，而不是嵌套的元素集。 */

        [XmlElement(ElementName = "Members", Namespace = "http://www.cpandl.com")]
        public Employee[] Employees;

        [XmlElement(DataType = "double", ElementName = "Building")]
        public double GroupID;

        [XmlElement(DataType = "hexBinary")]
        public byte[] HexBytes;

        [XmlElement(DataType = "boolean")]
        public bool IsActive;

        [XmlElement(Type = typeof(Manager))]
        public Employee Manager;

        [XmlElement(typeof(int), ElementName = "ObjectNumber"),
        XmlElement(typeof(string), ElementName = "ObjectString")]
        public ArrayList ExtraInfo;
    }

    public class Employee
    {
        public string Name;
    }

    public class Manager : Employee
    {
        public int Level;
    }
}
