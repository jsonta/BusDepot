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

        /// <summary>
        /// Wypisuje wszystkie relacje ze szczegółami w formie listy.
        /// </summary>
        /// <response code="200">Lista obiektów JSON</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Relation>), 200)]
        [ProducesResponseType(500)]
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

        /// <summary>
        /// Wypisuje relację ze szczegółami, określoną przez jej ID.
        /// </summary>
        /// <param name="id" example="901-R1">Identyfikator relacji</param>
        /// <response code="200">Obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Relation), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
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
                return NotFound("Nie znaleziono");

            return relation;
        }

        /// <summary>
        /// Aktualizuje szczegóły relacji, określonej przez jej ID.
        /// </summary>
        /// <param name="id" example="901-R1">Identyfikator relacji</param>
        /// <param name="update">Parametry, jakie mają zostać zaktualizowane (w formie obiektu JSON). Wystarczy podać tylko nowe wartości - pozostałe zostaną skopiowane.</param>
        /// <response code="200">Aktualizacja pomyślna, zaktualizowany obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Relation), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
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
                update.id = id;
                foreach (PropertyInfo pi in typeof(Relation).GetProperties())
                {
                    if ((pi.GetValue(update) != pi.GetValue(current))
                        && (pi.GetValue(update) != null)
                        && (!pi.Name.Equals("id")))
                        _context.Entry(update).Property(pi.Name).IsModified = true;
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

        /// <summary>
        /// Dodaje nową relację do spisu relacji (bazy danych).
        /// </summary>
        /// <param name="relation">Parametry relacji (w formie obiektu JSON). Wszystkie muszą być wypełnione.</param>
        /// <response code="201">Pomyślnie stworzono, nowy obiekt JSON</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpPost]
        [ProducesResponseType(typeof(Relation), 201)]
        [ProducesResponseType(500)]
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

        /// <summary>
        /// Usuwa relację, określoną przez jej ID, ze spisu relacji (bazy danych).
        /// </summary>
        /// <param name="id" example="901-R1">Identyfikator relacji</param>
        /// <response code="200">Operacja wykonana pomyślnie, usunięty obiekt JSON</response>
        /// <response code="404">Nie znaleziono</response>
        /// <response code="500">Błąd serwera SQL</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Brigade), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
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
                return NotFound("Nie znaleziono");

            _context.relations.Remove(relation);
            await _context.SaveChangesAsync();

            return relation;
        }
    }
}
