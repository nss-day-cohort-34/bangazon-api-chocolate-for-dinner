using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class trainingProgramsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public trainingProgramsController(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET api/trainingPrograms
        [HttpGet]
        public async Task<IActionResult> Get(string completed, string q)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (completed == "false")
                    {
                        cmd.CommandText = @"SELECT Id, Name, StartDate, EndDate, MaxAttendees
                                              FROM TrainingProgram
                                             WHERE (StartDate >= SYSDATETIME());";
                    }

                    else
                    {
                        cmd.CommandText = @"SELECT Id, Name, StartDate, EndDate, MaxAttendees
                                              FROM TrainingProgram;";
                    }

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    List<TrainingProgram> TrainingPrograms = new List<TrainingProgram>();
                    while (reader.Read())
                    {
                        TrainingProgram newtrainingProgram = new TrainingProgram
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                        };
                        TrainingPrograms.Add(newtrainingProgram);
                    }

                    reader.Close();

                    return Ok(TrainingPrograms);
                }
            }
        }

        //// GET api/trainingPrograms/5
        //[HttpGet("{id}", Name = "GettrainingProgram")]
        //public async Task<IActionResult> Get(int id, string _include, string q)
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            if (_include == "products")
        //            {
        //                cmd.CommandText = @"SELECT c.Id AS trainingProgramId, c.FirstName, c.LastName, c.CreationDate, c.LastActiveDate,
        //                                                               p.Id AS ProductId, p.Title, p.Price, p.ProductTypeId, p.Description, p.Quantity,
        //                                                               pt.Name AS ProductType
        //                                                    FROM trainingProgram c
        //                                                               LEFT JOIN Product p ON c.Id = p.trainingProgramId
        //                                                               INNER JOIN ProductType pt ON pt.Id = p.ProductTypeId
        //                                                    WHERE c.Id = @Id";
        //            }

        //            else if (_include == "payments")
        //            {
        //                cmd.CommandText = @"SELECT c.Id AS trainingProgramId, c.FirstName, c.LastName, c.CreationDate, c.LastActiveDate,
        //                                                                pyt.Id AS PaymentTypeId, pyt.AcctNumber, pyt.Name AS PaymentTypeName
        //                                                FROM trainingProgram c
        //                                                                LEFT JOIN PaymentType pyt ON c.Id = pyt.trainingProgramId
        //                                                    WHERE c.Id = @Id";
        //            }

        //            else if (q != null)
        //            {
        //                cmd.CommandText = @"SELECT Id AS trainingProgramId, FirstName, LastName, CreationDate, LastActiveDate
        //                                                        FROM trainingProgram
        //                                                        WHERE Id = @Id AND (FirstName LIKE @q OR LastName LIKE @q)";
        //                cmd.Parameters.Add(new SqlParameter("@q", $"%{q}%"));
        //            }

        //            else
        //            {
        //                cmd.CommandText = @"SELECT Id AS trainingProgramId, FirstName, LastName, CreationDate, LastActiveDate
        //                                                        FROM trainingProgram
        //                                                        WHERE Id = @Id";
        //            }

        //            cmd.Parameters.Add(new SqlParameter("@Id", id));
        //            SqlDataReader reader = await cmd.ExecuteReaderAsync();

        //            trainingProgram selectedtrainingProgram = null;

        //            while (reader.Read())
        //            {
        //                if (selectedtrainingProgram == null)
        //                {
        //                    selectedtrainingProgram = new trainingProgram
        //                    {
        //                        Id = reader.GetInt32(reader.GetOrdinal("trainingProgramId")),
        //                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
        //                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
        //                        CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
        //                        LastActiveDate = reader.GetDateTime(reader.GetOrdinal("LastActiveDate"))
        //                    };
        //                }

        //                if (_include == "products")
        //                {
        //                    if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
        //                    {
        //                        Product newProduct = new Product
        //                        {
        //                            Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
        //                            ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
        //                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
        //                            Title = reader.GetString(reader.GetOrdinal("Title")),
        //                            Description = reader.GetString(reader.GetOrdinal("Description")),
        //                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
        //                        };
        //                        selectedtrainingProgram.Products.Add(newProduct);
        //                    }
        //                }

        //                else if (_include == "payments")
        //                {
        //                    if (!reader.IsDBNull(reader.GetOrdinal("PaymentTypeId")))
        //                    {
        //                        PaymentType newPayment = new PaymentType
        //                        {
        //                            Id = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
        //                            AcctNumber = reader.GetString(reader.GetOrdinal("AcctNumber")),
        //                            Name = reader.GetString(reader.GetOrdinal("PaymentTypeName"))
        //                        };
        //                        selectedtrainingProgram.PaymentTypes.Add(newPayment);
        //                    }
        //                }
        //            }
        //            reader.Close();
        //            return Ok(selectedtrainingProgram);
        //        }
        //    }
        //}

        //// POST api/trainingPrograms
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] trainingProgram trainingProgram)
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            // More string interpolation
        //            cmd.CommandText = @"
        //                INSERT INTO trainingProgram (FirstName, LastName, CreationDate, LastActiveDate)
        //                OUTPUT INSERTED.Id
        //                VALUES (@FirstName, @LastName, @CreationDate, @LastActiveDate)
        //            ";
        //            cmd.Parameters.Add(new SqlParameter("@FirstName", trainingProgram.FirstName));
        //            cmd.Parameters.Add(new SqlParameter("@LastName", trainingProgram.LastName));
        //            cmd.Parameters.Add(new SqlParameter("@CreationDate", trainingProgram.CreationDate));
        //            cmd.Parameters.Add(new SqlParameter("@LastActiveDate", trainingProgram.LastActiveDate));

        //            trainingProgram.Id = (int)await cmd.ExecuteScalarAsync();

        //            return CreatedAtRoute("GettrainingProgram", new { id = trainingProgram.Id }, trainingProgram);
        //        }
        //    }
        //}

        //// PUT api/trainingPrograms/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put([FromRoute] int id, [FromBody] trainingProgram trainingProgram)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = Connection)
        //        {
        //            conn.Open();
        //            using (SqlCommand cmd = conn.CreateCommand())
        //            {
        //                cmd.CommandText = @"
        //                    UPDATE trainingProgram
        //                    SET FirstName = @FirstName, LastName = @LastName, CreationDate = @CreationDate, LastActiveDate = @LastActiveDate
        //                    WHERE Id = @Id";
        //                cmd.Parameters.Add(new SqlParameter("@FirstName", trainingProgram.FirstName));
        //                cmd.Parameters.Add(new SqlParameter("@LastName", trainingProgram.LastName));
        //                cmd.Parameters.Add(new SqlParameter("@CreationDate", trainingProgram.CreationDate));
        //                cmd.Parameters.Add(new SqlParameter("@LastActiveDate", trainingProgram.LastActiveDate));
        //                cmd.Parameters.Add(new SqlParameter("@id", id));

        //                int rowsAffected = await cmd.ExecuteNonQueryAsync();

        //                if (rowsAffected > 0)
        //                {
        //                    return new StatusCodeResult(StatusCodes.Status204NoContent);
        //                }

        //                throw new Exception("No rows affected");
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        if (!trainingProgramExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}

        ////// DELETE api/trainingPrograms/5
        ////[HttpDelete("{id}")]
        ////public async Task<IActionResult> Delete(int id)
        ////{
        ////    throw new NotImplementedException("This method isn't implemented...yet.");
        ////}

        //private bool trainingProgramExists(int id)
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = "SELECT Id FROM trainingProgram WHERE Id = @id";
        //            cmd.Parameters.Add(new SqlParameter("@id", id));

        //            SqlDataReader reader = cmd.ExecuteReader();

        //            return reader.Read();
        //        }
        //    }
        //}
    }
}
