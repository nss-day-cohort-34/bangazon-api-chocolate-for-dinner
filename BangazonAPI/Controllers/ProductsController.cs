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
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductsController(IConfiguration config)
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

        // GET api/products
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id AS ProductId, ProductTypeId, CustomerId, Price, Title, Description, Quantity
                                                                FROM Product";

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    List<Product> products = new List<Product>();
                    while (reader.Read())
                    {
                        Product newProduct = new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                            ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                        };

                        products.Add(newProduct);
                    }
                    reader.Close();

                    return Ok(products);
                }
            }
        }

        // GET api/products/5
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id AS ProductId, ProductTypeId, CustomerId, Price, Title, Description, Quantity
                                                                FROM Product
                                            WHERE id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Product product = null;
                    if (reader.Read())
                    {
                        product = new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                            ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                        };
                    }
                    reader.Close();

                    if (product == null)
                    {
                        return NotFound();
                    }

                    return Ok(product);
                }
            }

            //// POST api/products
            //[HttpPost]
            //public async Task<IActionResult> Post([FromBody] Product product)
            //{
            //    using (SqlConnection conn = Connection)
            //    {
            //        conn.Open();
            //        using (SqlCommand cmd = conn.CreateCommand())
            //        {
            //            // More string interpolation
            //            cmd.CommandText = @"
            //                INSERT INTO Product (FirstName, LastName, CreationDate, LastActiveDate)
            //                OUTPUT INSERTED.Id
            //                VALUES (@FirstName, @LastName, @CreationDate, @LastActiveDate)
            //            ";
            //            cmd.Parameters.Add(new SqlParameter("@FirstName", product.FirstName));
            //            cmd.Parameters.Add(new SqlParameter("@LastName", product.LastName));
            //            cmd.Parameters.Add(new SqlParameter("@CreationDate", product.CreationDate));
            //            cmd.Parameters.Add(new SqlParameter("@LastActiveDate", product.LastActiveDate));

            //            product.Id = (int)await cmd.ExecuteScalarAsync();

            //            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
            //        }
            //    }
            //}

            //// PUT api/products/5
            //[HttpPut("{id}")]
            //public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Product product)
            //{
            //    try
            //    {
            //        using (SqlConnection conn = Connection)
            //        {
            //            conn.Open();
            //            using (SqlCommand cmd = conn.CreateCommand())
            //            {
            //                cmd.CommandText = @"
            //                    UPDATE Product
            //                    SET FirstName = @FirstName, LastName = @LastName, CreationDate = @CreationDate, LastActiveDate = @LastActiveDate
            //                    WHERE Id = @Id";
            //                cmd.Parameters.Add(new SqlParameter("@FirstName", product.FirstName));
            //                cmd.Parameters.Add(new SqlParameter("@LastName", product.LastName));
            //                cmd.Parameters.Add(new SqlParameter("@CreationDate", product.CreationDate));
            //                cmd.Parameters.Add(new SqlParameter("@LastActiveDate", product.LastActiveDate));
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
            //        if (!ProductExists(id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //}

            ////// DELETE api/products/5
            ////[HttpDelete("{id}")]
            ////public async Task<IActionResult> Delete(int id)
            ////{
            ////    throw new NotImplementedException("This method isn't implemented...yet.");
            ////}

            //private bool ProductExists(int id)
            //{
            //    using (SqlConnection conn = Connection)
            //    {
            //        conn.Open();
            //        using (SqlCommand cmd = conn.CreateCommand())
            //        {
            //            cmd.CommandText = "SELECT Id FROM Product WHERE Id = @id";
            //            cmd.Parameters.Add(new SqlParameter("@id", id));

            //            SqlDataReader reader = cmd.ExecuteReader();

            //            return reader.Read();
            //        }
            //    }
            //}
        }
    }
}
