namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateComplainmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Complains", "OrderId", c => c.Int(nullable: false));
            AddColumn("dbo.Complains", "Title", c => c.String());
            AddColumn("dbo.Complains", "AttachFileUrl", c => c.String());
            CreateIndex("dbo.Complains", "OrderId");
            AddForeignKey("dbo.Complains", "OrderId", "dbo.OrderHeaders", "Id", cascadeDelete: true);
            DropColumn("dbo.Complains", "Email");
            DropColumn("dbo.Complains", "Phone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Complains", "Phone", c => c.String());
            AddColumn("dbo.Complains", "Email", c => c.String());
            DropForeignKey("dbo.Complains", "OrderId", "dbo.OrderHeaders");
            DropIndex("dbo.Complains", new[] { "OrderId" });
            DropColumn("dbo.Complains", "AttachFileUrl");
            DropColumn("dbo.Complains", "Title");
            DropColumn("dbo.Complains", "OrderId");
        }
    }
}
