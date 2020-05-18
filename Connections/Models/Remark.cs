using System.ComponentModel.DataAnnotations;

namespace Connections.Models
{
    public class Remark
    {
        /// <summary>
        /// Identyfikator uwagi (pojedynczy znak).
        /// </summary>
        /// <example>P</example>
        [Required]
        public char id { get; set; }

        /// <summary>
        /// Opis uwagi.
        /// </summary>
        /// <example>Przerwa na posiłek</example>
        [Required]
        public string name { get; set; }
    }
}
