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
    public class CustomersController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CustomersController(IConfiguration config)
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

        // GET api/customers
        [HttpGet]
        public async Task<IActionResult> Get(string include, string q)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include == "products")
                    {
                        cmd.CommandText = @"SELECT c.Id AS CustomerId, c.FirstName, c.LastName, c.CreationDate, c.LastActiveDate,
                                                                       p.Id AS ProductId, p.Title, p.Price, p.ProductTypeId, p.Description, p.Quantity,
                                                                       pt.Name AS ProductType
                                                            FROM Customer c
                                                                       LEFT JOIN Product p ON c.Id = p.CustomerId
                                                                       INNER JOIN ProductType pt ON pt.Id = p.ProductTypeId";
                    }

                    else if (include == "payments")
                    {
                        cmd.CommandText = @"SELECT c.Id AS CustomerId, c.FirstName, c.LastName, c.CreationDate, c.LastActiveDate,
                                                                        pyt.Id AS PaymentTypeId, pyt.AcctNumber, pyt.Name AS PaymentTypeName
                                                        FROM Customer c
                                                                        LEFT JOIN PaymentType pyt ON c.Id = pyt.CustomerId";
                    }

                    else if (q != null)
                    {
                        cmd.CommandText = @"SELECT Id AS CustomerId, FirstName, LastName, CreationDate, LastActiveDate
                                                                FROM Customer
                                                                WHERE FirstName LIKE @q OR LastName LIKE @q";
                        cmd.Parameters.Add(new SqlParameter("@q", $"%{q}%"));
                    }

                    else
                    {
                        cmd.CommandText = @"SELECT Id AS CustomerId, FirstName, LastName, CreationDate, LastActiveDate
                                                                FROM Customer";
                    }

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    List<Customer> customers = new List<Customer>();
                    while (reader.Read())
                    {
                        Customer newCustomer = new Customer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                            LastActiveDate = reader.GetDateTime(reader.GetOrdinal("LastActiveDate"))
                        };

                        if (include == "products")
                        {
                            Product newProduct = new Product
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                            };

                            // Check to see if the newly-created customer has already been added to the customer list
                            if (!customers.Exists(customerInList => customerInList.Id == newCustomer.Id))
                            {
                                customers.Add(newCustomer);
                                newCustomer.Products.Add(newProduct);
                            }

                            else
                            {
                                Customer existingCustomer = customers.Find(customer => customer.Id == newCustomer.Id);
                                existingCustomer.Products.Add(newProduct);
                            }
                        }

                        else if (include == "payments")
                        {
                            PaymentType newPayment = new PaymentType
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
                                AcctNumber = reader.GetString(reader.GetOrdinal("AcctNumber")),
                                Name = reader.GetString(reader.GetOrdinal("PaymentTypeName"))
                            };

                            // Check to see if the newly-created customer has already been added to the customer list
                            if (!customers.Exists(customerInList => customerInList.Id == newCustomer.Id))
                            {
                                customers.Add(newCustomer);
                                newCustomer.PaymentTypes.Add(newPayment);
                            }

                            else
                            {
                                Customer existingCustomer = customers.Find(customer => customer.Id == newCustomer.Id);
                                existingCustomer.PaymentTypes.Add(newPayment);
                            }
                        }

                        // Else condition below applies for both a query paramter of "q" and customers without parameters
                        else
                        {
                            customers.Add(newCustomer);
                        }

                    }

                    reader.Close();

                    return Ok(customers);
                }
            }
        }

        // GET api/customers/5
        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> Get(int id, string include, string q)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include == "products")
                    {
                        cmd.CommandText = @"SELECT c.Id AS CustomerId, c.FirstName, c.LastName, c.CreationDate, c.LastActiveDate,
                                                                       p.Id AS ProductId, p.Title, p.Price, p.ProductTypeId, p.Description, p.Quantity,
                                                                       pt.Name AS ProductType
                                                            FROM Customer c
                                                                       LEFT JOIN Product p ON c.Id = p.CustomerId
                                                                       INNER JOIN ProductType pt ON pt.Id = p.ProductTypeId
                                                            WHERE c.Id = @Id";
                    }

                    else if (include == "payments")
                    {
                        cmd.CommandText = @"SELECT c.Id AS CustomerId, c.FirstName, c.LastName, c.CreationDate, c.LastActiveDate,
                                                                        pyt.Id AS PaymentTypeId, pyt.AcctNumber, pyt.Name AS PaymentTypeName
                                                        FROM Customer c
                                                                        LEFT JOIN PaymentType pyt ON c.Id = pyt.CustomerId
                                                            WHERE c.Id = @Id";
                    }

                    else if (q != null)
                    {
                        cmd.CommandText = @"SELECT Id AS CustomerId, FirstName, LastName, CreationDate, LastActiveDate
                                                                FROM Customer
                                                                WHERE Id = @Id AND (FirstName LIKE @q OR LastName LIKE @q)";
                        cmd.Parameters.Add(new SqlParameter("@q", $"%{q}%"));
                    }

                    else
                    {
                        cmd.CommandText = @"SELECT Id AS CustomerId, FirstName, LastName, CreationDate, LastActiveDate
                                                                FROM Customer
                                                                WHERE Id = @Id";
                    }

                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Customer selectedCustomer = null;

                    while (reader.Read())
                    {
                        if (selectedCustomer == null)
                        {
                            selectedCustomer = new Customer
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                                LastActiveDate = reader.GetDateTime(reader.GetOrdinal("LastActiveDate"))
                            };
                        }

                        if (include == "products")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                            {
                                Product newProduct = new Product
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                                };
                                selectedCustomer.Products.Add(newProduct);
                            }
                        }

                        else if (include == "payments")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("PaymentTypeId")))
                            {
                                PaymentType newPayment = new PaymentType
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
                                    AcctNumber = reader.GetString(reader.GetOrdinal("AcctNumber")),
                                    Name = reader.GetString(reader.GetOrdinal("PaymentTypeName"))
                                };
                                selectedCustomer.PaymentTypes.Add(newPayment);
                            }
                        }
                    }
                    reader.Close();
                    return Ok(selectedCustomer);
                }
            }
        }

        // POST api/customers
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // More string interpolation
                    cmd.CommandText = @"
                        INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate)
                        OUTPUT INSERTED.Id
                        VALUES (@FirstName, @LastName, @CreationDate, @LastActiveDate)
                    ";
                    cmd.Parameters.Add(new SqlParameter("@FirstName", customer.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", customer.LastName));
                    cmd.Parameters.Add(new SqlParameter("@CreationDate", customer.CreationDate));
                    cmd.Parameters.Add(new SqlParameter("@LastActiveDate", customer.LastActiveDate));

                    customer.Id = (int)await cmd.ExecuteScalarAsync();

                    return CreatedAtRoute("GetCustomer", new { id = customer.Id }, customer);
                }
            }
        }

        // PUT api/customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Customer customer)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            UPDATE Customer
                            SET FirstName = @FirstName, LastName = @LastName, CreationDate = @CreationDate, LastActiveDate = @LastActiveDate
                            WHERE Id = @Id";
                        cmd.Parameters.Add(new SqlParameter("@FirstName", customer.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@LastName", customer.LastName));
                        cmd.Parameters.Add(new SqlParameter("@CreationDate", customer.CreationDate));
                        cmd.Parameters.Add(new SqlParameter("@LastActiveDate", customer.LastActiveDate));
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
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        //// DELETE api/customers/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    throw new NotImplementedException("This method isn't implemented...yet.");
        //}

        private bool CustomerExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM Customer WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    return reader.Read();
                }
            }
        }
    }
}
