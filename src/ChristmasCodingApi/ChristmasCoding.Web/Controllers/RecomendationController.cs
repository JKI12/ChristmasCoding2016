namespace ChristmasCoding.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Data;
    using Data.Dto;
    using Data.Models;
    using Newtonsoft.Json.Linq;

    public class RecomendationController : ApiController
    {
        private readonly MovieService ms = new MovieService();
        private readonly FacebookUserService fus = new FacebookUserService();
        private readonly TMDBService tmdb = new TMDBService();

        private Random rng = new Random();

        public async Task<IHttpActionResult> GetRecomendation(string id)
        {
            if (id == string.Empty)
            {
                return BadRequest("Provide an id");
            }
            
            var movies = new List<MovieDto>();
            var skyMovies = await ms.GetMovies();
            
            var wMovies = await fus.GetUsersMovies(id);
            var recData = new RecomendationModel();

            foreach (var m in wMovies)
            {
                var data = await tmdb.GetMovieData(m);

                if (data == null)
                {
                    continue;
                }

                foreach (var actor in data.Actors)
                {
                    var value = -1;
                    var gotValue = recData.ActorData.TryGetValue(actor.ToLower(), out value);

                    if (gotValue)
                    {
                        recData.ActorData[actor.ToLower()]++;
                    }
                    else
                    {
                        recData.ActorData.Add(actor.ToLower(), 1);
                    }
                }

                foreach (var genre in data.Genres)
                {
                    var value = -1;
                    var gotValue = recData.GenreData.TryGetValue(genre.ToLower(), out value);

                    if (gotValue)
                    {
                        recData.GenreData[genre.ToLower()]++;
                    }
                    else
                    {
                        recData.GenreData.Add(genre.ToLower(), 1);
                    }
                }
            }

            recData.ActorData = recData.ActorData.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, y => y.Value);
            recData.GenreData = recData.GenreData.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, y => y.Value);

            var temp = new List<MovieDto>();

            // Top 3 Genres
            for (var i = 0; i < (recData.GenreData.Count >= 3 ? 3 : recData.GenreData.Count); i++)
            {
                var genre = recData.GenreData.Keys.ElementAt(i);

                temp = skyMovies.Where(x => x.Genres.Contains(genre, StringComparer.InvariantCultureIgnoreCase)).ToList();
            }

            // Top 10 actors
            for (var i = 0; i < 10; i++)
            {
                var actor = recData.ActorData.Keys.ElementAt(i);

                var m = temp.Where(x => x.Actors.Contains(actor, StringComparer.InvariantCultureIgnoreCase));

                if (m.Any())
                {
                    movies = movies.Concat(m).ToList();
                }
            }

            if (movies.Count == 0)
            {
                movies = temp;
            }

            return Ok(JObject.FromObject(movies[rng.Next(movies.Count)]));
        }
    }
}
