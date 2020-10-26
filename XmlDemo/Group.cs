using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace XmlDemo
{

    public class Group
    {
        /* Set the element name and namespace of the XML element.
        By applying an XmlElementAttribute to an array,  you instruct
        the XmlSerializer to serialize the array as a series of XML
        elements, instead of a nested set of elements. */

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
