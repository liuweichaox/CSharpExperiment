using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace XmlDemo
{
    //XmlRoot属性允许您设置备用名称
    //XML元素及其名称空间的（PurchaseOrder）。 通过
    //默认情况下，XmlSerializer使用类名。 属性
    //还允许您设置元素的XML名称空间。 最后，
    //该属性设置IsNullable属性，该属性指定是否
    //如果将类实例设置为xsi：null属性，则会出现
    //空引用。
    [XmlRoot("PurchaseOrder", Namespace = "http://www.cpandl.com", IsNullable = false)]
    public class PurchaseOrder
    {
        public Address ShipTo;
        public string OrderDate;
        // XmlArray属性更改XML元素名称
        //从默认的“ OrderedItems”到“ Items”。
        [XmlArray("Items")]
        public OrderedItem[] OrderedItems;
        public decimal SubTotal;
        public decimal ShipCost;
        public decimal TotalCost;
    }

    public class Address
    {
        // XmlAttribute属性指示XmlSerializer序列化
        //名称字段作为XML属性而不是XML元素（XML元素是
        //默认行为）。
        [XmlAttribute]
        public string Name;
        public string Line1;

        //将IsNullable属性设置为false会指示
        // XmlSerializer，如果
        // City字段设置为空引用。
        [XmlElement(IsNullable = false)]
        public string City;
        public string State;
        public string Zip;
    }

    public class OrderedItem
    {
        public string ItemName;
        public string Description;
        public decimal UnitPrice;
        public int Quantity;
        public decimal LineTotal;

        // Calculate是一种自定义方法，用于计算每件商品的价格
        //并将值存储在字段中。
        public void Calculate()
        {
            LineTotal = UnitPrice * Quantity;
        }
    }
}
