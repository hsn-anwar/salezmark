namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateproductsmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Store_Products", "Cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Store_Products", "ActualPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Store_Products", "Tax", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Store_Products", "Discount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Store_Products", "FeatureImageUrl", c => c.String());
            DropColumn("dbo.Store_Products", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Store_Products", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Store_Products", "FeatureImageUrl");
            DropColumn("dbo.Store_Products", "Discount");
            DropColumn("dbo.Store_Products", "Tax");
            DropColumn("dbo.Store_Products", "ActualPrice");
            DropColumn("dbo.Store_Products", "Cost");
        }
    }
}
