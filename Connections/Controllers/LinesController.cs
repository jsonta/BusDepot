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
    public class LinesController : ControllerBase
    {
        private readonly CnctnsContext _context;
        public LinesController(CnctnsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Wypisuje wszystkie linie ze szczegółami w formie listy.
        /// </summary>
        /// <response code="200">Lista obiektów JSON</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Line>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Line>>> GetLines()
        {
            try
            {
                _context.lines.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            return await _context.Set<Line>().OrderBy(line => line.id).ToListAsync();
        }

        /// <summary>
        /// Wypisuje linię ze szczegółami, określoną przez jej ID.
        /// </summary>
        /// <param name="id" example="901">Identyfikator linii</param>
        /// <response code="200">Obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Line), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Line>> GetLine(int? id)
        {
            Line line;

            try
            {
                line = await _context.lines.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (line == null)
                return NotFound("Nie znaleziono");

            return line;
        }

        /// <summary>
        /// Aktualizuje szczegóły linii, określonej przez jej ID.
        /// </summary>
        /// <param name="id" example="901">Identyfikator linii</param>
        /// <param name="update">Parametry, jakie mają zostać zaktualizowane (w formie obiektu JSON). Wystarczy podać tylko nowe wartości - pozostałe zostaną skopiowane.</param>
        /// <response code="200">Aktualizacja pomyślna, zaktualizowany obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Line), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutLine(int? id, Line update)
        {
            Line current;

            try
            {
                current = await _context.lines.FindAsync(id);
                _context.Entry(current).State = EntityState.Detached;
            }
            catch (PostgresException)
            {
                throw;
            }

            if (current != null)
            {
                foreach (PropertyInfo pi in typeof(Line).GetProperties())
                {
                    if ((pi.GetValue(update) != pi.GetValue(current)) && (pi.GetValue(update) != null))
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
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(update);
        }

        /// <summary>
        /// Dodaje nową linię do spisu linii (bazy danych).
        /// </summary>
        /// <param name="line">Parametry linii (w formie obiektu JSON). Wszystkie muszą być wypełnione, o ile nie zaznaczono inaczej.</param>
        /// <response code="201">Pomyślnie stworzono, nowy obiekt JSON</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpPost]
        [ProducesResponseType(typeof(Line), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Line>> PostLine(Line line)
        {
            try
            {
                _context.lines.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            _context.lines.Add(line);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLine", new { line.id }, line);
        }

        /// <summary>
        /// Usuwa linię, określoną przez jej ID, ze spisu linii (bazy danych).
        /// </summary>
        /// <param name="id" example="901">Identyfikator linii</param>
        /// <response code="200">Operacja wykonana pomyślnie, usunięty obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Line), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Line>> DeleteLine(int? id)
        {
            Line line;

            try
            {
                line = await _context.lines.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (line == null)
                return NotFound("Nie znaleziono");

            _context.lines.Remove(line);
            await _context.SaveChangesAsync();

            return line;
        }
    }
}
