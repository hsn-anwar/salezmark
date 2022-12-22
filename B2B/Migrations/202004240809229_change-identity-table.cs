namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeidentitytable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "UserID", c => c.String());
            AddColumn("dbo.AspNetUsers", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Status");
            DropColumn("dbo.AspNetUsers", "UserID");
            DropColumn("dbo.AspNetUsers", "CreatedOn");
        }
    }
}
