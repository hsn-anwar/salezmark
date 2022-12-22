namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtablebranchs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Store_Company",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ImageUrl = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Store_Company");
        }
    }
}
