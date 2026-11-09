using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string firstName { get; set; } = "";

        [Required]
        public string surName { get; set; } = "";

        [Required]
        [EmailAddress]
        public string email { get; set; } = "";

        [Required]
        [MinLength(8)]
        public string password { get; set; } = "";

        [Required]
        public string role { get; set; } = "Participant";
    }
}