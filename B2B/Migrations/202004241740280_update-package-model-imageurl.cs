namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatepackagemodelimageurl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Packages", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Packages", "ImageUrl");
        }
    }
}
