using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusDepot.Models;
using System.Reflection;

namespace BusDepot.Controllers
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
        public IEnumerable<Bus> GetBuses()
        {
            return _context.Set<Bus>().OrderBy(bus => bus.Id);
        }

        // GET: api/Buses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBus([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bus = await _context.Buses.FindAsync(id);

            if (bus == null)
            {
                return NotFound();
            }

            return Ok(bus);
        }

        // PUT: api/Buses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBus([FromRoute] int id, [FromBody] Bus bus)
        {
            bus.Id = id;
            foreach (PropertyInfo prop in typeof(Bus).GetProperties())
            {
                if (prop.GetValue(bus) != null || bus.HasIntValue(prop.Name))
                {
                    if (!prop.Name.Equals("Id"))
                    {
                        _context.Entry(bus).Property(prop.Name).IsModified = true;
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(bus);
        }

        // POST: api/Buses
        [HttpPost]
        public async Task<IActionResult> PostBus([FromBody] Bus bus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Buses.Add(bus);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBus), new { id = bus.Id }, bus);
        }

        // DELETE: api/Buses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBus([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
            {
                return NotFound();
            }

            _context.Buses.Remove(bus);
            await _context.SaveChangesAsync();

            return Ok(bus);
        }

        private bool BusExists(int id)
        {
            return _context.Buses.Any(e => e.Id == id);
        }
    }
}