namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateempty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Store_Measurement_Units", "CreatedByUserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Store_Measurement_Units", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Store_Products", "Name", c => c.String(nullable: false));
            CreateIndex("dbo.Store_Measurement_Units", "CreatedByUserId");
            AddForeignKey("dbo.Store_Measurement_Units", "CreatedByUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Store_Measurement_Units", "CreatedByUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Store_Measurement_Units", new[] { "CreatedByUserId" });
            AlterColumn("dbo.Store_Products", "Name", c => c.String());
            AlterColumn("dbo.Store_Measurement_Units", "Name", c => c.String());
            DropColumn("dbo.Store_Measurement_Units", "CreatedByUserId");
        }
    }
}
