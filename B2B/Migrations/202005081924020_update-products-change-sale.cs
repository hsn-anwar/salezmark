namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateproductschangesale : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Store_Products", "SalePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Store_Products", "SalePrice", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
