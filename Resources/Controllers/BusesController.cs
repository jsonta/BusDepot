using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Resources.Models;

namespace Resources.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusesController : ControllerBase
    {
        private readonly RsrcsContext _context;
        public BusesController(RsrcsContext context)
        {
            _context = context;
        }

        // GET: api/Buses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bus>>> GetBuses()
        {
            try
            {
                _context.buses.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            return await _context.Set<Bus>().OrderBy(bus => bus.id).ToListAsync();
        }

        // GET: api/Buses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bus>> GetBus(int? id)
        {
            Bus bus;

            try
            {
                bus = await _context.buses.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (bus == null)
                return NotFound();

            return bus;
        }

        // PUT: api/Buses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBus(int? id, Bus update)
        {
            Bus current;

            try
            {
                current = await _context.buses.FindAsync(id);
                _context.Entry(current).State = EntityState.Detached;
            }
            catch (PostgresException)
            {
                throw;
            }

            if (current != null)
            {
                foreach (PropertyInfo pi in typeof(Bus).GetProperties())
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

        // POST: api/Buses
        [HttpPost]
        public async Task<ActionResult<Bus>> PostBus(Bus bus)
        {
            try
            {
                _context.buses.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            _context.buses.Add(bus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBus", new { bus.id }, bus);
        }

        // DELETE: api/Buses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Bus>> DeleteBus(int? id)
        {
            Bus bus;

            try
            {
                bus = await _context.buses.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (bus == null)
                return NotFound();

            _context.buses.Remove(bus);
            await _context.SaveChangesAsync();

            return bus;
        }
    }
}
