using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UtvecklartestAgioMVC.DataAccessLayer
{
    public class EmployeeContext : DbContext
    {
        public DbSet<Models.Employee> Employee { get; set; }

        public EmployeeContext() : base("DefaultConnection")
        {

        }
    }
}