namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateproductaddcompanyid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Store_Products", "CompanyId", c => c.Int(nullable: false));
            CreateIndex("dbo.Store_Products", "CompanyId");
            AddForeignKey("dbo.Store_Products", "CompanyId", "dbo.Store_Company", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Store_Products", "CompanyId", "dbo.Store_Company");
            DropIndex("dbo.Store_Products", new[] { "CompanyId" });
            DropColumn("dbo.Store_Products", "CompanyId");
        }
    }
}
