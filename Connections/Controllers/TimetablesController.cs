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
    public class TimetablesController : ControllerBase
    {
        private readonly CnctnsContext _context;
        public TimetablesController(CnctnsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Wypisuje wszystkie wpisy ze szczegółami w formie listy.
        /// </summary>
        /// <response code="200">Lista obiektów JSON</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Timetable>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Timetable>>> Getbrigades_timetable()
        {
            try
            {
                _context.brigades_timetable.Any();
            }
            catch (PostgresException e)
            {
                throw new NpgsqlException("Błąd serwera SQL - " + e.MessageText + " (kod " + e.SqlState + ")");
            }

            return await _context.Set<Timetable>().OrderBy(brigade => brigade.id).ToListAsync();
        }

        /// <summary>
        /// Wypisuje wpis ze szczegółami, określony przez jego ID.
        /// </summary>
        /// <param name="id" example="1">Identyfikator wpisu</param>
        /// <response code="200">Obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Timetable), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Timetable>> GetTimetable(int id)
        {
            Timetable timetable;

            try
            {
                timetable = await _context.brigades_timetable.FindAsync(id);
            }
            catch (PostgresException e)
            {
                throw new NpgsqlException("Błąd serwera SQL - " + e.MessageText + " (kod " + e.SqlState + ")");
            }

            if (timetable == null)
                return NotFound("Nie znaleziono");

            return timetable;
        }

        /// <summary>
        /// Aktualizuje szczegóły wpisu, określonego przez jego ID.
        /// </summary>
        /// <param name="id" example="1">Identyfikator wpisu</param>
        /// <param name="update">Parametry, jakie mają zostać zaktualizowane (w formie obiektu JSON). Wystarczy podać tylko nowe wartości - pozostałe zostaną skopiowane.</param>
        /// <response code="200">Aktualizacja pomyślna, zaktualizowany obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Timetable), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutTimetable(int id, Timetable update)
        {
            Timetable timetable;

            try
            {
                timetable = await _context.brigades_timetable.FindAsync(id);
                _context.Entry(timetable).State = EntityState.Detached;
            }
            catch (PostgresException e)
            {
                throw new NpgsqlException("Błąd serwera SQL - " + e.MessageText + " (kod " + e.SqlState + ")");
            }

            if (timetable != null)
            {
                update.id = id;
                foreach (PropertyInfo pi in typeof(Timetable).GetProperties())
                {
                    if ((pi.GetValue(update) != pi.GetValue(timetable))
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
                timetable = await _context.brigades_timetable.FindAsync(id);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException("Błąd podczas aktualizacji bazy danych - " + e.Message);
            }

            return Ok(timetable);
        }

        /// <summary>
        /// Dodaje nowy wpis do spisu (bazy danych).
        /// </summary>
        /// <param name="timetable">Parametry (w formie obiektu JSON). Wszystkie muszą być wypełnione.</param>
        /// <response code="201">Pomyślnie stworzono, nowy obiekt JSON</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpPost]
        [ProducesResponseType(typeof(Timetable), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Timetable>> PostTimetable(Timetable timetable)
        {
            try
            {
                _context.brigades_timetable.Any();
            }
            catch (PostgresException e)
            {
                throw new NpgsqlException("Błąd serwera SQL - " + e.MessageText + " (kod " + e.SqlState + ")");
            }

            _context.brigades_timetable.Add(timetable);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException("Błąd podczas aktualizacji bazy danych - " + e.Message);
            }

            return CreatedAtAction("GetTimetable", new { timetable.id }, timetable);
        }

        /// <summary>
        /// Usuwa wpis, określony przez jego ID, ze spisu (bazy danych).
        /// </summary>
        /// <param name="id" example="1">Identyfikator wpisu</param>
        /// <response code="200">Operacja wykonana pomyślnie, usunięty obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Timetable), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Timetable>> DeleteTimetable(int id)
        {
            Timetable timetable;

            try
            {
                timetable = await _context.brigades_timetable.FindAsync(id);
            }
            catch (PostgresException e)
            {
                throw new NpgsqlException("Błąd serwera SQL - " + e.MessageText + " (kod " + e.SqlState + ")");
            }

            if (timetable == null)
                return NotFound("Nie znaleziono");

            _context.brigades_timetable.Remove(timetable);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException("Błąd podczas aktualizacji bazy danych - " + e.Message);
            }

            return timetable;
        }
    }
}
