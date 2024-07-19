using Microsoft.AspNetCore.Mvc;
using PoliziaMunicipaleApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace PoliziaMunicipaleApp.Controllers
{
    public class VerbaliController : Controller
    {
        private readonly SqlConnection _connection;

        public VerbaliController(SqlConnection connection)
        {
            _connection = connection;
        }

        public IActionResult Index()
        {
            var verbali = new List<VerbaleViewModel>();

            string query = @"
                SELECT 
                    Verbali.Id, 
                    Verbali.NumeroVerbale, 
                    Trasgressori.Id AS TrasgressoreId,
                    Trasgressori.Nome AS NomeTrasgressore, 
                    Trasgressori.Cognome AS CognomeTrasgressore, 
                    Violazioni.Descrizione AS DescrizioneViolazione, 
                    Verbali.DataViolazione, 
                    Verbali.Importo 
                FROM Verbali
                JOIN Trasgressori ON Verbali.TrasgressoreId = Trasgressori.Id
                JOIN Violazioni ON Verbali.ViolazioneId = Violazioni.Id";

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                _connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        verbali.Add(new VerbaleViewModel
                        {
                            TrasgressoreId = reader.GetInt32(2),
                            NomeTrasgressore = reader.GetString(3),
                            CognomeTrasgressore = reader.GetString(4),
                            NumeroVerbale = reader.GetString(1),
                            DescrizioneViolazione = reader.GetString(5),
                            DataViolazione = reader.GetDateTime(6),
                            Importo = reader.GetDecimal(7)
                        });
                    }
                }
                _connection.Close();
            }

            return View(verbali);
        }

        public IActionResult Create()
        {
            ViewBag.Violazioni = GetViolazioni();
            ViewBag.Trasgressori = GetTrasgressori();
            return View(new Verbale { NumeroVerbale = GenerateUniqueVerbaleNumber() });
        }

        [HttpPost]
        public IActionResult Create(Verbale verbale)
        {
            if (ModelState.IsValid)
            {
                string query = "INSERT INTO Verbali (NumeroVerbale, TrasgressoreId, ViolazioneId, DataViolazione, Importo) VALUES (@NumeroVerbale, @TrasgressoreId, @ViolazioneId, @DataViolazione, @Importo)";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@NumeroVerbale", verbale.NumeroVerbale);
                    command.Parameters.AddWithValue("@TrasgressoreId", verbale.TrasgressoreId);
                    command.Parameters.AddWithValue("@ViolazioneId", verbale.ViolazioneId);
                    command.Parameters.AddWithValue("@DataViolazione", verbale.DataViolazione);
                    command.Parameters.AddWithValue("@Importo", verbale.Importo);

                    _connection.Open();
                    command.ExecuteNonQuery();
                    _connection.Close();
                }

                return RedirectToAction("Index");
            }

            ViewBag.Violazioni = GetViolazioni();
            ViewBag.Trasgressori = GetTrasgressori();
            return View(verbale);
        }

        private List<Violazione> GetViolazioni()
        {
            var violazioni = new List<Violazione>();

            string query = "SELECT Id, Descrizione FROM Violazioni";
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
                            Descrizione = reader.GetString(1)
                        });
                    }
                }
                _connection.Close();
            }

            return violazioni;
        }

        private List<Trasgressore> GetTrasgressori()
        {
            var trasgressori = new List<Trasgressore>();

            string query = "SELECT Id, Nome, Cognome FROM Trasgressori";
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
                            Nome = reader.GetString(1),
                            Cognome = reader.GetString(2)
                        });
                    }
                }
                _connection.Close();
            }

            return trasgressori;
        }

        private string GenerateUniqueVerbaleNumber()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
