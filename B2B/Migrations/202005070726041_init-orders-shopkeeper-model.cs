namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initordersshopkeepermodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        UserId = c.String(maxLength: 128),
                        LocationId = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.OrderHeaders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNumber = c.String(),
                        Email = c.String(),
                        RejectionComment = c.String(),
                        CanceledComment = c.String(),
                        DeliveryNote = c.String(),
                        PhoneNumber = c.String(),
                        PaymentStatus = c.Int(nullable: false),
                        DeliveryOption = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ShippingCharges = c.Decimal(precision: 18, scale: 2),
                        OrderByUserId = c.String(maxLength: 128),
                        OrderForUserId = c.String(maxLength: 128),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.OrderByUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.OrderForUserId)
                .Index(t => t.OrderByUserId)
                .Index(t => t.OrderForUserId);
            
            CreateTable(
                "dbo.OrderLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderHeaderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LineTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderHeaders", t => t.OrderHeaderId, cascadeDelete: true)
                .ForeignKey("dbo.Store_Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderHeaderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.PurchaseOrderHeaders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        OrderNumber = c.String(),
                        CreatedByUserId = c.String(maxLength: 128),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedByUserId)
                .Index(t => t.CreatedByUserId);
            
            CreateTable(
                "dbo.PurchaseOrderLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        PurchaseOrderHeaderId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Store_Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.PurchaseOrderHeaders", t => t.PurchaseOrderHeaderId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.PurchaseOrderHeaderId);
            
            CreateTable(
                "dbo.Store_Qty_Movement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InQty = c.Decimal(precision: 18, scale: 2),
                        OutQty = c.Decimal(precision: 18, scale: 2),
                        PurchaseOrderHeaderId = c.Int(),
                        OrderHeaderId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderHeaders", t => t.OrderHeaderId)
                .ForeignKey("dbo.Store_Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.PurchaseOrderHeaders", t => t.PurchaseOrderHeaderId)
                .Index(t => t.PurchaseOrderHeaderId)
                .Index(t => t.OrderHeaderId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Store_Qty_Movement", "PurchaseOrderHeaderId", "dbo.PurchaseOrderHeaders");
            DropForeignKey("dbo.Store_Qty_Movement", "ProductId", "dbo.Store_Products");
            DropForeignKey("dbo.Store_Qty_Movement", "OrderHeaderId", "dbo.OrderHeaders");
            DropForeignKey("dbo.PurchaseOrderLines", "PurchaseOrderHeaderId", "dbo.PurchaseOrderHeaders");
            DropForeignKey("dbo.PurchaseOrderLines", "ProductId", "dbo.Store_Products");
            DropForeignKey("dbo.PurchaseOrderHeaders", "CreatedByUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrderLines", "ProductId", "dbo.Store_Products");
            DropForeignKey("dbo.OrderLines", "OrderHeaderId", "dbo.OrderHeaders");
            DropForeignKey("dbo.OrderHeaders", "OrderForUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrderHeaders", "OrderByUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Branches", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Branches", "LocationId", "dbo.Locations");
            DropIndex("dbo.Store_Qty_Movement", new[] { "ProductId" });
            DropIndex("dbo.Store_Qty_Movement", new[] { "OrderHeaderId" });
            DropIndex("dbo.Store_Qty_Movement", new[] { "PurchaseOrderHeaderId" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "PurchaseOrderHeaderId" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "ProductId" });
            DropIndex("dbo.PurchaseOrderHeaders", new[] { "CreatedByUserId" });
            DropIndex("dbo.OrderLines", new[] { "ProductId" });
            DropIndex("dbo.OrderLines", new[] { "OrderHeaderId" });
            DropIndex("dbo.OrderHeaders", new[] { "OrderForUserId" });
            DropIndex("dbo.OrderHeaders", new[] { "OrderByUserId" });
            DropIndex("dbo.Branches", new[] { "LocationId" });
            DropIndex("dbo.Branches", new[] { "UserId" });
            DropTable("dbo.Store_Qty_Movement");
            DropTable("dbo.PurchaseOrderLines");
            DropTable("dbo.PurchaseOrderHeaders");
            DropTable("dbo.OrderLines");
            DropTable("dbo.OrderHeaders");
            DropTable("dbo.Branches");
        }
    }
}
