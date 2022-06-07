using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UtvecklartestAgioMVC.Models
{
    public class Employee
    {
        [Required(ErrorMessage = "Ett förnamn behövs")]
        public string Förnamn { get; set; }

        [Required(ErrorMessage = "Ett efternamn behövs")]
        public string Efternamn { get; set; }

        [Required(ErrorMessage = "Ett personnummer behövs")]
        public string Personnummer { get; set; }

        [Required(ErrorMessage = "Ett anställningsnummer behövs")]
        public string Anställningsnummer { get; set; }
        public int Id { get; set; }

    }
}