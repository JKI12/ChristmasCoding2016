namespace ChristmasCoding.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inital : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uuid = c.String(),
                        Title = c.String(),
                        Rating = c.String(),
                        Duration = c.Int(nullable: false),
                        Actors = c.String(),
                        Genres = c.String(),
                        Synopsis = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Movies");
        }
    }
}
