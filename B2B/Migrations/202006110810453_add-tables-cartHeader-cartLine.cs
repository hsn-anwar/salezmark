namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtablescartHeadercartLine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartHeaders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        CreatedForUserId = c.String(maxLength: 128),
                        CreatedByUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedByUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedForUserId)
                .Index(t => t.CreatedForUserId)
                .Index(t => t.CreatedByUserId);
            
            CreateTable(
                "dbo.CartLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(precision: 18, scale: 2),
                        LineTotal = c.Decimal(precision: 18, scale: 2),
                        ProductId = c.Int(nullable: false),
                        HeaderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CartHeaders", t => t.HeaderId, cascadeDelete: true)
                .ForeignKey("dbo.Store_Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.HeaderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartLines", "ProductId", "dbo.Store_Products");
            DropForeignKey("dbo.CartLines", "HeaderId", "dbo.CartHeaders");
            DropForeignKey("dbo.CartHeaders", "CreatedForUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CartHeaders", "CreatedByUserId", "dbo.AspNetUsers");
            DropIndex("dbo.CartLines", new[] { "HeaderId" });
            DropIndex("dbo.CartLines", new[] { "ProductId" });
            DropIndex("dbo.CartHeaders", new[] { "CreatedByUserId" });
            DropIndex("dbo.CartHeaders", new[] { "CreatedForUserId" });
            DropTable("dbo.CartLines");
            DropTable("dbo.CartHeaders");
        }
    }
}
