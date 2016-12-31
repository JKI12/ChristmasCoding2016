namespace ChristmasCoding.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Uuid { get; set; }
        public string Title { get; set; }
        public string Rating { get; set; }
        public int Duration { get; set; }
        public string Actors { get; set; }
        public string Genres { get; set; }
        public string Synopsis { get; set; }
    }
}
