using System.ComponentModel.DataAnnotations;

namespace Connections.Models
{
    public class Line
    {
        /// <summary>
        /// Numer linii, będący jednocześnie jej ID.
        /// </summary>
        /// <example>901</example>
        [Required]
        public int? id { get; set; }

        /// <summary>
        /// Nazwa linii, jeśli nie jest ona numeryczna.
        /// </summary>
        /// <example>A</example>
        public string name { get; set; }
    }
}
