using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RaceDayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDBContext _context;
        public ResultsController(RaceDayDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Result>>> GetResults()
        {
            return await _context.results.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result>>GetResults(int id)
        {
            var result = await _context.results.FindAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }
    }
}
