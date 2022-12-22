namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateidentity03 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "NotificationEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "OrderAuthenticationEnabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "OrderAuthenticationEnabled");
            DropColumn("dbo.AspNetUsers", "NotificationEnabled");
        }
    }
}
