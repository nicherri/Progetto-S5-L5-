using Microsoft.AspNetCore.Mvc;
using PoliziaMunicipaleApp.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PoliziaMunicipaleApp.Controllers
{
    public class TrasgressoriController : Controller
    {
        private readonly SqlConnection _connection;

        public TrasgressoriController(SqlConnection connection)
        {
            _connection = connection;
        }

        public IActionResult Index()
        {
            var trasgressori = new List<Trasgressore>();

            string query = "SELECT * FROM Trasgressori";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                _connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        trasgressori.Add(new Trasgressore
                        {
                            Id = reader.GetInt32(0),
                            Cognome = reader.GetString(1),
                            Nome = reader.GetString(2),
                            Indirizzo = reader.GetString(3),
                            CodiceFiscale = reader.GetString(4)
                        });
                    }
                }
                _connection.Close();
            }

            return View(trasgressori);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Trasgressore trasgressore)
        {
            string query = "INSERT INTO Trasgressori (Cognome, Nome, Indirizzo, CodiceFiscale) VALUES (@Cognome, @Nome, @Indirizzo, @CodiceFiscale)";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@Cognome", trasgressore.Cognome);
                command.Parameters.AddWithValue("@Nome", trasgressore.Nome);
                command.Parameters.AddWithValue("@Indirizzo", trasgressore.Indirizzo);
                command.Parameters.AddWithValue("@CodiceFiscale", trasgressore.CodiceFiscale);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }

            return RedirectToAction("Index");
        }
    }
}
