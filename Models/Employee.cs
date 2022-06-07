using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UtvecklartestAgioMVC.Models
{
    public class Employee
    {
        [Required]
        public string Förnamn { get; set; }
        
        [Required]
        public string Efternamn { get; set; }

        [Required]
        public string Personnummer { get; set; }

        [Required]
        public string Anställningsnummer { get; set; }
        public int Id { get; set; }

    }
}