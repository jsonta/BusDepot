using System.ComponentModel.DataAnnotations;

namespace Connections.Models
{
    public class Terminus
    {
        /// <summary>
        /// Identyfikator przystanku początkowego/końcowego (generowany automatycznie).
        /// </summary>
        /// <example>1</example>
        public int? id { get; set; }

        /// <summary>
        /// Nazwa przystanku początkowego/końcowego.
        /// </summary>
        /// <example>Wiśniowa</example>
        [Required]
        public string name { get; set; }
    }
}
