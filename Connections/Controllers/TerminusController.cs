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
    public class TerminusController : ControllerBase
    {
        private readonly CnctnsContext _context;
        public TerminusController(CnctnsContext context)
        {
            _context = context;
        }

        // GET: api/Terminuss
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Terminus>>> GetTerminuss()
        {
            try
            {
                _context.terminus.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            return await _context.Set<Terminus>().OrderBy(terminus => terminus.id).ToListAsync();
        }

        // GET: api/Terminuss/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Terminus>> GetTerminus(int? id)
        {
            Terminus terminus;

            try
            {
                terminus = await _context.terminus.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (terminus == null)
                return NotFound();

            return terminus;
        }

        // PUT: api/Terminuss/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTerminus(int? id, Terminus update)
        {
            Terminus current;

            try
            {
                current = await _context.terminus.FindAsync(id);
                _context.Entry(current).State = EntityState.Detached;
            }
            catch (PostgresException)
            {
                throw;
            }

            if (current != null)
            {
                foreach (PropertyInfo pi in typeof(Terminus).GetProperties())
                {
                    if (pi.GetValue(update) != pi.GetValue(current))
                    {
                        if (!pi.Name.Equals("id"))
                            _context.Entry(update).Property(pi.Name).IsModified = true;
                        else
                            update.id = id;
                    }
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

        // POST: api/Terminuss
        [HttpPost]
        public async Task<ActionResult<Terminus>> PostTerminus(Terminus terminus)
        {
            try
            {
                _context.terminus.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            _context.terminus.Add(terminus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTerminus", new { terminus.id }, terminus);
        }

        // DELETE: api/Terminuss/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Terminus>> DeleteTerminus(int? id)
        {
            Terminus terminus;

            try
            {
                terminus = await _context.terminus.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (terminus == null)
                return NotFound();

            _context.terminus.Remove(terminus);
            await _context.SaveChangesAsync();

            return terminus;
        }
    }
}
