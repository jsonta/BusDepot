using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Connections.Models;
using Npgsql;
using System.Reflection;

namespace Connections.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimetablesController : ControllerBase
    {
        private readonly CnctnsContext _context;
        public TimetablesController(CnctnsContext context)
        {
            _context = context;
        }

        // GET: api/Timetables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Timetable>>> Getbrigades_timetable()
        {
            try
            {
                _context.brigades_timetable.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            return await _context.Set<Timetable>().OrderBy(brigade => brigade.id).ToListAsync();
        }

        // GET: api/Timetables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Timetable>> GetTimetable(string id)
        {
            Timetable timetable;

            try
            {
                timetable = await _context.brigades_timetable.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (timetable == null)
                return NotFound();

            return timetable;
        }

        // PUT: api/Timetables/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimetable(string id, Timetable update)
        {
            Timetable current;

            try
            {
                current = await _context.brigades_timetable.FindAsync(id);
                _context.Entry(current).State = EntityState.Detached;
            }
            catch (PostgresException)
            {
                throw;
            }

            if (current != null)
            {
                foreach (PropertyInfo pi in typeof(Timetable).GetProperties())
                {
                    if ((pi.GetValue(update) != pi.GetValue(current)) && (pi.GetValue(update) != null))
                        _context.Entry(update).Property(pi.Name).IsModified = true;
                    else if (pi.Name.Equals("id"))
                        update.id = id;
                }
            }
            else
                return NotFound();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(update);
        }

        // POST: api/Timetables
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Timetable>> PostTimetable(Timetable timetable)
        {
            try
            {
                _context.brigades_timetable.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            _context.brigades_timetable.Add(timetable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTimetable", new { timetable.id }, timetable);
        }

        // DELETE: api/Timetables/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Timetable>> DeleteTimetable(string id)
        {
            Timetable timetable;

            try
            {
                timetable = await _context.brigades_timetable.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (timetable == null)
                return NotFound();

            _context.brigades_timetable.Remove(timetable);
            await _context.SaveChangesAsync();

            return timetable;
        }
    }
}
