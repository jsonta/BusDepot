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
    public class LinesController : ControllerBase
    {
        private readonly CnctnsContext _context;
        public LinesController(CnctnsContext context)
        {
            _context = context;
        }

        // GET: api/Lines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Line>>> GetLines()
        {
            try
            {
                _context.lines.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            return await _context.Set<Line>().OrderBy(line => line.id).ToListAsync();
        }

        // GET: api/Lines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Line>> GetLine(int? id)
        {
            Line line;

            try
            {
                line = await _context.lines.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (line == null)
                return NotFound();

            return line;
        }

        // PUT: api/Lines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLine(int? id, Line update)
        {
            Line current;

            try
            {
                current = await _context.lines.FindAsync(id);
                _context.Entry(current).State = EntityState.Detached;
            }
            catch (PostgresException)
            {
                throw;
            }

            if (current != null)
            {
                foreach (PropertyInfo pi in typeof(Line).GetProperties())
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

        // POST: api/Lines
        [HttpPost]
        public async Task<ActionResult<Line>> PostLine(Line line)
        {
            try
            {
                _context.lines.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            _context.lines.Add(line);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLine", new { line.id }, line);
        }

        // DELETE: api/Lines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Line>> DeleteLine(int? id)
        {
            Line line;

            try
            {
                line = await _context.lines.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (line == null)
                return NotFound();

            _context.lines.Remove(line);
            await _context.SaveChangesAsync();

            return line;
        }
    }
}
