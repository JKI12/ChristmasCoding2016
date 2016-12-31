namespace ChristmasCoding.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Required_Field : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FacebookUsers", "AuthToken", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FacebookUsers", "AuthToken", c => c.String());
        }
    }
}
