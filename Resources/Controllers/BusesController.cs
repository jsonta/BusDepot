using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Resources.Models;

namespace Resources.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusesController : ControllerBase
    {
        private readonly RsrcsContext _context;
        public IConfiguration Config { get; }

        public BusesController(RsrcsContext context, IConfiguration config)
        {
            _context = context;
            Config = config;
        }

        // GET: api/Buses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bus>>> GetBuses()
        {
            try
            {
                _context.Buses.Any();
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    CreateTable();
                else
                    throw;
            }

            return await _context.Set<Bus>().OrderBy(bus => bus.Id).ToListAsync();
        }

        // GET: api/Buses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bus>> GetBus(int? id)
        {
            Bus bus;

            try
            {
                bus = await _context.Buses.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (bus == null)
            {
                return NotFound();
            }

            return bus;
        }

        // PUT: api/Buses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBus(int? id, Bus bus)
        {
            try
            {
                bus = await _context.Buses.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (BusExists(id))
            {
                bus.Id = id;
                foreach (PropertyInfo pi in typeof(Bus).GetProperties())
                {
                    if (pi.GetValue(bus) != null || bus.HasIntValue(pi.Name))
                    {
                        if (!pi.Name.Equals("Id"))
                        {
                            _context.Entry(bus).Property(pi.Name).IsModified = true;
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

            return Ok(bus);
        }

        // POST: api/Buses
        [HttpPost]
        public async Task<ActionResult<Bus>> PostBus(Bus bus)
        {
            try
            {
                _context.Buses.Any();
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    CreateTable();
                else
                    throw;
            }

            _context.Buses.Add(bus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBus", new { id = bus.Id }, bus);
        }

        // DELETE: api/Buses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Bus>> DeleteBus(int? id)
        {
            Bus bus;

            try
            {
                bus = await _context.Buses.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (bus == null)
            {
                return NotFound();
            }
            _context.Buses.Remove(bus);
            await _context.SaveChangesAsync();

            return bus;
        }

        private bool BusExists(int? id)
        {
            return _context.Buses.Any(e => e.Id == id);
        }

        private void CreateTable()
        {
            NpgsqlConnection conn = new NpgsqlConnection(Config.GetConnectionString("MyWebApiConection"));
            string query = @"CREATE TABLE ""Buses"" (
                                ""Id"" int PRIMARY KEY NOT NULL,
	                            ""Brand"" text NOT NULL,
	                            ""Model"" text NOT NULL,
	                            ""Axes"" int NOT NULL,
	                            ""VRN"" text NOT NULL,
	                            ""ProdYear"" int NOT NULL,
	                            ""PrchYear"" int NOT NULL,
	                            ""PlcsAmnt"" int NOT NULL,
	                            ""CpctClss"" text NOT NULL,
	                            ""EN"" text NOT NULL
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