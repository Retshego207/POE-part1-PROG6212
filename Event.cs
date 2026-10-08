namespace RaceDayAPI
{
    public class Event
    {
        public int eventID { get; set; }
        public int categoryID { get; set; }
        public string organiserID { get; set; }
        public string eventName { get; set; }
        public DateTime eventDate { get; set; }
        public TimeSpan startTime { get; set; }
        public string location { get; set; }
        public string distance { get; set; }
        public string? description { get; set; }
        public decimal entryFee { get; set; }
        public string status { get; set; }
    }
}
