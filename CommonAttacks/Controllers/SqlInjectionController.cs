using CommonAttacks.Data;
using CommonAttacks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CommonAttacks.Controllers
{
    public class SqlInjectionController : Controller
    {
        //https://localhost:7006/SqlInjection/VulnerableGetCarById?id=1
        //https://localhost:7006/SqlInjection/VulnerableGetCarById?id=0;DELETE%20FROM%20Car%20%20WHERE%20Id%20=%201%20OR%201%20=%201
        public SqlInjectionController(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        public IActionResult VulnerableGetCarById()
        {
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM Car WHERE Id = {Request.Query["Id"]}", connection);

                using (var reader = command.ExecuteReader())
                {
                    Car foundCar = null;
                    if (reader.Read())
                    {
                        foundCar = new Car
                        {
                            Id = (int)reader["Id"],
                            Make = reader["Make"].ToString(),
                            Model = reader["Model"].ToString(),
                            Color = reader["Color"].ToString(),
                            Year = (int)reader["Year"]
                        };
                    }
                    else
                    {
                        return NotFound();
                    }
                    return View("GetCarById", foundCar);
                }
            }

        }
    }
}
