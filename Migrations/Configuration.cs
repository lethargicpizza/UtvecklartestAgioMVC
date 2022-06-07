namespace UtvecklartestAgioMVC.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<UtvecklartestAgioMVC.DataAccessLayer.EmployeeContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(UtvecklartestAgioMVC.DataAccessLayer.EmployeeContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.

            context.Employee.AddOrUpdate(
              p => p.Förnamn,
              new Models.Employee { Förnamn = "Eva", Efternamn = "Karlsson", Personnummer = "650506-9336", Anställningsnummer = "6" },
              new Models.Employee { Förnamn = "Joakim", Efternamn = "Sundvall", Personnummer = "780228-2033", Anställningsnummer = "12" },
              new Models.Employee { Förnamn = "Isak", Efternamn = "Adolfsson", Personnummer = "861106-1758", Anställningsnummer = "7" });
        }
    }
}
