namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrdersaddFeildIsPaid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderHeaders", "IsPaid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderHeaders", "IsPaid");
        }
    }
}
