namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateunknown02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartHeaders", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.OrderHeaders", "BranchId", c => c.Int());
            CreateIndex("dbo.OrderHeaders", "BranchId");
            AddForeignKey("dbo.OrderHeaders", "BranchId", "dbo.Branches", "Id");
            DropColumn("dbo.CartLines", "Price");
            DropColumn("dbo.CartLines", "LineTotal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CartLines", "LineTotal", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CartLines", "Price", c => c.Decimal(precision: 18, scale: 2));
            DropForeignKey("dbo.OrderHeaders", "BranchId", "dbo.Branches");
            DropIndex("dbo.OrderHeaders", new[] { "BranchId" });
            DropColumn("dbo.OrderHeaders", "BranchId");
            DropColumn("dbo.CartHeaders", "CreatedOn");
        }
    }
}
