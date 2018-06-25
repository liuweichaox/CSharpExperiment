using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elasticsearch
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
