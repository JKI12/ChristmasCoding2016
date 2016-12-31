namespace ChristmasCoding.Data.Dto
{
    using System.Collections.Generic;

    public class MovieDto
    {
        public string Uuid { get; set; }
        public string Title { get; set; }
        public string Rating { get; set; }
        public int Duration { get; set; }
        public List<string> Actors { get; set; }
        public List<string> Genres { get; set; }
        public string Synopsis { get; set; }
    }
}
