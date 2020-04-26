using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class TerminussController : ControllerBase
    {
        private readonly CnctnsContext _context;
        public IConfiguration Config { get; }

        public TerminussController(CnctnsContext context, IConfiguration config)
        {
            _context = context;
            Config = config;
        }

        // GET: api/Terminuss
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Terminus>>> GetTerminuss()
        {
            try
            {
                _context.Terminuss.Any();
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                  //CreateTable();
              //else
                    throw;
            }

            return await _context.Set<Terminus>().OrderBy(terminus => terminus.Id).ToListAsync();
        }

        // GET: api/Terminuss/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Terminus>> GetTerminus(int? id)
        {
            Terminus terminus;

            try
            {
                terminus = await _context.Terminuss.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (terminus == null)
            {
                return NotFound();
            }

            return terminus;
        }

        // PUT: api/Terminuss/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTerminus(int? id, Terminus terminus)
        {
            try
            {
                terminus = await _context.Terminuss.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (TerminusExists(id))
            {
                terminus.Id = id;
                foreach (PropertyInfo pi in typeof(Line).GetProperties())
                {
                    if (pi.GetValue(terminus) != null || terminus.HasIntValue(pi.Name))
                    {
                        if (!pi.Name.Equals("Id"))
                        {
                            _context.Entry(terminus).Property(pi.Name).IsModified = true;
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

            return Ok(terminus);
        }

        // POST: api/Terminuss
        [HttpPost]
        public async Task<ActionResult<Terminus>> PostTerminus(Terminus terminus)
        {
            try
            {
                _context.Terminuss.Any();
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                  //CreateTable();
              //else
                    throw;
            }

            _context.Terminuss.Add(terminus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLine", new { id = terminus.Id }, terminus);
        }

        // DELETE: api/Terminuss/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Terminus>> DeleteTerminus(int? id)
        {
            Terminus terminus;

            try
            {
                terminus = await _context.Terminuss.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (terminus == null)
            {
                return NotFound();
            }
            _context.Terminuss.Remove(terminus);
            await _context.SaveChangesAsync();

            return terminus;
        }

        private bool TerminusExists(int? id)
        {
            return _context.Terminuss.Any(e => e.Id == id);
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
