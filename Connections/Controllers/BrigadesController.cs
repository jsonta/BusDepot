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
    public class BrigadesController : ControllerBase
    {
        private readonly CnctnsContext _context;
        public BrigadesController(CnctnsContext context)
        {
            _context = context;
        }

        // GET: api/Brigades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brigade>>> GetBrigades()
        {
            try
            {
                _context.brigades.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            return await _context.Set<Brigade>().OrderBy(brigade => brigade.id).ToListAsync();
        }

        // GET: api/Brigades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brigade>> GetBrigade(string id)
        {
            Brigade brigade;

            try
            {
                brigade = await _context.brigades.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (brigade == null)
                return NotFound();

            return brigade;
        }

        // PUT: api/Brigades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrigade(string id, Brigade update)
        {
            Brigade current;

            try
            {
                current = await _context.brigades.FindAsync(id);
                _context.Entry(current).State = EntityState.Detached;
            }
            catch (PostgresException)
            {
                throw;
            }

            if (current != null)
            {
                foreach (PropertyInfo pi in typeof(Brigade).GetProperties())
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

        // POST: api/Brigades
        [HttpPost]
        public async Task<ActionResult<Brigade>> PostBrigade(Brigade brigade)
        {
            try
            {
                _context.brigades.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            _context.brigades.Add(brigade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBrigade", new { brigade.id }, brigade);
        }

        // DELETE: api/Brigades/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Brigade>> DeleteBrigade(string id)
        {
            Brigade brigade;

            try
            {
                brigade = await _context.brigades.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (brigade == null)
                return NotFound();

            _context.brigades.Remove(brigade);
            await _context.SaveChangesAsync();

            return brigade;
        }
    }
}
