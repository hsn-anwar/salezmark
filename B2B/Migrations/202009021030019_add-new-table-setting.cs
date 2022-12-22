namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnewtablesetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AboutUs = c.String(),
                        TermsandCondition = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Settings");
        }
    }
}
