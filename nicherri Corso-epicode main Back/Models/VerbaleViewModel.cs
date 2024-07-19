namespace PoliziaMunicipaleApp.Models
{
    public class VerbaleViewModel
    {
        public int TrasgressoreId { get; set; }
        public string NomeTrasgressore { get; set; }
        public string CognomeTrasgressore { get; set; }
        public string NumeroVerbale { get; set; }
        public string DescrizioneViolazione { get; set; }
        public DateTime DataViolazione { get; set; }
        public int PuntiDecurtati { get; set; }
        public decimal Importo { get; set; }
    }
}
