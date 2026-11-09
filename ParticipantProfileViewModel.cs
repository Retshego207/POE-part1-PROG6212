namespace RaceDayMVC.Models
{
    public class ParticipantProfileViewModel
    {
        public int profileID { get; set; }
        public int userID { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string gender { get; set; } = "";
        public string emergencyContact { get; set; } = "";
        public String? clubName { get; set; }
    }
}
