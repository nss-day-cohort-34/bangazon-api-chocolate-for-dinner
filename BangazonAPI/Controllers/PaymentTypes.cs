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
//    public class PaymentTypesController : ControllerBase
//    {
//        private readonly IConfiguration _config;

//        public PaymentTypesController(IConfiguration config)
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

//        // GET api/paymentTypes
//        [HttpGet]
//        public async Task<IActionResult> Get()
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = "Write your SQL statement here to get all paymentTypes";
//                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

//                    List<PaymentType> paymentTypes = new List<PaymentType>();
//                    while (reader.Read())
//                    {
//                        PaymentType paymentType = new PaymentType
//                        {
//                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
//                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
//                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
//                            // You might have more columns
//                        };

//                        paymentTypes.Add(paymentType);
//                    }

//                    reader.Close();

//                    return Ok(paymentTypes);
//                }
//            }
//        }

//        // GET api/paymentTypes/5
//        [HttpGet("{id}")]
//        public async Task<IActionResult> Get(int id)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = "Write your SQL statement here to get a single paymentType";
//                    cmd.Parameters.Add(new SqlParameter("@id", id));
//                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

//                    PaymentType paymentType = null;
//                    if (reader.Read())
//                    {
//                        paymentType = new PaymentType
//                        {
//                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
//                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
//                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
//                            // You might have more columns
//                        };
//                    }

//                    reader.Close();

//                    return Ok(paymentType);
//                }
//            }
//        }

//        // POST api/paymentTypes
//        [HttpPost]
//        public async Task<IActionResult> Post([FromBody] PaymentType paymentType)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    // More string interpolation
//                    cmd.CommandText = @"
//                        INSERT INTO PaymentType ()
//                        OUTPUT INSERTED.Id
//                        VALUES ()
//                    ";
//                    cmd.Parameters.Add(new SqlParameter("@firstName", paymentType.FirstName));

//                    paymentType.Id = (int) await cmd.ExecuteScalarAsync();

//                    return CreatedAtRoute("GetPaymentType", new { id = paymentType.Id }, paymentType);
//                }
//            }
//        }

//        // PUT api/paymentTypes/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Put(int id, [FromBody] PaymentType paymentType)
//        {
//            try
//            {
//                using (SqlConnection conn = Connection)
//                {
//                    conn.Open();
//                    using (SqlCommand cmd = conn.CreateCommand())
//                    {
//                        cmd.CommandText = @"
//                            UPDATE PaymentType
//                            SET FirstName = @firstName
//                            -- Set the remaining columns here
//                            WHERE Id = @id
//                        ";
//                        cmd.Parameters.Add(new SqlParameter("@id", paymentType.Id));
//                        cmd.Parameters.Add(new SqlParameter("@firstName", paymentType.FirstName));

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
//                if (!PaymentTypeExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//        }

//        // DELETE api/paymentTypes/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            throw new NotImplementedException("This method isn't implemented...yet.");
//        }

//        private bool PaymentTypeExists(int id)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = "SELECT Id FROM PaymentType WHERE Id = @id";
//                    cmd.Parameters.Add(new SqlParameter("@id", id));

//                    SqlDataReader reader = cmd.ExecuteReader();

//                    return reader.Read();
//                }
//            }
//        }
//    }
//}
