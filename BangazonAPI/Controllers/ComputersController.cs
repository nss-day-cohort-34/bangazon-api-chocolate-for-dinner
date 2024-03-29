﻿using BangazonAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace BangazonAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ComputersController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ComputersController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }


   
        [HttpGet]
        public async Task<IActionResult> GetAllComputers(string active)
        {

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (active == "false")
                    {
                        string commandText = $"SELECT Id, PurchaseDate, Make, Manufacturer, DecomissionDate FROM Computer WHERE DecomissionDate IS NOT NULL";

                        cmd.CommandText = commandText;

                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        List<Computer> computers = new List<Computer>();
                        Computer computer = null;


                        while (reader.Read())
                        {
                            computer = new Computer
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                                Make = reader.GetString(reader.GetOrdinal("Make")),
                                Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                                DecomissionDate = reader.GetDateTime(reader.GetOrdinal("DecomissionDate"))

                            };


                            computers.Add(computer);
                        }


                        reader.Close();

                        return Ok(computers);
                    }
                    else
                    {
                        string commandText = $"SELECT Id, PurchaseDate, Make, Manufacturer FROM Computer WHERE DecomissionDate IS NULL";

                        cmd.CommandText = commandText;

                        SqlDataReader reader = cmd.ExecuteReader();
                        List<Computer> computers = new List<Computer>();
                        Computer computer = null;


                        while (reader.Read())
                        {
                            computer = new Computer
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                                Make = reader.GetString(reader.GetOrdinal("Make")),
                                Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer"))
                            };


                            computers.Add(computer);
                        }


                        reader.Close();

                        return Ok(computers);
                    }
                }
            }
        }



        [HttpGet("{id}", Name = "Computer")]
        public async Task<IActionResult> GetSingleComputer([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"SELECT Id, PurchaseDate, Make, Manufacturer, DecomissionDate FROM Computer WHERE id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Computer computerToDisplay = null;

                    while (reader.Read())
                    {

                        {
                            computerToDisplay = new Computer
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                                Make = reader.GetString(reader.GetOrdinal("Make")),
                                Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer"))

                            };

                           

                            if (!reader.IsDBNull(reader.GetOrdinal("DecomissionDate")))
                            {
                                computerToDisplay.DecomissionDate = reader.GetDateTime(reader.GetOrdinal("DecomissionDate"));
                            }
                            else
                            {
                                computerToDisplay.DecomissionDate = DateTime.MinValue;
                            }
                        };
                    };


                    reader.Close();

                    return Ok(computerToDisplay);
                }
            }
        }

        //  POST: create a computer
        [HttpPost]
        public async Task<IActionResult> Computer([FromBody] Computer computer)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = $@"INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer)
                                                    OUTPUT INSERTED.Id
                                                    VALUES (@PurchaseDate, Null, @Make, @Manufacturer)";
                    DateTime currentDate = DateTime.Now;
                    cmd.Parameters.Add(new SqlParameter("@PurchaseDate", currentDate));
                    cmd.Parameters.Add(new SqlParameter("@Make", computer.Make));
                    cmd.Parameters.Add(new SqlParameter("@Manufacturer", computer.Manufacturer));
                   

                    int newId = (int) await cmd.ExecuteScalarAsync();
                    computer.Id = newId;
                    computer.DecomissionDate = DateTime.MinValue;

                    return CreatedAtRoute("Computer", new { id = newId }, computer);
                }
            }
        }

        // PUT: Code for editing a computer
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComputer([FromRoute] int id, [FromBody] Computer computer)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Computer
                                                        SET 
                                                        DecomissionDate = Null,
                                                        PurchaseDate = @PurchaseDate,
                                                        Make=@Make,
                                                        Manufacturer = @Manufacturer
                                                        WHERE id = @id";

           
                        cmd.Parameters.Add(new SqlParameter("@Make", computer.Make));
                        cmd.Parameters.Add(new SqlParameter("@PurchaseDate", computer.PurchaseDate));
                        cmd.Parameters.Add(new SqlParameter("@Manufacturer", computer.Manufacturer));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!ComputerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Computer([FromRoute] int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {

                        cmd.CommandText = @"UPDATE Computer
                                            SET DecomissionDate = @currentDate
                                            WHERE id = @id";

                        //Set the current date to today
                        DateTime currentDate = DateTime.Now;

                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        cmd.Parameters.Add(new SqlParameter("@currentDate", currentDate));

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!ComputerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        private bool ComputerExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Make
                                    FROM Computer
                                    WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }


    }
}
