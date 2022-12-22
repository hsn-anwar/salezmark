namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checkupdatev10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Complains",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Email = c.String(),
                        Phone = c.String(),
                        Complain = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.FavouriteMerchants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MerchantId = c.String(maxLength: 128),
                        UserID = c.String(maxLength: 128),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.MerchantId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.MerchantId)
                .Index(t => t.UserID);
            
            AddColumn("dbo.Store_Products", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FavouriteMerchants", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.FavouriteMerchants", "MerchantId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Complains", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.FavouriteMerchants", new[] { "UserID" });
            DropIndex("dbo.FavouriteMerchants", new[] { "MerchantId" });
            DropIndex("dbo.Complains", new[] { "UserId" });
            DropColumn("dbo.Store_Products", "Code");
            DropTable("dbo.FavouriteMerchants");
            DropTable("dbo.Complains");
        }
    }
}
