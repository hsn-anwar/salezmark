namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class unknownchanges2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Store_Products", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Store_Products", "Status");
        }
    }
}
