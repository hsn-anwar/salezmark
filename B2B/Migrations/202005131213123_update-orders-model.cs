namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateordersmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderHeaders", "OrderAssignedToUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.OrderHeaders", "OrderAssignedToUserId");
            AddForeignKey("dbo.OrderHeaders", "OrderAssignedToUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderHeaders", "OrderAssignedToUserId", "dbo.AspNetUsers");
            DropIndex("dbo.OrderHeaders", new[] { "OrderAssignedToUserId" });
            DropColumn("dbo.OrderHeaders", "OrderAssignedToUserId");
        }
    }
}
