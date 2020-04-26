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
    public class RelationsController : ControllerBase
    {
        private readonly CnctnsContext _context;
        public IConfiguration Config { get; }

        public RelationsController(CnctnsContext context, IConfiguration config)
        {
            _context = context;
            Config = config;
        }

        // GET: api/Relations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Relation>>> GetRelations()
        {
            try
            {
                _context.Relations.Any();
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                  //CreateTable();
              //else
                    throw;
            }

            return await _context.Set<Relation>().OrderBy(relation => relation.Line).ToListAsync();
        }

        // GET: api/Relations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Relation>> GetRelation(int? id)
        {
            Relation relation;

            try
            {
                relation = await _context.Relations.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (relation == null)
            {
                return NotFound();
            }

            return relation;
        }

        // PUT: api/Relations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRelation(int? id, Relation relation)
        {
            try
            {
                relation = await _context.Relations.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (RelationExists(id))
            {
                relation.Id = id;
                foreach (PropertyInfo pi in typeof(Relation).GetProperties())
                {
                    if (pi.GetValue(relation) != null || relation.HasIntValue(pi.Name))
                    {
                        if (!pi.Name.Equals("Id"))
                        {
                            _context.Entry(relation).Property(pi.Name).IsModified = true;
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

            return Ok(relation);
        }

        // POST: api/Relations
        [HttpPost]
        public async Task<ActionResult<Relation>> PostRelation(Relation relation)
        {
            try
            {
                _context.Relations.Any();
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                  //CreateTable();
              //else
                    throw;
            }

            _context.Relations.Add(relation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBus", new { id = relation.Id }, relation);
        }

        // DELETE: api/Relations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Relation>> DeleteRelation(int? id)
        {
            Relation relation;

            try
            {
                relation = await _context.Relations.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (relation == null)
            {
                return NotFound();
            }
            _context.Relations.Remove(relation);
            await _context.SaveChangesAsync();

            return relation;
        }

        private bool RelationExists(int? id)
        {
            return _context.Relations.Any(e => e.Id == id);
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
