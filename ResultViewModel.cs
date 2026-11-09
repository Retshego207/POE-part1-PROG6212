namespace RaceDayMVC.Models
{
    public class ResultViewModel
    {
        public int result { get; set; }

        public int enrollmentID { get; set; }

        public TimeSpan? finishTime { get; set; }

        public int? finishPosition { get; set; }

        public string? averagePace { get; set; }

        public string resultStatus { get; set; } = "";

        public string eventName { get; set; } = "";

        public DateTime eventDate { get; set; }
    }
}