namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateproductcatalog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Store_Category", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Store_Category", "IsDeleted");
        }
    }
}
