namespace ChristmasCoding.Data.Models
{
    using System.Collections.Generic;

    public class RecomendationModel
    {
        public Dictionary<string, int> ActorData { get; set; }
        public Dictionary<string, int> GenreData { get; set; }

        public RecomendationModel()
        {
            ActorData = new Dictionary<string, int>();
            GenreData = new Dictionary<string, int>();
        }
    }
}
