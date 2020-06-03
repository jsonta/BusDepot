using System.ComponentModel.DataAnnotations;

namespace Resources.Models
{
    public class Bus
    {
        /// <summary>
        /// Numer boczny autobusu, będący jednocześnie jego ID.
        /// </summary>
        /// <example>1000</example>
        [Required]
        public int? id { get; set; }

        /// <summary>
        /// Marka (producent) autobusu.
        /// </summary>
        /// <example>Solaris</example>
        [Required]
        public string brand { get; set; }

        /// <summary>
        /// Model autobusu.
        /// </summary>
        /// <example>Urbino 12</example>
        [Required]
        public string model { get; set; }

        /// <summary>
        /// Liczba osi autobusu.
        /// </summary>
        /// <example>1</example>
        [Required]
        public int? axes { get; set; }
        
        /// <summary>
        /// Numer tablic rejestracyjnych autobusu.
        /// </summary>
        /// <example>AB 12345</example>
        [Required]
        public string vrn { get; set; }

        /// <summary>
        /// Rok produkcji autobusu.
        /// </summary>
        /// <example>1999</example>
        [Required]
        public int? yr_prod { get; set; }

        /// <summary>
        /// Rok zakupu autobusu.
        /// </summary>
        /// <example>2000</example>
        [Required]
        public int? yr_prch { get; set; }

        /// <summary>
        /// Ilość miejsc stojących.
        /// </summary>
        /// <example>60</example>
        [Required]
        public int? stand_plcs { get; set; }

        /// <summary>
        /// Ilość miejsc siedzących.
        /// </summary>
        /// <example>28</example>
        [Required]
        public int? sit_plcs { get; set; }

        /// <summary>
        /// Klasa pojemnościowa (MIDI, MAXI, MEGA) autobusu.
        /// </summary>
        /// <example>Maxi</example>
        [Required]
        public string cpct_class { get; set; }
        
        /// <summary>
        /// Norma emisji spalin (Euro) autobusu.
        /// </summary>
        /// <example>Euro2</example>
        [Required]
        public string emsn_norm { get; set; }

        /// <summary>
        /// Status zajętości autobusu, tzn. czy można go wysłać do obsługi jakiejś brygady, czy nie.
        /// </summary>
        /// <example>false</example>
        [Required]
        public bool in_service { get; set; }
    }
}
