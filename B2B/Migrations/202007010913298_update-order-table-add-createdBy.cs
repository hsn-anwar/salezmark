namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateordertableaddcreatedBy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderHeaders", "OrderCreatedByUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.OrderHeaders", "OrderCreatedByUserId");
            AddForeignKey("dbo.OrderHeaders", "OrderCreatedByUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderHeaders", "OrderCreatedByUserId", "dbo.AspNetUsers");
            DropIndex("dbo.OrderHeaders", new[] { "OrderCreatedByUserId" });
            DropColumn("dbo.OrderHeaders", "OrderCreatedByUserId");
        }
    }
}
