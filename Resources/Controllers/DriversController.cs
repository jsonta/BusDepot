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
    public class DriversController : ControllerBase
    {
        private readonly RsrcsContext _context;
        public DriversController(RsrcsContext context)
        {
            _context = context;
        }

        // GET: api/Drivers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
        {
            try
            {
                _context.drivers.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            return await _context.Set<Driver>().OrderBy(driver => driver.id).ToListAsync();
        }

        // GET: api/Drivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Driver>> GetDriver(long? id)
        {
            Driver driver;

            try
            {
                driver = await _context.drivers.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (driver == null)
                return NotFound();

            return driver;
        }

        // PUT: api/Drivers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(long? id, Driver update)
        {
            Driver current;

            try
            {
                current = await _context.drivers.FindAsync(id);
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

        // POST: api/Drivers
        [HttpPost]
        public async Task<ActionResult<Driver>> PostDriver(Driver driver)
        {
            try
            {
                _context.drivers.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            _context.drivers.Add(driver);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriver", new { driver.id }, driver);
        }

        // DELETE: api/Drivers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Driver>> DeleteDriver(long? id)
        {
            Driver driver;

            try
            {
                driver = await _context.drivers.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (driver == null)
                return NotFound();

            _context.drivers.Remove(driver);
            await _context.SaveChangesAsync();

            return driver;
        }
    }
}
