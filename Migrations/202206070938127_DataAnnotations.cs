namespace UtvecklartestAgioMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Förnamn", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "Efternamn", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "Personnummer", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "Anställningsnummer", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "Anställningsnummer", c => c.String());
            AlterColumn("dbo.Employees", "Personnummer", c => c.String());
            AlterColumn("dbo.Employees", "Efternamn", c => c.String());
            AlterColumn("dbo.Employees", "Förnamn", c => c.String());
        }
    }
}
