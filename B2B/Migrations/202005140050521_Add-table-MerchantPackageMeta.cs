namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddtableMerchantPackageMeta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MerchantPackageMetas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartingFrom = c.DateTime(nullable: false),
                        ValidTill = c.DateTime(nullable: false),
                        PackageId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsAmountPaid = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Packages", t => t.PackageId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.PackageId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Packages", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MerchantPackageMetas", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MerchantPackageMetas", "PackageId", "dbo.Packages");
            DropIndex("dbo.MerchantPackageMetas", new[] { "UserId" });
            DropIndex("dbo.MerchantPackageMetas", new[] { "PackageId" });
            DropColumn("dbo.Packages", "Price");
            DropTable("dbo.MerchantPackageMetas");
        }
    }
}
