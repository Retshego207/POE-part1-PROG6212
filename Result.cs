namespace RaceDayAPI
{
    public class Result
    {
        public int result {  get; set; }
        public int enrollmentID { get; set; }
        public TimeSpan? finishTime { get; set; }
        public int? finishPosition { get; set; }
        public string? averagePace { get; set; }
        public string resultStatus { get; set; }
       
    }
}
