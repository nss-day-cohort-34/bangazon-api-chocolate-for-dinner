using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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
    public class OrdersController : ControllerBase
    {
        private readonly IConfiguration _config;

        public OrdersController(IConfiguration config)
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

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string _include, string completed)
        {
            string sqlCommandTxt;
            if (_include == "products")
            {
                sqlCommandTxt = @"SELECT o.Id, o.CustomerId, o.PaymentTypeId, p.CustomerId AS PaymentCustomerId, p.Description, p.Id AS ProductId, p.Price, p.ProductTypeId, p.Quantity, p.Title
                                       FROM [Order] o
                                       LEFT JOIN OrderProduct op ON op.OrderId = o.Id
                                       LEFT JOIN Product p ON p.Id = op.ProductId";
            }
            else if (_include == "customers")
            {
                sqlCommandTxt = @"SELECT o.Id, o.CustomerId, o.PaymentTypeId, 
                                       c.Id AS CustomerId, c.FirstName, c.LastName
                                       FROM [Order] o
                                       LEFT JOIN Customer c ON c.Id = o.CustomerId";
            }
            else if (completed == "true")
            {
                sqlCommandTxt = @"SELECT Id, CustomerId, PaymentTypeId
                   FROM[Order]
                   WHERE PaymentTypeId != 0";
            }
            else if (completed == "false")
            {
                sqlCommandTxt = @"SELECT Id, CustomerId, PaymentTypeId
                   FROM[Order]
                   WHERE PaymentTypeId IS NULL";
            }
            else
            {
                sqlCommandTxt = @"SELECT Id, CustomerId, PaymentTypeId
                                        FROM [Order]";
            }
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sqlCommandTxt;
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    List<Order> orders = new List<Order>();
                    while (reader.Read())
                    {
                        Order order = new Order
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                        };
                        if (!reader.IsDBNull(reader.GetOrdinal("PaymentTypeId")))
                        {
                            order.PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId"));
                        }
                   
                        if (_include == "products")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                            {
                                Product product = new Product
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("PaymentCustomerId")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                                };
                                order.Products = new List<Product>();
                                if (orders.Any(o => o.Id == order.Id))
                                {
                                    Order existingOrder = orders.Find(o => o.Id == order.Id);
                                    existingOrder.Products.Add(product);
                                }
                                else
                                {
                                    order.Products.Add(product);
                                    orders.Add(order);
                                }
                            }
                        }
                        else if (_include == "customers")
                        {
                            Customer customer = new Customer
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName"))
                            };
                            order.Customer = customer;
                            orders.Add(order);
                        }
                        else
                        {
                            orders.Add(order);
                        }
                    }
                    reader.Close();
                    return Ok(orders);
                }
            }
        }
        // GET api/orders/1
        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<IActionResult> Get([FromRoute] int id, [FromQuery] string _include, string completed)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (!OrderExists(id))
                    {
                        return new StatusCodeResult(StatusCodes.Status404NotFound);
                    }




                    if (_include == "customers")
                    
                        {
                        cmd.CommandText = @"SELECT o.Id, o.CustomerId, o.PaymentTypeId, 
                                       c.Id AS CustomerId, c.FirstName, c.LastName
                                       FROM[Order] o
                                      LEFT JOIN Customer c ON c.Id = o.CustomerId

                                       WHERE o.id = @id";
                    }

                    else if (_include == "products")
                    {
                        cmd.CommandText = @"SELECT o.Id, o.CustomerId, o.PaymentTypeId, p.CustomerId AS PaymentCustomerId, p.Description, p.Id AS ProductId, p.Price, p.ProductTypeId, p.Quantity, p.Title
                                       FROM [Order] o
                                       LEFT JOIN OrderProduct op ON op.OrderId = o.Id
                                       LEFT JOIN Product p ON p.Id = op.ProductId WHERE o.id = @id";
                    }

                    

                    else
                    {
                        cmd.CommandText = @"SELECT Id, CustomerId, PaymentTypeId
                                        FROM [Order] o WHERE o.id = @id";
                    }
                    if (!OrderExists(id))
                    {
                        return new StatusCodeResult(StatusCodes.Status404NotFound);
                    }


                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Order order = null;

                    while (reader.Read())
                    {


                        if (order == null)
                        {
                            order = new Order
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("PaymentTypeId")))
                            {
                                order.PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId"));
                            }

                          
                        }
                        if (_include == "products")
                        {

                            if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                            {
                                order.Products.Add(
                                    new Product
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("PaymentCustomerId")),
                                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                        Title = reader.GetString(reader.GetOrdinal("Title")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                                    }
                               );
                            }
                        }
                        if (_include == "customers")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("CustomerId")))
                            {
                                order.Customer =
                                new Customer
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName"))
                                };
                            }

                        }
                    }

                    reader.Close();

                    return Ok(order);
                }
            }
        }

        // POST api/orders
      
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO [Order] (CustomerId, PaymentTypeId)
                        OUTPUT INSERTED.Id
                        VALUES (@customerId, null)
                    ";
                    cmd.Parameters.Add(new SqlParameter("@customerId", order.CustomerId));
                  

                    order.Id = (int)await cmd.ExecuteScalarAsync();

                    return CreatedAtRoute("GetOrder", new { id = order.Id }, order);
                }
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Order order)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            UPDATE [Order]
                            SET CustomerId = @customerId, 
                                PaymentTypeId = @paymentTypeId
                            WHERE Id = @id
                        ";
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        cmd.Parameters.Add(new SqlParameter("@customerId", order.CustomerId));
                        cmd.Parameters.Add(new SqlParameter("@paymentTypeId", order.PaymentTypeId));

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
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM OrderProduct WHERE OrderId = @id
                                            DELETE FROM [Order] WHERE Id = @id AND PaymentTypeId IS NULL
                                            ";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("You cannot delete this order because it has already been completed");
                    }
                }
            }
            catch (Exception)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool OrderExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, CustomerId, PaymentTypeId
                        FROM [Order]
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }
    }
}