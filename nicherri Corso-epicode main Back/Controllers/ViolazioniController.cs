using Microsoft.AspNetCore.Mvc;
using PoliziaMunicipaleApp.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PoliziaMunicipaleApp.Controllers
{
    public class ViolazioniController : Controller
    {
        private readonly SqlConnection _connection;

        public ViolazioniController(SqlConnection connection)
        {
            _connection = connection;
        }

        public IActionResult Index()
        {
            var violazioni = new List<Violazione>();

            string query = "SELECT * FROM Violazioni";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                _connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        violazioni.Add(new Violazione
                        {
                            Id = reader.GetInt32(0),
                            Descrizione = reader.GetString(1),
                            PuntiDecurtati = reader.GetInt32(2),
                            Importo = reader.GetDecimal(3)
                        });
                    }
                }
                _connection.Close();
            }

            return View(violazioni);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Violazione violazione)
        {
            string query = "INSERT INTO Violazioni (Descrizione, PuntiDecurtati, Importo) VALUES (@Descrizione, @PuntiDecurtati, @Importo)";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@Descrizione", violazione.Descrizione);
                command.Parameters.AddWithValue("@PuntiDecurtati", violazione.PuntiDecurtati);
                command.Parameters.AddWithValue("@Importo", violazione.Importo);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }

            return RedirectToAction("Index");
        }
    }
}
