using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FaciTWebApi.Models
{
    public class FaciTUser
    {
        public int Id { get; set; }
        [Required, StringLength(15)]
        public string UserName { get; set; }
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Email format is not valid")]
        public string Email { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone Format is not valid")]
        public string Phone { get; set; }
        public string Region { get; set; }
        public string UserGroup { get; set; }
        public string ImagePath { get; set; }
        public int Date { get; set; }
    

        [NotMapped]
        public byte[] ImageArray { get; set; }


    }
}