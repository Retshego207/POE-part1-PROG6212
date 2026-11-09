using System.Diagnostics.Contracts;

namespace RaceDayMVC.Models
{
    public class EnrollmentViewModel
    {
        internal string status;

        public int enrollmentID { get; set; }
        public int eventID { get; set; }
        public int userID { get; set; }

        public DateTime enrollmentDate { get; set; }
        public String Status { get; set; } = "Pending";
        public int? raceNumber { get; set; }

        public String eventName { get; set; } = "";
        public String location { get; set; } = "";
        public DateTime eventDate { get; set; }

    }
}
