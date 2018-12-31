using System;
using System.Collections.Generic;
using System.Text;

namespace FaciTApp.Models
{
    public class FaciTUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Region { get; set; }
        public string UserGroup { get; set; }
        public string ImagePath { get; set; }
        public int Date { get; set; }
        public object ImageArray { get; set; }
    }
}
