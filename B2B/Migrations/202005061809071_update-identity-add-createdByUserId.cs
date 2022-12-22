namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateidentityaddcreatedByUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CreatedByUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "CreatedByUserId");
            AddForeignKey("dbo.AspNetUsers", "CreatedByUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "CreatedByUserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "CreatedByUserId" });
            DropColumn("dbo.AspNetUsers", "CreatedByUserId");
        }
    }
}
