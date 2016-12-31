namespace ChristmasCoding.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Psid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FacebookUsers", "Psid", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FacebookUsers", "Psid");
        }
    }
}
