using System.ComponentModel.DataAnnotations;

namespace PoliziaMunicipaleApp.Models
{
    public class Trasgressore
    {
        public int Id { get; set; }

        [Required]
        public string Cognome { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Indirizzo { get; set; }

        [Required]
        public string CodiceFiscale { get; set; }
    }
}
