namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnotificationtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotifyToUserId = c.String(),
                        NotifyByUserId = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        Isseen = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Notifications");
        }
    }
}
