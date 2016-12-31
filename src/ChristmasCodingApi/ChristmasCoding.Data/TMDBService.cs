namespace ChristmasCoding.Data
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dto;
    using Flurl;
    using Flurl.Http;
    using Newtonsoft.Json.Linq;

    public class TMDBService
    {
        private readonly string API_KEY = ConfigurationManager.AppSettings["tmdb_key"];

        public async Task<MovieDto> GetMovieData(string movieTitle)
        {
            var search =
                await $"https://api.themoviedb.org/3/search/movie?api_key={API_KEY}&language=en-US&page=1&include_adult=false"
                .SetQueryParam("query", Uri.EscapeUriString(movieTitle)).GetJsonAsync<JObject>();

            if (search["total_results"].ToString().Equals("0"))
            {
                return null;
            }

            var movieId = search["results"].ToArray().First()["id"];

            var genres = new List<string>();
            var movieData =
                await $"https://api.themoviedb.org/3/movie/{movieId}?api_key={API_KEY}&language=en-US"
                .GetJsonAsync<JObject>();

            var extratedGenres = movieData["genres"].ToArray();

            foreach (var genre in extratedGenres)
            {
                genres.Add(genre["name"].ToString().Equals("Science Fiction") ? "sci-fi" : genre["name"].ToString());
            }

            var actors = new List<string>();
            var peopleData =
                await $"https://api.themoviedb.org/3/movie/{movieId}/credits?api_key={API_KEY}"
                .GetJsonAsync<JObject>();

            var castMembers = peopleData["cast"].ToArray();

            foreach (var cast in castMembers)
            {
                actors.Add(cast["name"].ToString().Replace(".", string.Empty));
            }

            return new MovieDto()
            {
                Actors = actors,
                Genres = genres
            };
        }
    }
}
