namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatepackagemodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Packages", "DurationType", c => c.Int(nullable: false));
            AddColumn("dbo.Packages", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Packages", "IsActive");
            DropColumn("dbo.Packages", "DurationType");
        }
    }
}
