namespace ChristmasCoding.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class FacebookUser
    {
        [Key, Required]
        public string UserId { get; set; }

        [Required]
        public string AuthToken { get; set; }

        public string Psid { get; set; }
    }
}
