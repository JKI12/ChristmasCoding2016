namespace ChristmasCoding.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adding_Facebook_Users_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FacebookUsers",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        AuthToken = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FacebookUsers");
        }
    }
}
