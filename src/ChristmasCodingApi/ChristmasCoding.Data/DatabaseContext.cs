namespace ChristmasCoding.Data
{
    using System.Data.Entity;
    using Models;

    public class DatabaseContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<FacebookUser> FacebookUsers { get; set; }
    }
}
