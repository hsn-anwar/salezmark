namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePurchaseOrderModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrderHeaders", "SupplierName", c => c.String());
            AddColumn("dbo.PurchaseOrderHeaders", "PhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseOrderHeaders", "PhoneNumber");
            DropColumn("dbo.PurchaseOrderHeaders", "SupplierName");
        }
    }
}
