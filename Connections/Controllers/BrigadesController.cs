using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Connections.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Reflection;

namespace Connections.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrigadesController : ControllerBase
    {
        private readonly CnctnsContext _context;
        public IConfiguration Config { get; }

        public BrigadesController(CnctnsContext context, IConfiguration config)
        {
            _context = context;
            Config = config;
        }

        // GET: api/Brigades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brigade>>> GetBrigades()
        {
            try
            {
                _context.Brigades.Any();
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                  //CreateTable();
              //else
                    throw;
            }

            return await _context.Set<Brigade>().OrderBy(brigade => brigade.Line).ToListAsync();
        }

        // GET: api/Brigades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brigade>> GetBrigade(int? id)
        {
            Brigade brigade;

            try
            {
                brigade = await _context.Brigades.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (brigade == null)
            {
                return NotFound();
            }

            return brigade;
        }

        // PUT: api/Brigades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrigade(int? id, Brigade brigade)
        {
            try
            {
                brigade = await _context.Brigades.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (BrigadeExists(id))
            {
                brigade.Id = id;
                foreach (PropertyInfo pi in typeof(Brigade).GetProperties())
                {
                    if (pi.GetValue(brigade) != null || brigade.HasIntValue(pi.Name))
                    {
                        if (!pi.Name.Equals("Id"))
                        {
                            _context.Entry(brigade).Property(pi.Name).IsModified = true;
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

            return Ok(brigade);
        }

        // POST: api/Brigades
        [HttpPost]
        public async Task<ActionResult<Brigade>> PostBrigade(Brigade brigade)
        {
            try
            {
                _context.Brigades.Any();
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                  //CreateTable();
              //else
                    throw;
            }

            _context.Brigades.Add(brigade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBrigade", new { id = brigade.Id }, brigade);
        }

        // DELETE: api/Brigades/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Brigade>> DeleteBrigade(int? id)
        {
            Brigade brigade;

            try
            {
                brigade = await _context.Brigades.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (brigade == null)
            {
                return NotFound();
            }
            _context.Brigades.Remove(brigade);
            await _context.SaveChangesAsync();

            return brigade;
        }

        private bool BrigadeExists(int? id)
        {
            return _context.Brigades.Any(e => e.Id == id);
        }

        private void CreateTable()
        {
            /*
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
            */
        }
    }
}
