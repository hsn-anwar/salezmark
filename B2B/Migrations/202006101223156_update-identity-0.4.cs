namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateidentity04 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "AssignBranchId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "AssignBranchId");
            AddForeignKey("dbo.AspNetUsers", "AssignBranchId", "dbo.Branches", "Id");
            DropColumn("dbo.AspNetUsers", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            DropForeignKey("dbo.AspNetUsers", "AssignBranchId", "dbo.Branches");
            DropIndex("dbo.AspNetUsers", new[] { "AssignBranchId" });
            DropColumn("dbo.AspNetUsers", "AssignBranchId");
            DropColumn("dbo.AspNetUsers", "IsDeleted");
        }
    }
}
