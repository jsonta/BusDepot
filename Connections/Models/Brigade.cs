using System.ComponentModel.DataAnnotations;

namespace Connections.Models
{
    public class Brigade
    {
        /// <summary>
        /// Identyfikator brygady (linia-brygada).
        /// </summary>
        /// <example>901-01</example>
        [Required]
        public string id { get; set; }

        /// <summary>
        /// Numer linii, której dotyczy dana brygada.
        /// </summary>
        /// <example>901</example>
        [Required]
        public int? line { get; set; }

        /// <summary>
        /// Status zajętości brygady, tzn. czy może zostać obsadzona, czy nie.
        /// </summary>
        /// <example>false</example>
        [Required]
        public bool in_service { get; set; }
    }
}
