using Microsoft.AspNetCore.Mvc;
using PoliziaMunicipaleApp.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PoliziaMunicipaleApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly SqlConnection _connection;

        public HomeController(SqlConnection connection)
        {
            _connection = connection;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string searchTerm, string searchType)
        {
            var multe = new List<MulteViewModel>();

            string query = "";
            if (searchType == "NumeroVerbale")
            {
                query = "SELECT Violazioni.Descrizione, Verbali.DataViolazione, Violazioni.PuntiDecurtati, Verbali.Importo " +
                        "FROM Verbali " +
                        "JOIN Violazioni ON Verbali.ViolazioneId = Violazioni.Id " +
                        "WHERE Verbali.NumeroVerbale = @searchTerm";
            }
            else if (searchType == "CodiceFiscale")
            {
                query = "SELECT Violazioni.Descrizione, Verbali.DataViolazione, Violazioni.PuntiDecurtati, Verbali.Importo " +
                        "FROM Verbali " +
                        "JOIN Violazioni ON Verbali.ViolazioneId = Violazioni.Id " +
                        "JOIN Trasgressori ON Verbali.TrasgressoreId = Trasgressori.Id " +
                        "WHERE Trasgressori.CodiceFiscale = @searchTerm";
            }

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@searchTerm", searchTerm);
                _connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        multe.Add(new MulteViewModel
                        {
                            Descrizione = reader.GetString(0),
                            DataViolazione = reader.GetDateTime(1),
                            PuntiDecurtati = reader.GetInt32(2),
                            Importo = reader.GetDecimal(3)
                        });
                    }
                }
                _connection.Close();
            }

            return View("Index", multe);
        }
    }
}
