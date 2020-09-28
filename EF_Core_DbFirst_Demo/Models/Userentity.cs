using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF_Core_DbFirst_Demo.Models
{
    public partial class Userentity
    {
        public int Id { get; set; }            
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int? Age { get; set; }
        public string Qq { get; set; }
        public string Phone { get; set; }
    }
}
