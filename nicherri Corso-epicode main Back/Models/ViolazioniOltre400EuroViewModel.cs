﻿namespace PoliziaMunicipaleApp.Models
{
    public class ViolazioniOltre400EuroViewModel
    {
        public int TrasgressoreId { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataViolazione { get; set; }
        public decimal Importo { get; set; }
    }
}
