using Microsoft.AspNetCore.Mvc;
using PoliziaMunicipaleApp.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PoliziaMunicipaleApp.Controllers
{
    public class ReportsController : Controller
    {
        private readonly SqlConnection _connection;

        public ReportsController(SqlConnection connection)
        {
            _connection = connection;
        }

        public IActionResult TotaleVerbaliPerTrasgressore()
        {
            var report = new List<TotaleVerbaliPerTrasgressoreViewModel>();

            string query = @"
                SELECT 
                    Trasgressori.Id AS TrasgressoreId, 
                    Trasgressori.Nome, 
                    Trasgressori.Cognome, 
                    COUNT(Verbali.Id) AS TotaleVerbali 
                FROM Verbali
                JOIN Trasgressori ON Verbali.TrasgressoreId = Trasgressori.Id
                GROUP BY Trasgressori.Id, Trasgressori.Nome, Trasgressori.Cognome";

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                _connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        report.Add(new TotaleVerbaliPerTrasgressoreViewModel
                        {
                            TrasgressoreId = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            Cognome = reader.GetString(2),
                            TotaleVerbali = reader.GetInt32(3)
                        });
                    }
                }
                _connection.Close();
            }

            return View(report);
        }

        public IActionResult TotalePuntiDecurtatiPerTrasgressore()
        {
            var report = new List<dynamic>();

            string query = "SELECT TrasgressoreId, SUM(Violazioni.PuntiDecurtati) AS TotalePunti FROM Verbali JOIN Violazioni ON Verbali.ViolazioneId = Violazioni.Id GROUP BY TrasgressoreId";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                _connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        report.Add(new
                        {
                            TrasgressoreId = reader.GetInt32(0),
                            TotalePunti = reader.GetInt32(1)
                        });
                    }
                }
                _connection.Close();
            }

            return View(report);
        }

        public IActionResult ViolazioniOltre10Punti()
        {
            var report = new List<ViolazioniOltre10PuntiViewModel>();

            string query = @"
                SELECT 
                    Trasgressori.Id AS TrasgressoreId, 
                    Trasgressori.Cognome, 
                    Trasgressori.Nome, 
                    Violazioni.Descrizione, 
                    Verbali.DataViolazione, 
                    Verbali.Importo 
                FROM Verbali
                JOIN Trasgressori ON Verbali.TrasgressoreId = Trasgressori.Id
                JOIN Violazioni ON Verbali.ViolazioneId = Violazioni.Id
                WHERE Violazioni.PuntiDecurtati > 10";

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                _connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        report.Add(new ViolazioniOltre10PuntiViewModel
                        {
                            TrasgressoreId = reader.GetInt32(0),
                            Cognome = reader.GetString(1),
                            Nome = reader.GetString(2),
                            Descrizione = reader.GetString(3),
                            DataViolazione = reader.GetDateTime(4),
                            Importo = reader.GetDecimal(5)
                        });
                    }
                }
                _connection.Close();
            }

            return View(report);
        }

        public IActionResult ViolazioniOltre400Euro()
        {
            var report = new List<ViolazioniOltre400EuroViewModel>();

            string query = @"
                SELECT 
                    Trasgressori.Id AS TrasgressoreId, 
                    Trasgressori.Cognome, 
                    Trasgressori.Nome, 
                    Violazioni.Descrizione, 
                    Verbali.DataViolazione, 
                    Verbali.Importo 
                FROM Verbali
                JOIN Trasgressori ON Verbali.TrasgressoreId = Trasgressori.Id
                JOIN Violazioni ON Verbali.ViolazioneId = Violazioni.Id
                WHERE Violazioni.Importo > 400";

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                _connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        report.Add(new ViolazioniOltre400EuroViewModel
                        {
                            TrasgressoreId = reader.GetInt32(0),
                            Cognome = reader.GetString(1),
                            Nome = reader.GetString(2),
                            Descrizione = reader.GetString(3),
                            DataViolazione = reader.GetDateTime(4),
                            Importo = reader.GetDecimal(5)
                        });
                    }
                }
                _connection.Close();
            }

            return View(report);
        }
    }
}
