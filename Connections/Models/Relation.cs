using System.ComponentModel.DataAnnotations;

namespace Connections.Models
{
    public class Relation
    {
        /// <summary>
        /// Numer linii, której dotyczy dana relacja.
        /// </summary>
        /// <example>901</example>
        [Required]
        public int? line { get; set; }

        /// <summary>
        /// Identyfikator relacji (linia-Rnr)
        /// </summary>
        /// <example>901-R1</example>
        [Required]
        public string id { get; set; }

        /// <summary>
        /// Identyfikator przystanku początkowego.
        /// </summary>
        /// <example>1</example>
        [Required]
        public int? start { get; set; }

        /// <summary>
        /// Identyfikator przystanku końcowego.
        /// </summary>
        /// <example>2</example>
        [Required]
        public int? end { get; set; }
    }
}
