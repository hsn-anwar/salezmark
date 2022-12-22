namespace B2B.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetablebranchsaddvalidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Store_Company", "Name", c => c.String(nullable: false, maxLength: 25));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Store_Company", "Name", c => c.String());
        }
    }
}
