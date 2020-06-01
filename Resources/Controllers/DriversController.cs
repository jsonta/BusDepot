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
    public class DriversController : ControllerBase
    {
        private readonly RsrcsContext _context;
        public DriversController(RsrcsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Wypisuje wszystkich kierowców z ich danymi w formie listy.
        /// </summary>
        /// <response code="200">Lista obiektów JSON</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Driver>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
        {
            try
            {
                _context.drivers.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            return await _context.Set<Driver>().OrderBy(driver => driver.id).ToListAsync();
        }

        /// <summary>
        /// Wypisuje kierowcę ze szczegółami, określonego przez jego nr PESEL (ID).
        /// </summary>
        /// <param name="id" example="99123100000">Numer PESEL kierowcy</param>
        /// <response code="200">Obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Driver), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Driver>> GetDriver(long? id)
        {
            Driver driver;

            try
            {
                driver = await _context.drivers.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (driver == null)
                return NotFound("Nie znaleziono");

            return driver;
        }

        /// <summary>
        /// Aktualizuje dane kierowcy, określonego przez jego nr PESEL (ID).
        /// </summary>
        /// <param name="id" example="99123100000">Numer PESEL kierowcy</param>
        /// <param name="update">Parametry, jakie mają zostać zaktualizowane (w formie obiektu JSON). Wystarczy podać tylko nowe wartości - pozostałe zostaną skopiowane.</param>
        /// <response code="200">Aktualizacja pomyślna, zaktualizowany obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Driver), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutDriver(int? id, Driver update)
        {
            Driver current;

            try
            {
                current = await _context.drivers.FindAsync(id);
                _context.Entry(current).State = EntityState.Detached;
            }
            catch (PostgresException)
            {
                throw;
            }

            if (current != null)
            {
                foreach (PropertyInfo pi in typeof(Driver).GetProperties())
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
        /// Dodaje nowego kierowcę do spisu kierowców (bazy danych).
        /// </summary>
        /// <param name="driver">Dane osobowe kierowcy (w formie obiektu JSON). Wszystkie muszą być wypełnione, o ile nie zaznaczono inaczej.</param>
        /// <response code="201">Pomyślnie stworzono, nowy obiekt JSON</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpPost]
        [ProducesResponseType(typeof(Driver), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Driver>> PostDriver(Driver driver)
        {
            try
            {
                _context.drivers.Any();
            }
            catch (PostgresException)
            {
                throw;
            }

            _context.drivers.Add(driver);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriver", new { driver.id }, driver);
        }

        /// <summary>
        /// Usuwa kierowcę, określony przez jego nr PESEL (ID), ze spisu kierowców (bazy danych).
        /// </summary>
        /// <param name="id" example="99123100000">Numer PESEL kierowcy</param>
        /// <response code="200">Operacja wykonana pomyślnie, usunięty obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Driver), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Driver>> DeleteDriver(long? id)
        {
            Driver driver;

            try
            {
                driver = await _context.drivers.FindAsync(id);
            }
            catch (PostgresException)
            {
                throw;
            }

            if (driver == null)
                return NotFound("Nie znaleziono");

            _context.drivers.Remove(driver);
            await _context.SaveChangesAsync();

            return driver;
        }
    }
}
