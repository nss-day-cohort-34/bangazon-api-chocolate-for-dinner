//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Threading.Tasks;
//using BangazonAPI.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;

//namespace BangazonAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ComputersController : ControllerBase
//    {
//        private readonly IConfiguration _config;

//        public ComputersController(IConfiguration config)
//        {
//            _config = config;
//        }

//        private SqlConnection Connection
//        {
//            get
//            {
//                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
//            }
//        }

//        // GET api/computers
//        [HttpGet]
//        public async Task<IActionResult> Get()
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = "Write your SQL statement here to get all computers";
//                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

//                    List<Computer> computers = new List<Computer>();
//                    while (reader.Read())
//                    {
//                        Computer computer = new Computer
//                        {
//                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
//                            // You might have more columns
//                        };

//                        computers.Add(computer);
//                    }

//                    reader.Close();

//                    return Ok(computers);
//                }
//            }
//        }

//        // GET api/computers/5
//        [HttpGet("{id}")]
//        public async Task<IActionResult> Get(int id)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = "Write your SQL statement here to get a single computer";
//                    cmd.Parameters.Add(new SqlParameter("@id", id));
//                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

//                    Computer computer = null;
//                    if (reader.Read())
//                    {
//                        computer = new Computer
//                        {
//                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
//                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
//                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
//                            // You might have more columns
//                        };
//                    }

//                    reader.Close();

//                    return Ok(computer);
//                }
//            }
//        }

//        // POST api/computers
//        [HttpPost]
//        public async Task<IActionResult> Post([FromBody] Computer computer)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    // More string interpolation
//                    cmd.CommandText = @"
//                        INSERT INTO Computer ()
//                        OUTPUT INSERTED.Id
//                        VALUES ()
//                    ";
//                    cmd.Parameters.Add(new SqlParameter("@firstName", computer.FirstName));

//                    computer.Id = (int) await cmd.ExecuteScalarAsync();

//                    return CreatedAtRoute("GetComputer", new { id = computer.Id }, computer);
//                }
//            }
//        }

//        // PUT api/computers/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Put(int id, [FromBody] Computer computer)
//        {
//            try
//            {
//                using (SqlConnection conn = Connection)
//                {
//                    conn.Open();
//                    using (SqlCommand cmd = conn.CreateCommand())
//                    {
//                        cmd.CommandText = @"
//                            UPDATE Computer
//                            SET FirstName = @firstName
//                            -- Set the remaining columns here
//                            WHERE Id = @id
//                        ";
//                        cmd.Parameters.Add(new SqlParameter("@id", computer.Id));
//                        cmd.Parameters.Add(new SqlParameter("@firstName", computer.FirstName));

//                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

//                        if (rowsAffected > 0)
//                        {
//                            return new StatusCodeResult(StatusCodes.Status204NoContent);
//                        }

//                        throw new Exception("No rows affected");
//                    }
//                }
//            }
//            catch (Exception)
//            {
//                if (!ComputerExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//        }

//        // DELETE api/computers/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            throw new NotImplementedException("This method isn't implemented...yet.");
//        }

//        private bool ComputerExists(int id)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = "SELECT Id FROM Computer WHERE Id = @id";
//                    cmd.Parameters.Add(new SqlParameter("@id", id));

//                    SqlDataReader reader = cmd.ExecuteReader();

//                    return reader.Read();
//                }
//            }
//        }
//    }
//}
