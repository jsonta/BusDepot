using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using BusDepot.Models;

namespace Resources.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly RsrcsContext _context;
        public IConfiguration Config { get; }

        public DriversController(RsrcsContext context, IConfiguration config)
        {
            _context = context;
            Config = config;
        }

        // GET: api/Drivers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
        {
            try
            {
                _context.Drivers.Any();
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    CreateTable();
                else
                    throw;
            }

            return await _context.Set<Driver>().OrderBy(driver => driver.Id).ToListAsync();
        }

        // GET: api/Drivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Driver>> GetDriver(long? id)
        {
            Driver driver;

            try
            {
                driver = await _context.Drivers.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (driver == null)
            {
                return NotFound();
            }

            return driver;
        }

        // PUT: api/Drivers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(long? id, Driver driver)
        {
            try
            {
                driver = await _context.Drivers.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (DriverExists(id))
            {
                driver.Id = id;
                foreach (PropertyInfo pi in typeof(Bus).GetProperties())
                {
                    if (pi.GetValue(driver) != null || driver.HasIntValue(pi.Name))
                    {
                        if (!pi.Name.Equals("Id"))
                        {
                            _context.Entry(driver).Property(pi.Name).IsModified = true;
                        }
                    }
                }
            }
            else
            {
                return NotFound();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(driver);
        }

        // POST: api/Drivers
        [HttpPost]
        public async Task<ActionResult<Driver>> PostDriver(Driver driver)
        {
            try
            {
                _context.Drivers.Any();
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    CreateTable();
                else
                    throw;
            }

            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriver", new { id = driver.Id }, driver);
        }

        // DELETE: api/Drivers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Driver>> DeleteDriver(long? id)
        {
            Driver driver;

            try
            {
                driver = await _context.Drivers.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (driver == null)
            {
                return NotFound();
            }

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();

            return driver;
        }

        private bool DriverExists(long? id)
        {
            return _context.Drivers.Any(e => e.Id == id);
        }

        private void CreateTable()
        {
            NpgsqlConnection conn = new NpgsqlConnection(Config.GetConnectionString("MyWebApiConection"));
            string query = @"CREATE TABLE ""Drivers"" (
                                ""Id"" bigint PRIMARY KEY NOT NULL,
	                            ""FirstName"" text NOT NULL,
	                            ""LastName"" text NOT NULL,
	                            ""Birthday"" text NOT NULL,
	                            ""Phone"" bigint NOT NULL,
	                            ""Email"" text,
	                            ""StreetName"" text NOT NULL,
	                            ""BuildingNumber"" int NOT NULL,
	                            ""ApartmentNumber"" int,
	                            ""City"" text NOT NULL,
                                ""ZipCode"" text NOT NULL
                            );";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (PostgresException)
            {
                throw;
            }
        }
    }
}
