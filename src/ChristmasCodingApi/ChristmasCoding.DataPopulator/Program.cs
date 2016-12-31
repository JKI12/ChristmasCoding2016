namespace ChristmasCoding.DataPopulator
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data;
    using Data.Models;
    using Microsoft.VisualBasic.FileIO;

    class Program
    {
        static MovieService ms = new MovieService();

        static void Main(string[] args)
        {
            var parser = new TextFieldParser(@"Z:\Downloads\data.csv")
            {
                HasFieldsEnclosedInQuotes = true
            };
            parser.SetDelimiters(",");
            
            while (!parser.EndOfData)
            {
                var fields = parser.ReadFields();

                if (parser.LineNumber == 2) continue;
                
                var movie = new Movie
                {
                    Uuid = fields[0],
                    Title = fields[1],
                    Duration = Int32.Parse(fields[4]),
                    Rating = fields[3],
                    Synopsis = fields[11],
                    Genres = fields[7]
                };

                var splitCast = fields[5].Split(',');
                var actors = new List<string>();

                foreach (var actor in splitCast)
                {
                    var a = actor.Split(':');
                    actors.Add(a[0]);
                }

                movie.Actors = string.Join(",", actors);

                Console.WriteLine($"Adding {movie.Title}");

                AddMovie(movie).Wait();
            }

            parser.Close();

            Console.WriteLine("Done...");
            Console.ReadLine();
        }

        static async Task AddMovie(Movie movie)
        {
            await ms.AddMovie(movie);
        }
    }
}
