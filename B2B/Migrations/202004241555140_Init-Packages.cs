namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitPackages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Features",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Note = c.String(),
                        Duration = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PackageFeatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FeatureId = c.Int(nullable: false),
                        PackageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Features", t => t.FeatureId, cascadeDelete: true)
                .ForeignKey("dbo.Packages", t => t.PackageId, cascadeDelete: true)
                .Index(t => t.FeatureId)
                .Index(t => t.PackageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PackageFeatures", "PackageId", "dbo.Packages");
            DropForeignKey("dbo.PackageFeatures", "FeatureId", "dbo.Features");
            DropIndex("dbo.PackageFeatures", new[] { "PackageId" });
            DropIndex("dbo.PackageFeatures", new[] { "FeatureId" });
            DropTable("dbo.PackageFeatures");
            DropTable("dbo.Packages");
            DropTable("dbo.Features");
        }
    }
}
