using System;
using System.ComponentModel.DataAnnotations;

namespace Resources.Models
{
    public class Driver
    {
        /// <summary>
        /// Identyfikator kierowcy (generowany automatycznie).
        /// </summary>
        /// <example>1</example>
        public int? id { get; set; }

        /// <summary>
        /// Imię kierowcy.
        /// </summary>
        /// <example>Jan</example>
        [Required]
        public string fname { get; set; }

        /// <summary>
        /// Nazwisko kierowcy.
        /// </summary>
        /// <example>Kowalski</example>
        [Required]
        public string lname { get; set; }

        /// <summary>
        /// Numer PESEL kierowcy.
        /// </summary>
        /// <example>99123100000</example>
        [Required]
        public string pesel { get; set; }

        /// <summary>
        /// Data urodzenia kierowcy (YYYY-MM-DD). Baza danych przechowuje w tej kolumnie TYLKO datę.
        /// </summary>
        /// <example>1999-12-31</example>
        [Required]
        public string bday_date { get; set; }

        /// <summary>
        /// Numer telefonu (komórkowy lub stacjonarny z nr kierunkowym) kierowcy.
        /// </summary>
        /// <example>123456789</example>
        [Required]
        public string phone { get; set; }

        /// <summary>
        /// Adres e-mail kierowcy (opcjonalnie).
        /// </summary>
        /// <example>nazwa@domena.pl</example>
        public string email { get; set; }

        /// <summary>
        /// Adres zamieszkania kierowcy - nazwa ulicy.
        /// </summary>
        /// <example>Stacyjna</example>
        [Required]
        public string street { get; set; }

        /// <summary>
        /// Adres zamieszkania kierowcy - numer budynku.
        /// </summary>
        /// <example>1</example>
        [Required]
        public int? building { get; set; }

        /// <summary>
        /// Adres zamieszkania kierowcy - numer mieszkania/lokalu (opcjonalnie).
        /// </summary>
        /// <example>2</example>
        public int? apartment { get; set; }

        /// <summary>
        /// Adres zamieszkania kierowcy - miasto.
        /// </summary>
        /// <example>Warszawa</example>
        [Required]
        public string city { get; set; }

        /// <summary>
        /// Adres zamieszkania kierowcy - kod pocztowy
        /// </summary>
        /// <example>01-234</example>
        [Required]
        public string zip { get; set; }

        /// <summary>
        /// Status zajętości kierowcy, tzn. czy można go wysłać do obsługi jakiejś brygady, czy nie.
        /// </summary>
        /// <example>false</example>
        [Required]
        public bool in_service { get; set; }
    }
}
