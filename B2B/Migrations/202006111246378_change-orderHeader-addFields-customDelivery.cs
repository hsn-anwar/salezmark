namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeorderHeaderaddFieldscustomDelivery : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderHeaders", "WithCustomDelivery", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderHeaders", "CustomDeliveryTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderHeaders", "CustomDeliveryTime");
            DropColumn("dbo.OrderHeaders", "WithCustomDelivery");
        }
    }
}
