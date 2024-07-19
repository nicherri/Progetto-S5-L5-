using System;
using System.ComponentModel.DataAnnotations;

namespace PoliziaMunicipaleApp.Models
{
    public class Verbale
    {
        public int Id { get; set; }

        [Required]
        public string NumeroVerbale { get; set; }

        [Required]
        public int TrasgressoreId { get; set; }

        [Required]
        public int ViolazioneId { get; set; }

        [Required]
        public DateTime DataViolazione { get; set; }

        [Required]
        public decimal Importo { get; set; }
    }
}
