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
    public class RemarksController : ControllerBase
    {
        private readonly CnctnsContext _context;
        public IConfiguration Config { get; }

        public RemarksController(CnctnsContext context, IConfiguration config)
        {
            _context = context;
            Config = config;
        }

        // GET: api/Remarks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Remark>>> GetRemarks()
        {
            try
            {
                _context.Remarks.Any();
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                  //CreateTable();
              //else
                    throw;
            }

            return await _context.Set<Remark>().OrderBy(remark => remark.Id).ToListAsync();
        }

        // GET: api/Remarks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Remark>> GetRemark(char id)
        {
            Remark remark;

            try
            {
                remark = await _context.Remarks.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (remark == null)
            {
                return NotFound();
            }

            return remark;
        }

        // PUT: api/Remarks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRemark(char id, Remark remark)
        {
            try
            {
                remark = await _context.Remarks.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (RemarkExists(id))
            {
                remark.Id = id;
                foreach (PropertyInfo pi in typeof(Remark).GetProperties())
                {
                    if (pi.GetValue(remark) != null)
                    {
                        if (!pi.Name.Equals("Id"))
                        {
                            _context.Entry(remark).Property(pi.Name).IsModified = true;
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

            return Ok(remark);
        }

        // POST: api/Remarks
        [HttpPost]
        public async Task<ActionResult<Remark>> PostRemark(Remark remark)
        {
            try
            {
                _context.Remarks.Any();
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                  //CreateTable();
              //else
                    throw;
            }

            _context.Remarks.Add(remark);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBus", new { id = remark.Id }, remark);
        }

        // DELETE: api/Remarks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Remark>> DeleteRemark(char id)
        {
            Remark remark;

            try
            {
                remark = await _context.Remarks.FindAsync(id);
            }
            catch (PostgresException e)
            {
                if (e.SqlState.Equals("42P01"))
                    return NotFound();
                else
                    throw;
            }

            if (remark == null)
            {
                return NotFound();
            }
            _context.Remarks.Remove(remark);
            await _context.SaveChangesAsync();

            return remark;
        }

        private bool RemarkExists(char id)
        {
            return _context.Remarks.Any(e => e.Id == id);
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
