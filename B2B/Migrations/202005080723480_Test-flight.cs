namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Testflight : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Store_Products", "InStock", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Store_Qty_Movement", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Store_Qty_Movement", "Cost", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Store_Qty_Movement", "Price", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Store_Qty_Movement", "Total", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Store_Qty_Movement", "Total");
            DropColumn("dbo.Store_Qty_Movement", "Price");
            DropColumn("dbo.Store_Qty_Movement", "Cost");
            DropColumn("dbo.Store_Qty_Movement", "Type");
            DropColumn("dbo.Store_Products", "InStock");
        }
    }
}
