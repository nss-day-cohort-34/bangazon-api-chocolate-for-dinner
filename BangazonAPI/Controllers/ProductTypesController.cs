﻿//using System;
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
//    public class ProductTypesController : ControllerBase
//    {
//        private readonly IConfiguration _config;

//        public ProductTypesController(IConfiguration config)
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

//        // GET api/productTypes
//        [HttpGet]
//        public async Task<IActionResult> Get()
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = "Write your SQL statement here to get all productTypes";
//                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

//                    List<ProductType> productTypes = new List<ProductType>();
//                    while (reader.Read())
//                    {
//                        ProductType productType = new ProductType
//                        {
//                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
//                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
//                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
//                            // You might have more columns
//                        };

//                        productTypes.Add(productType);
//                    }

//                    reader.Close();

//                    return Ok(productTypes);
//                }
//            }
//        }

//        // GET api/productTypes/5
//        [HttpGet("{id}")]
//        public async Task<IActionResult> Get(int id)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = "Write your SQL statement here to get a single productType";
//                    cmd.Parameters.Add(new SqlParameter("@id", id));
//                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

//                    ProductType productType = null;
//                    if (reader.Read())
//                    {
//                        productType = new ProductType
//                        {
//                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
//                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
//                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
//                            // You might have more columns
//                        };
//                    }

//                    reader.Close();

//                    return Ok(productType);
//                }
//            }
//        }

//        // POST api/productTypes
//        [HttpPost]
//        public async Task<IActionResult> Post([FromBody] ProductType productType)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    // More string interpolation
//                    cmd.CommandText = @"
//                        INSERT INTO ProductType ()
//                        OUTPUT INSERTED.Id
//                        VALUES ()
//                    ";
//                    cmd.Parameters.Add(new SqlParameter("@firstName", productType.FirstName));

//                    productType.Id = (int) await cmd.ExecuteScalarAsync();

//                    return CreatedAtRoute("GetProductType", new { id = productType.Id }, productType);
//                }
//            }
//        }

//        // PUT api/productTypes/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Put(int id, [FromBody] ProductType productType)
//        {
//            try
//            {
//                using (SqlConnection conn = Connection)
//                {
//                    conn.Open();
//                    using (SqlCommand cmd = conn.CreateCommand())
//                    {
//                        cmd.CommandText = @"
//                            UPDATE ProductType
//                            SET FirstName = @firstName
//                            -- Set the remaining columns here
//                            WHERE Id = @id
//                        ";
//                        cmd.Parameters.Add(new SqlParameter("@id", productType.Id));
//                        cmd.Parameters.Add(new SqlParameter("@firstName", productType.FirstName));

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
//                if (!ProductTypeExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//        }

//        // DELETE api/productTypes/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            throw new NotImplementedException("This method isn't implemented...yet.");
//        }

//        private bool ProductTypeExists(int id)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = "SELECT Id FROM ProductType WHERE Id = @id";
//                    cmd.Parameters.Add(new SqlParameter("@id", id));

//                    SqlDataReader reader = cmd.ExecuteReader();

//                    return reader.Read();
//                }
//            }
//        }
//    }
//}
