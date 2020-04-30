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
    public class RemarksController : ControllerBase
    {
        private readonly CnctnsContext _context;
        public RemarksController(CnctnsContext context)
        {
            _context = context;
        }

        // GET: api/Remarks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Remark>>> GetRemarks()
        {
            try
            {
                _context.remarks.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            return await _context.Set<Remark>().OrderBy(remark => remark.id).ToListAsync();
        }

        // GET: api/Remarks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Remark>> GetRemark(char id)
        {
            Remark remark;

            try
            {
                remark = await _context.remarks.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (remark == null)
                return NotFound();

            return remark;
        }

        // PUT: api/Remarks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRemark(char id, Remark update)
        {
            Remark current;

            try
            {
                current = await _context.remarks.FindAsync(id);
                _context.Entry(current).State = EntityState.Detached;
            }
            catch (PostgresException)
            {
                throw;
            }

            if (current != null)
            {
                foreach (PropertyInfo pi in typeof(Remark).GetProperties())
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

        // POST: api/Remarks
        [HttpPost]
        public async Task<ActionResult<Remark>> PostRemark(Remark remark)
        {
            try
            {
                _context.remarks.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            _context.remarks.Add(remark);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRemark", new { remark.id }, remark);
        }

        // DELETE: api/Remarks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Remark>> DeleteRemark(char id)
        {
            Remark remark;

            try
            {
                remark = await _context.remarks.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (remark == null)
                return NotFound();

            _context.remarks.Remove(remark);
            await _context.SaveChangesAsync();

            return remark;
        }
    }
}
