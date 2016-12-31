namespace ChristmasCoding.Data
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Dto;
    using Models;

    public class MovieService
    {
        private readonly DatabaseContext db = new DatabaseContext();

        public async Task AddMovie(Movie movie)
        {
            db.Movies.Add(movie);
            await db.SaveChangesAsync();
        }

        public async Task<List<MovieDto>> GetMovies()
        {
            var dbMovies = await db.Movies.ToListAsync();

            var movies = new List<MovieDto>();

            foreach (var m in dbMovies)
            {
                movies.Add(new MovieDto()
                {
                    Uuid = m.Uuid,
                    Duration = m.Duration,
                    Rating = m.Rating,
                    Synopsis = m.Synopsis,
                    Title = m.Title,
                    Actors = m.Actors.Split(',').ToList(),
                    Genres = m.Genres.Split(',').ToList()
                });
            }

            return movies;
        }
    }
}
