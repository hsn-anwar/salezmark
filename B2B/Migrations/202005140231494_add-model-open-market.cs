namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmodelopenmarket : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MerchantConnections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MerchantId = c.String(maxLength: 128),
                        ShopkeeperId = c.String(maxLength: 128),
                        Status = c.Int(nullable: false),
                        SubscribeAt = c.DateTime(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.MerchantId)
                .ForeignKey("dbo.AspNetUsers", t => t.ShopkeeperId)
                .Index(t => t.MerchantId)
                .Index(t => t.ShopkeeperId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MerchantConnections", "ShopkeeperId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MerchantConnections", "MerchantId", "dbo.AspNetUsers");
            DropIndex("dbo.MerchantConnections", new[] { "ShopkeeperId" });
            DropIndex("dbo.MerchantConnections", new[] { "MerchantId" });
            DropTable("dbo.MerchantConnections");
        }
    }
}
