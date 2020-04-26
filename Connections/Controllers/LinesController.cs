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
    public class LinesController : ControllerBase
    {
        private readonly CnctnsContext _context;
        public IConfiguration Config { get; }

        public LinesController(CnctnsContext context, IConfiguration config)
        {
            _context = context;
            Config = config;
        }

        // GET: api/Lines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Line>>> GetLines()
        {
            try
            {
                _context.Lines.Any();
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                  //CreateTable();
              //else
                    throw;
            }

            return await _context.Set<Line>().OrderBy(line => line.Id).ToListAsync();
        }

        // GET: api/Lines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Line>> GetLine(int? id)
        {
            Line line;

            try
            {
                line = await _context.Lines.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (line == null)
            {
                return NotFound();
            }

            return line;
        }

        // PUT: api/Lines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLine(int? id, Line line)
        {
            try
            {
                line = await _context.Lines.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (LineExists(id))
            {
                line.Id = id;
                foreach (PropertyInfo pi in typeof(Line).GetProperties())
                {
                    if (pi.GetValue(line) != null || line.HasIntValue(pi.Name))
                    {
                        if (!pi.Name.Equals("Id"))
                        {
                            _context.Entry(line).Property(pi.Name).IsModified = true;
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

            return Ok(line);
        }

        // POST: api/Lines
        [HttpPost]
        public async Task<ActionResult<Line>> PostLine(Line line)
        {
            try
            {
                _context.Lines.Any();
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                  //CreateTable();
              //else
                    throw;
            }

            _context.Lines.Add(line);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLine", new { id = line.Id }, line);
        }

        // DELETE: api/Lines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Line>> DeleteLine(int? id)
        {
            Line line;

            try
            {
                line = await _context.Lines.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (line == null)
            {
                return NotFound();
            }
            _context.Lines.Remove(line);
            await _context.SaveChangesAsync();

            return line;
        }

        private bool LineExists(int? id)
        {
            return _context.Lines.Any(e => e.Id == id);
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
