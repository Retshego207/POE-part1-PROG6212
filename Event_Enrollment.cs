namespace RaceDayAPI
{
    public class Event_Enrollment
    {
        public int enrollmentID {  get; set; }
        public int eventID { get; set; }
        public int userID { get; set; }
        public DateTime enrollmentdate { get; set; }
        public string status { get; set; }
        public string? raceNumber { get; set; }
    }
}
