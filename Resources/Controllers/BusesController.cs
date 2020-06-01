using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Resources.Models;

namespace Resources.Controllers
{
    [Route("resources/[controller]")]
    [ApiController]
    public class BusesController : ControllerBase
    {
        private readonly RsrcsContext _context;
        public BusesController(RsrcsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Wypisuje wszystkie autobusy ze szczegółami w formie listy.
        /// </summary>
        /// <response code="200">Lista obiektów JSON</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Bus>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Bus>>> GetBuses()
        {
            try
            {
                _context.buses.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            return await _context.Set<Bus>().OrderBy(bus => bus.id).ToListAsync();
        }

        /// <summary>
        /// Wypisuje autobus ze szczegółami, określony przez jego ID.
        /// </summary>
        /// <param name="id" example="1000">Numer boczny autobusu</param>
        /// <response code="200">Obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Bus), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Bus>> GetBus(int? id)
        {
            Bus bus;

            try
            {
                bus = await _context.buses.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (bus == null)
                return NotFound("Nie znaleziono");

            return bus;
        }

        /// <summary>
        /// Aktualizuje szczegóły autobusu, określonego przez jego ID.
        /// </summary>
        /// <param name="id" example="1000">Numer boczny autobusu</param>
        /// <param name="update">Parametry, jakie mają zostać zaktualizowane (w formie obiektu JSON). Wystarczy podać tylko nowe wartości - pozostałe zostaną skopiowane.</param>
        /// <response code="200">Aktualizacja pomyślna, zaktualizowany obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Bus), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutBus(int? id, Bus update)
        {
            Bus current;

            try
            {
                current = await _context.buses.FindAsync(id);
                _context.Entry(current).State = EntityState.Detached;
            }
            catch (PostgresException)
            {
                throw;
            }

            if (current != null)
            {
                foreach (PropertyInfo pi in typeof(Bus).GetProperties())
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
        /// Dodaje nowy autobus do spisu autobusów (bazy danych).
        /// </summary>
        /// <param name="bus">Parametry autobusu (w formie obiektu JSON). Wszystkie muszą być wypełnione.</param>
        /// <response code="201">Pomyślnie stworzono, nowy obiekt JSON</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpPost]
        [ProducesResponseType(typeof(Bus), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Bus>> PostBus(Bus bus)
        {
            try
            {
                _context.buses.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            _context.buses.Add(bus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBus", new { bus.id }, bus);
        }

        /// <summary>
        /// Usuwa autobus, określony przez jego ID, ze spisu autobusów (bazy danych).
        /// </summary>
        /// <param name="id" example="1000">Numer boczny autobusu</param>
        /// <response code="200">Operacja wykonana pomyślnie, usunięty obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Bus), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Bus>> DeleteBus(int? id)
        {
            Bus bus;

            try
            {
                bus = await _context.buses.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (bus == null)
                return NotFound("Nie znaleziono");

            _context.buses.Remove(bus);
            await _context.SaveChangesAsync();

            return bus;
        }
    }
}
