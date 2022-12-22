namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class revertidentity04change : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "AssignBranchId", "dbo.Branches");
            DropIndex("dbo.AspNetUsers", new[] { "AssignBranchId" });
            DropColumn("dbo.AspNetUsers", "AssignBranchId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "AssignBranchId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "AssignBranchId");
            AddForeignKey("dbo.AspNetUsers", "AssignBranchId", "dbo.Branches", "Id");
        }
    }
}
