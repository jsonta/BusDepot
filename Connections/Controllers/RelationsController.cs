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
    public class RelationsController : ControllerBase
    {
        private readonly CnctnsContext _context;
        public RelationsController(CnctnsContext context)
        {
            _context = context;
        }

        // GET: api/Relations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Relation>>> GetRelations()
        {
            try
            {
                _context.relations.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            return await _context.Set<Relation>().OrderBy(relation => relation.id).ToListAsync();
        }

        // GET: api/Relations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Relation>> GetRelation(string id)
        {
            Relation relation;

            try
            {
                relation = await _context.relations.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (relation == null)
                return NotFound();

            return relation;
        }

        // PUT: api/Relations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRelation(string id, Relation update)
        {
            Relation current;

            try
            {
                current = await _context.relations.FindAsync(id);
                _context.Entry(current).State = EntityState.Detached;
            }
            catch (PostgresException)
            {
                throw;
            }

            if (current != null)
            {
                foreach (PropertyInfo pi in typeof(Relation).GetProperties())
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

        // POST: api/Relations
        [HttpPost]
        public async Task<ActionResult<Relation>> PostRelation(Relation relation)
        {
            try
            {
                _context.relations.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            _context.relations.Add(relation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRelation", new { relation.id }, relation);
        }

        // DELETE: api/Relations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Relation>> DeleteRelation(string id)
        {
            Relation relation;

            try
            {
                relation = await _context.relations.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (relation == null)
                return NotFound();

            _context.relations.Remove(relation);
            await _context.SaveChangesAsync();

            return relation;
        }
    }
}
