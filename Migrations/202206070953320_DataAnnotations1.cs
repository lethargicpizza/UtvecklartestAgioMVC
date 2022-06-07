namespace UtvecklartestAgioMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAnnotations1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Förnamn", c => c.String(nullable: false, maxLength: 1));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "Förnamn", c => c.String(nullable: false));
        }
    }
}
