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
    public class BrigadesController : ControllerBase
    {
        private readonly CnctnsContext _context;
        public BrigadesController(CnctnsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Wypisuje wszystkie brygady ze szczegółami w formie listy.
        /// </summary>
        /// <response code="200">Lista obiektów JSON</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Brigade>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Brigade>>> GetBrigades()
        {
            try
            {
                _context.brigades.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            return await _context.Set<Brigade>().OrderBy(brigade => brigade.id).ToListAsync();
        }

        /// <summary>
        /// Wypisuje brygadę ze szczegółami, określoną przez jej ID.
        /// </summary>
        /// <param name="id" example="901-01">Identyfikator brygady</param>
        /// <response code="200">Obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Brigade), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Brigade>> GetBrigade(string id)
        {
            Brigade brigade;

            try
            {
                brigade = await _context.brigades.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (brigade == null)
                return NotFound("Nie znaleziono");

            return brigade;
        }

        /// <summary>
        /// Aktualizuje szczegóły brygady, określonej przez jej ID.
        /// </summary>
        /// <param name="id" example="901-01">Identyfikator brygady</param>
        /// <param name="update">Parametry, jakie mają zostać zaktualizowane (w formie obiektu JSON). Wystarczy podać tylko nowe wartości - pozostałe zostaną skopiowane.</param>
        /// <response code="200">Aktualizacja pomyślna, zaktualizowany obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Brigade), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutBrigade(string id, Brigade update)
        {
            Brigade current;

            try
            {
                current = await _context.brigades.FindAsync(id);
                _context.Entry(current).State = EntityState.Detached;
            }
            catch (PostgresException)
            {
                throw;
            }

            if (current != null)
            {
                foreach (PropertyInfo pi in typeof(Brigade).GetProperties())
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
        /// Dodaje nową brygadę do spisu brygad (bazy danych).
        /// </summary>
        /// <param name="brigade">Parametry brygady (w formie obiektu JSON). Wszystkie muszą być wypełnione.</param>
        /// <response code="201">Pomyślnie stworzono, nowy obiekt JSON</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpPost]
        [ProducesResponseType(typeof(Brigade), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Brigade>> PostBrigade(Brigade brigade)
        {
            try
            {
                _context.brigades.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            _context.brigades.Add(brigade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBrigade", new { brigade.id }, brigade);
        }

        /// <summary>
        /// Usuwa brygadę, określoną przez jej ID, ze spisu brygad (bazy danych).
        /// </summary>
        /// <param name="id" example="901-01">Identyfikator brygady</param>
        /// <response code="200">Operacja wykonana pomyślnie, usunięty obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Brigade), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Brigade>> DeleteBrigade(string id)
        {
            Brigade brigade;

            try
            {
                brigade = await _context.brigades.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (brigade == null)
                return NotFound("Nie znaleziono");

            _context.brigades.Remove(brigade);
            await _context.SaveChangesAsync();

            return brigade;
        }
    }
}
