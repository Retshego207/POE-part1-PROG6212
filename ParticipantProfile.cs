namespace RaceDayAPI
{
    public class ParticipantProfile
    {
        public int profileID { get; set; }
        public int userID { get; set; }
        public DateTime dateofBirth { get; set; }
        public string gender { get; set; }
        public string emergencyContact { get; set; }
        public string clubName { get; set; }
    }
}
