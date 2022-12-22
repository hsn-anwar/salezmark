namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateproductsaddlowStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Store_Products", "LowStock", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Store_Products", "LowStock");
        }
    }
}
