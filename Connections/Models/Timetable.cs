using System.ComponentModel.DataAnnotations;

namespace Connections.Models
{
    public class Timetable
    {
        /// <summary>
        /// Numer pozycji wpisu rozkładu jazdy (generowany automatycznie).
        /// </summary>
        [Required]
        public int id { get; set; }

        /// <summary>
        /// Identyfikator brygady, której dotyczy dany wpis (linia-brygada).
        /// </summary>
        /// <example>901-01</example>
        [Required]
        public string brigade { get; set; }

        /// <summary>
        /// Identyfikator relacji, której dotyczy dany wpis (linia-Rnr).
        /// </summary>
        /// <example>901-R1</example>
        [Required]
        public string relation { get; set; }

        /// <summary>
        /// Identyfikator uwagi przypisanej do danego wpisu.
        /// </summary>
        /// <example>P</example>
        [Required]
        public char remarks { get; set; }

        /// <summary>
        /// Godzina odjazdu z przystanku początkowego (HH:mm).
        /// </summary>
        /// <example>07:00</example>
        [Required]
        public string dep_time { get; set; }

        /// <summary>
        /// Godzina przyjazdu na przystanek końcowy (HH:mm).
        /// </summary>
        /// <example>07:30</example>
        [Required]
        public string arr_time { get; set; }
    }
}
