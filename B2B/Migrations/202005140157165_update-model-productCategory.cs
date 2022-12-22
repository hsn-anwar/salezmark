namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatemodelproductCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Store_Products", "CategoryId", c => c.Int(nullable: false));
            AddColumn("dbo.Store_Category", "ParentCategoryId", c => c.Int());
            CreateIndex("dbo.Store_Products", "CategoryId");
            CreateIndex("dbo.Store_Category", "ParentCategoryId");
            AddForeignKey("dbo.Store_Category", "ParentCategoryId", "dbo.Store_Category", "Id");
            AddForeignKey("dbo.Store_Products", "CategoryId", "dbo.Store_Category", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Store_Products", "CategoryId", "dbo.Store_Category");
            DropForeignKey("dbo.Store_Category", "ParentCategoryId", "dbo.Store_Category");
            DropIndex("dbo.Store_Category", new[] { "ParentCategoryId" });
            DropIndex("dbo.Store_Products", new[] { "CategoryId" });
            DropColumn("dbo.Store_Category", "ParentCategoryId");
            DropColumn("dbo.Store_Products", "CategoryId");
        }
    }
}
