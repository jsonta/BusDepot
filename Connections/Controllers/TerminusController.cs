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
    public class TerminusController : ControllerBase
    {
        private readonly CnctnsContext _context;
        public TerminusController(CnctnsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Wypisuje wszystkie przystanki ze szczegółami w formie listy.
        /// </summary>
        /// <response code="200">Lista obiektów JSON</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Terminus>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Terminus>>> GetTerminuss()
        {
            try
            {
                _context.terminus.Any();
            }
            catch (PostgresException e)
            {
                throw new NpgsqlException("Błąd serwera SQL - " + e.MessageText + " (kod " + e.SqlState + ")");
            }

            return await _context.Set<Terminus>().OrderBy(terminus => terminus.id).ToListAsync();
        }

        /// <summary>
        /// Wypisuje przystanek ze szczegółami, określony przez jego ID.
        /// </summary>
        /// <param name="id" example="1">Identyfikator przystanku</param>
        /// <response code="200">Obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Terminus), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Terminus>> GetTerminus(int? id)
        {
            Terminus terminus;

            try
            {
                terminus = await _context.terminus.FindAsync(id);
            }
            catch (PostgresException e)
            {
                throw new NpgsqlException("Błąd serwera SQL - " + e.MessageText + " (kod " + e.SqlState + ")");
            }

            if (terminus == null)
                return NotFound("Nie znaleziono");

            return terminus;
        }

        /// <summary>
        /// Aktualizuje szczegóły przystanku, określonego przez jego ID.
        /// </summary>
        /// <param name="id" example="1">Identyfikator przystanku</param>
        /// <param name="update">Parametry, jakie mają zostać zaktualizowane (w formie obiektu JSON). Wystarczy podać tylko nowe wartości - pozostałe zostaną skopiowane.</param>
        /// <response code="200">Aktualizacja pomyślna, zaktualizowany obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Terminus), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutTerminus(int? id, Terminus update)
        {
            Terminus terminus;

            try
            {
                terminus = await _context.terminus.FindAsync(id);
                _context.Entry(terminus).State = EntityState.Detached;
            }
            catch (PostgresException e)
            {
                throw new NpgsqlException("Błąd serwera SQL - " + e.MessageText + " (kod " + e.SqlState + ")");
            }

            if (terminus != null)
            {
                foreach (PropertyInfo pi in typeof(Terminus).GetProperties())
                {
                    if ((pi.GetValue(update) != pi.GetValue(terminus)) && (pi.GetValue(update) != null))
                        _context.Entry(update).Property(pi.Name).IsModified = true;
                    else if (pi.Name.Equals("id"))
                        update.id = id;
                }
            }
            else
                return NotFound("Nie znaleziono");

            try
            {
                await _context.SaveChangesAsync();
                _context.Entry(update).State = EntityState.Detached;
                terminus = await _context.terminus.FindAsync(id);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException("Błąd podczas aktualizacji bazy danych - " + e.Message);
            }

            return Ok(terminus);
        }

        /// <summary>
        /// Dodaje nowy przystanek do spisu przystanków (bazy danych).
        /// </summary>
        /// <param name="terminus">Parametry przystanku (w formie obiektu JSON). Wszystkie muszą być wypełnione.</param>
        /// <response code="201">Pomyślnie stworzono, nowy obiekt JSON</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpPost]
        [ProducesResponseType(typeof(Terminus), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Terminus>> PostTerminus(Terminus terminus)
        {
            try
            {
                _context.terminus.Any();
            }
            catch (PostgresException e)
            {
                throw new NpgsqlException("Błąd serwera SQL - " + e.MessageText + " (kod " + e.SqlState + ")");
            }

            _context.terminus.Add(terminus);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException("Błąd podczas aktualizacji bazy danych - " + e.Message);
            }

            return CreatedAtAction("GetTerminus", new { terminus.id }, terminus);
        }

        /// <summary>
        /// Usuwa przystanek, określony przez jego ID, ze spisu przystanków (bazy danych).
        /// </summary>
        /// <param name="id" example="1">Identyfikator przystanku</param>
        /// <response code="200">Operacja wykonana pomyślnie, usunięty obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Terminus), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Terminus>> DeleteTerminus(int? id)
        {
            Terminus terminus;

            try
            {
                terminus = await _context.terminus.FindAsync(id);
            }
            catch (PostgresException e)
            {
                throw new NpgsqlException("Błąd serwera SQL - " + e.MessageText + " (kod " + e.SqlState + ")");
            }

            if (terminus == null)
                return NotFound("Nie znaleziono");

            _context.terminus.Remove(terminus);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException("Błąd podczas aktualizacji bazy danych - " + e.Message);
            }

            return terminus;
        }
    }
}
