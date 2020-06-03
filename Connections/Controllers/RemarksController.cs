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
    [Route("connections/[controller]")]
    [ApiController]
    public class RemarksController : ControllerBase
    {
        private readonly CnctnsContext _context;
        public RemarksController(CnctnsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Wypisuje wszystkie uwagi ze szczegółami w formie listy.
        /// </summary>
        /// <response code="200">Lista obiektów JSON</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Remark>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Remark>>> GetRemarks()
        {
            try
            {
                _context.remarks.Any();
            }
            catch (PostgresException e)
            {
                throw new NpgsqlException("Błąd serwera SQL - " + e.MessageText + " (kod " + e.SqlState + ")");
            }

            return await _context.Set<Remark>().OrderBy(remark => remark.id).ToListAsync();
        }

        /// <summary>
        /// Wypisuje uwagę ze szczegółami, określoną przez jej ID.
        /// </summary>
        /// <param name="id" example="P">Identyfikator uwagi</param>
        /// <response code="200">Obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Remark), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Remark>> GetRemark(char id)
        {
            Remark remark;

            try
            {
                remark = await _context.remarks.FindAsync(id);
            }
            catch (PostgresException e)
            {
                throw new NpgsqlException("Błąd serwera SQL - " + e.MessageText + " (kod " + e.SqlState + ")");
            }

            if (remark == null)
                return NotFound("Nie znaleziono");

            return remark;
        }

        /// <summary>
        /// Aktualizuje szczegóły uwagi, określonej przez jej ID.
        /// </summary>
        /// <param name="id" example="P">Identyfikator uwagi</param>
        /// <param name="update">Parametry, jakie mają zostać zaktualizowane (w formie obiektu JSON). Wystarczy podać tylko nowe wartości - pozostałe zostaną skopiowane.</param>
        /// <response code="200">Aktualizacja pomyślna, zaktualizowany obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Remark), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutRemark(char id, Remark update)
        {
            Remark remark;

            try
            {
                remark = await _context.remarks.FindAsync(id);
                _context.Entry(remark).State = EntityState.Detached;
            }
            catch (PostgresException e)
            {
                throw new NpgsqlException("Błąd serwera SQL - " + e.MessageText + " (kod " + e.SqlState + ")");
            }

            if (remark != null)
            {
                update.id = id;
                foreach (PropertyInfo pi in typeof(Remark).GetProperties())
                {
                    if ((pi.GetValue(update) != pi.GetValue(remark))
                        && (pi.GetValue(update) != null)
                        && (!pi.Name.Equals("id")))
                        _context.Entry(update).Property(pi.Name).IsModified = true;
                }
            }
            else
                return NotFound("Nie znaleziono");

            try
            {
                await _context.SaveChangesAsync();
                _context.Entry(update).State = EntityState.Detached;
                remark = await _context.remarks.FindAsync(id);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException("Błąd podczas aktualizacji bazy danych - " + e.Message);
            }

            return Ok(remark);
        }

        /// <summary>
        /// Dodaje nową uwagę do spisu uwag (bazy danych).
        /// </summary>
        /// <param name="remark">Parametry uwagi (w formie obiektu JSON). Wszystkie muszą być wypełnione.</param>
        /// <response code="201">Pomyślnie stworzono, nowy obiekt JSON</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpPost]
        [ProducesResponseType(typeof(Remark), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Remark>> PostRemark(Remark remark)
        {
            try
            {
                _context.remarks.Any();
            }
            catch (PostgresException e)
            {
                throw new NpgsqlException("Błąd serwera SQL - " + e.MessageText + " (kod " + e.SqlState + ")");
            }

            _context.remarks.Add(remark);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException("Błąd podczas aktualizacji bazy danych - " + e.Message);
            }

            return CreatedAtAction("GetRemark", new { remark.id }, remark);
        }

        /// <summary>
        /// Usuwa uwagę, określoną przez jej ID, ze spisu uwag (bazy danych).
        /// </summary>
        /// <param name="id" example="P">Identyfikator uwagi</param>
        /// <response code="200">Operacja wykonana pomyślnie, usunięty obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Remark), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Remark>> DeleteRemark(char id)
        {
            Remark remark;

            try
            {
                remark = await _context.remarks.FindAsync(id);
            }
            catch (PostgresException e)
            {
                throw new NpgsqlException("Błąd serwera SQL - " + e.MessageText + " (kod " + e.SqlState + ")");
            }

            if (remark == null)
                return NotFound("Nie znaleziono");

            _context.remarks.Remove(remark);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException("Błąd podczas aktualizacji bazy danych - " + e.Message);
            }

            return remark;
        }
    }
}
