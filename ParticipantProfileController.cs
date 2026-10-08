using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace RaceDayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantProfileController : ControllerBase
    {
        private readonly RaceDayDBContext _context;

        public ParticipantProfileController(RaceDayDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticipantProfile>>> GetParticipantProfiles()
        {
            return await _context.participantProfiles.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParticipantProfile>> GetParticipantProfile(int id)
        {
            var profile = await _context.participantProfiles.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
            }
            return profile;
        }
    }
}
