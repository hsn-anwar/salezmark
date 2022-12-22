namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateidentity02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AccountSuspend", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsEmailVerified", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "DeviceNumber", c => c.String());
            AddColumn("dbo.AspNetUsers", "DeviceToken", c => c.String());
            AddColumn("dbo.AspNetUsers", "DeviceFCM", c => c.String());
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "DeviceFCM");
            DropColumn("dbo.AspNetUsers", "DeviceToken");
            DropColumn("dbo.AspNetUsers", "DeviceNumber");
            DropColumn("dbo.AspNetUsers", "IsEmailVerified");
            DropColumn("dbo.AspNetUsers", "AccountSuspend");
        }
    }
}
