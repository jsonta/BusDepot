using System.ComponentModel.DataAnnotations;

namespace Connections.Models
{
    public class Terminus
    {
        /// <summary>
        /// Identyfikator przystanku początkowego/końcowego.
        /// </summary>
        /// <example>1</example>
        [Required]
        public int? id { get; set; }

        /// <summary>
        /// Nazwa przystanku początkowego/końcowego.
        /// </summary>
        /// <example>Wiśniowa</example>
        [Required]
        public string name { get; set; }
    }
}
