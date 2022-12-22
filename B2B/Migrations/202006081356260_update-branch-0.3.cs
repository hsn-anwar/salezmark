namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatebranch03 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Branches", "PhoneNumber", c => c.String());
            AddColumn("dbo.Branches", "ImageUrl", c => c.String());
            AddColumn("dbo.Branches", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Branches", "AssignedToUserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Locations", "Latitude", c => c.Double());
            AlterColumn("dbo.Locations", "Longitude", c => c.Double());
            CreateIndex("dbo.Branches", "AssignedToUserId");
            AddForeignKey("dbo.Branches", "AssignedToUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Branches", "AssignedToUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Branches", new[] { "AssignedToUserId" });
            AlterColumn("dbo.Locations", "Longitude", c => c.String());
            AlterColumn("dbo.Locations", "Latitude", c => c.String());
            DropColumn("dbo.Branches", "AssignedToUserId");
            DropColumn("dbo.Branches", "IsDeleted");
            DropColumn("dbo.Branches", "ImageUrl");
            DropColumn("dbo.Branches", "PhoneNumber");
        }
    }
}
