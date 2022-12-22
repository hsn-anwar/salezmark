namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatemeasurementUnits : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderHeaders", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Store_Measurement_Units", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Store_Measurement_Units", "IsDeleted");
            DropColumn("dbo.OrderHeaders", "Type");
        }
    }
}
