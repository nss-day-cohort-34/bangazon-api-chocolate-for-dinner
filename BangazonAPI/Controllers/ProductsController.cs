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
//    public class ProductsController : ControllerBase
//    {
//        private readonly IConfiguration _config;

//        public ProductsController(IConfiguration config)
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

//        // GET api/products
//        [HttpGet]
//        public async Task<IActionResult> Get()
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = "Write your SQL statement here to get all products";
//                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

//                    List<Product> products = new List<Product>();
//                    while (reader.Read())
//                    {
//                        Product product = new Product
//                        {
//                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
//                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
//                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
//                            // You might have more columns
//                        };

//                        products.Add(product);
//                    }

//                    reader.Close();

//                    return Ok(products);
//                }
//            }
//        }

//        // GET api/products/5
//        [HttpGet("{id}")]
//        public async Task<IActionResult> Get(int id)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = "Write your SQL statement here to get a single product";
//                    cmd.Parameters.Add(new SqlParameter("@id", id));
//                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

//                    Product product = null;
//                    if (reader.Read())
//                    {
//                        product = new Product
//                        {
//                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
//                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
//                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
//                            // You might have more columns
//                        };
//                    }

//                    reader.Close();

//                    return Ok(product);
//                }
//            }
//        }

//        // POST api/products
//        [HttpPost]
//        public async Task<IActionResult> Post([FromBody] Product product)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    // More string interpolation
//                    cmd.CommandText = @"
//                        INSERT INTO Product ()
//                        OUTPUT INSERTED.Id
//                        VALUES ()
//                    ";
//                    cmd.Parameters.Add(new SqlParameter("@firstName", product.FirstName));

//                    product.Id = (int) await cmd.ExecuteScalarAsync();

//                    return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
//                }
//            }
//        }

//        // PUT api/products/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Put(int id, [FromBody] Product product)
//        {
//            try
//            {
//                using (SqlConnection conn = Connection)
//                {
//                    conn.Open();
//                    using (SqlCommand cmd = conn.CreateCommand())
//                    {
//                        cmd.CommandText = @"
//                            UPDATE Product
//                            SET FirstName = @firstName
//                            -- Set the remaining columns here
//                            WHERE Id = @id
//                        ";
//                        cmd.Parameters.Add(new SqlParameter("@id", product.Id));
//                        cmd.Parameters.Add(new SqlParameter("@firstName", product.FirstName));

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
//                if (!ProductExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//        }

//        // DELETE api/products/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            throw new NotImplementedException("This method isn't implemented...yet.");
//        }

//        private bool ProductExists(int id)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = "SELECT Id FROM Product WHERE Id = @id";
//                    cmd.Parameters.Add(new SqlParameter("@id", id));

//                    SqlDataReader reader = cmd.ExecuteReader();

//                    return reader.Read();
//                }
//            }
//        }
//    }
//}
