namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitStore : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Store_Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ImageUrl = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Store_Measurement_Units",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Store_Product_Gallery",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageUrl = c.String(),
                        StoreProductId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Store_Products", t => t.StoreProductId, cascadeDelete: true)
                .Index(t => t.StoreProductId);
            
            CreateTable(
                "dbo.Store_Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalePrice = c.Decimal(precision: 18, scale: 2),
                        Description = c.String(),
                        Specification = c.String(),
                        StoreMeasurementUnitId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Store_Measurement_Units", t => t.StoreMeasurementUnitId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.StoreMeasurementUnitId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Store_Product_Gallery", "StoreProductId", "dbo.Store_Products");
            DropForeignKey("dbo.Store_Products", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Store_Products", "StoreMeasurementUnitId", "dbo.Store_Measurement_Units");
            DropIndex("dbo.Store_Products", new[] { "UserId" });
            DropIndex("dbo.Store_Products", new[] { "StoreMeasurementUnitId" });
            DropIndex("dbo.Store_Product_Gallery", new[] { "StoreProductId" });
            DropTable("dbo.Store_Products");
            DropTable("dbo.Store_Product_Gallery");
            DropTable("dbo.Store_Measurement_Units");
            DropTable("dbo.Store_Category");
        }
    }
}
