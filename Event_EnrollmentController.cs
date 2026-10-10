using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RaceDayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Event_EnrollmentController : ControllerBase
    {
        private readonly RaceDayDBContext _context;
         public Event_EnrollmentController(RaceDayDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event_Enrollment>>> GetEvent_Enrollments()
        {
            return await _context.event_Enrollments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event_Enrollment>> GetEvent_Enrollment(int id)
        {
            var enrollment = await _context.event_Enrollments.FindAsync(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return enrollment;
        }
    }
}
