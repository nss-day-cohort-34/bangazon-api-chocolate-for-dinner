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
    public class PaymentTypesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PaymentTypesController(IConfiguration config)
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

        // GET api/paymentTypes
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT pt.Id, 
                                               pt.AcctNumber, 
                                               pt.Name, 
                                               pt.CustomerId, 
                                               c.FirstName AS CustomerFirstName, 
                                               c.LastName AS CustomerLastName
                                          FROM PaymentType pt
	                                 LEFT JOIN Customer c
	                                        ON pt.CustomerId = c.Id
                                         WHERE IsDeleted = 0";

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    List<PaymentType> paymentTypes = new List<PaymentType>();

                    while (reader.Read())
                    {
                        PaymentType paymentType = new PaymentType
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            AcctNumber = reader.GetString(reader.GetOrdinal("AcctNumber")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"))
                        };

                        paymentTypes.Add(paymentType);
                    }

                    reader.Close();

                    return Ok(paymentTypes);
                }
            }
        }

        // GET api/paymentTypes/9
        [HttpGet("{id}", Name = "GetPaymentType")]
        public async Task<IActionResult> Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT pt.Id, 
                                               pt.AcctNumber, 
                                               pt.Name, 
                                               pt.CustomerId, 
                                               c.FirstName AS CustomerFirstName, 
                                               c.LastName AS CustomerLastName
                                          FROM PaymentType pt
                                               LEFT JOIN Customer c
                                            ON pt.CustomerId = c.Id
                                         WHERE pt.Id = @Id AND IsDeleted = 0";

                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    PaymentType paymentType = null;

                    if (reader.Read())
                    {
                        paymentType = new PaymentType
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            AcctNumber = reader.GetString(reader.GetOrdinal("AcctNumber")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"))
                        };
                    }
                    reader.Close();
                    return Ok(paymentType);
                }

            }
        }

        // POST api/paymentTypes
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentType paymentType)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO PaymentType (AcctNumber, Name, CustomerId)
                        OUTPUT INSERTED.Id
                        VALUES (@AcctNumber, @Name, @CustomerId);
                    ";
                    cmd.Parameters.Add(new SqlParameter("@AcctNumber", paymentType.AcctNumber));
                    cmd.Parameters.Add(new SqlParameter("@Name", paymentType.Name));
                    cmd.Parameters.Add(new SqlParameter("@CustomerId", paymentType.CustomerId));

                    paymentType.Id = (int)await cmd.ExecuteScalarAsync();

                    return CreatedAtRoute("GetPaymentType", new { id = paymentType.Id }, paymentType);
                }
            }
        }

        // PUT api/paymentTypes/9
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] PaymentType paymentType)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            UPDATE PaymentType
                               SET AcctNumber = @AcctNumber, Name = @Name, CustomerId = @CustomerId
                             WHERE Id = @Id;";
                        cmd.Parameters.Add(new SqlParameter("@AcctNumber", paymentType.AcctNumber));
                        cmd.Parameters.Add(new SqlParameter("@Name", paymentType.Name));
                        cmd.Parameters.Add(new SqlParameter("@CustomerId", paymentType.CustomerId));
                        cmd.Parameters.Add(new SqlParameter("@Id", id));

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
                if (!PaymentTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE api/paymentTypes/9
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE PaymentType 
                                           SET IsDeleted = 1
                                         WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@Id", id));

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        return new StatusCodeResult(StatusCodes.Status204NoContent);
                    }
                    throw new Exception("No rows affected");
                }
            }
        }

        // Checks to see if PaymentTypeExists
        private bool PaymentTypeExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM PaymentType WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    return reader.Read();
                }
            }
        }
    }
}

