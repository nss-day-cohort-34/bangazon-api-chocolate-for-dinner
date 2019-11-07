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
    public class DepartmentsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public DepartmentsController(IConfiguration config)
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

        // GET api/departments
        [HttpGet]
        public async Task<IActionResult> Get(string _include, string _filter, int _gt)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (_include == "employees")
                    {
                        cmd.CommandText = @"SELECT d.Id, d.Name, d.Budget, e.FirstName
                                                 , e.LastName, e.IsSuperVisor, e.Id AS EmployeeId
                                              FROM Department d 
                                         LEFT JOIN Employee e ON d.Id = e.DepartmentId";
                    }

                    else if (_filter == "budget")
                    {
                        cmd.CommandText = @"SELECT d.Id, d.Name, d.Budget
                                              FROM Department d 
                                              WHERE d.Budget > @_gt";
                        cmd.Parameters.Add(new SqlParameter("_gt", _gt));
                    }

                    else
                    {
                        cmd.CommandText = @"SELECT d.Id, d.Name, d.Budget
                                              FROM Department d";
                    }

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    List<Department> departments = new List<Department>();
                    while (reader.Read())
                    {
                        Department newDepartment = new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                        };

                        if (_include == "employees" && !reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                        {
                            Employee newEmployee = new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DepartmentId = reader.GetInt32(reader.GetOrdinal("Id")),
                                IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSuperVisor"))
                            };

                            // Check to see if the newly-created customer has already been added to the customer list
                            if (!departments.Exists(d => d.Id == newDepartment.Id))
                            {
                                departments.Add(newDepartment);
                                newDepartment.Employees.Add(newEmployee);
                            }

                            else
                            {
                                Department existingDepartment = departments.Find(d => d.Id == newDepartment.Id);
                                existingDepartment.Employees.Add(newEmployee);
                            }
                        }
                        else
                        {
                            departments.Add(newDepartment);
                        }

                    }

                    reader.Close();

                    return Ok(departments);
                }
            }
        }

        // GET api/departments/5
        [HttpGet("{id}", Name = "GetDepartment")]
        public async Task<IActionResult> Get(int id, string _include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (_include == "employees")
                    {
                        cmd.CommandText = @"SELECT d.Id, d.Name, d.Budget, e.FirstName
                                                 , e.LastName, e.IsSuperVisor, e.Id AS EmployeeId
                                              FROM Department d 
                                         LEFT JOIN Employee e ON d.Id = e.DepartmentId
                                             WHERE d.Id = @id";
                    }

                    else
                    {
                        cmd.CommandText = @"SELECT d.Id, d.Name, d.Budget
                                              FROM Department d
                                             WHERE d.Id = @id";
                    }

                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Department department = new Department();
                    while (reader.Read())
                    {
                        Department newDepartment = new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                        };

                        if (_include == "employees" && !reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                        {
                            Employee newEmployee = new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DepartmentId = reader.GetInt32(reader.GetOrdinal("Id")),
                                IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSuperVisor"))
                            };

                            department.Employees.Add(newEmployee);
                            
                        }
                        else
                        {
                            department = newDepartment;
                        }

                    }

                    reader.Close();

                    return Ok(department);
                }
            }
        }

        //// POST api/customers
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] Customer customer)
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            // More string interpolation
        //            cmd.CommandText = @"
        //                INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate)
        //                OUTPUT INSERTED.Id
        //                VALUES (@FirstName, @LastName, @CreationDate, @LastActiveDate)
        //            ";
        //            cmd.Parameters.Add(new SqlParameter("@FirstName", customer.FirstName));
        //            cmd.Parameters.Add(new SqlParameter("@LastName", customer.LastName));
        //            cmd.Parameters.Add(new SqlParameter("@CreationDate", customer.CreationDate));
        //            cmd.Parameters.Add(new SqlParameter("@LastActiveDate", customer.LastActiveDate));

        //            customer.Id = (int)await cmd.ExecuteScalarAsync();

        //            return CreatedAtRoute("GetCustomer", new { id = customer.Id }, customer);
        //        }
        //    }
        //}

        //// PUT api/customers/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Customer customer)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = Connection)
        //        {
        //            conn.Open();
        //            using (SqlCommand cmd = conn.CreateCommand())
        //            {
        //                cmd.CommandText = @"
        //                    UPDATE Customer
        //                    SET FirstName = @FirstName, LastName = @LastName, CreationDate = @CreationDate, LastActiveDate = @LastActiveDate
        //                    WHERE Id = @Id";
        //                cmd.Parameters.Add(new SqlParameter("@FirstName", customer.FirstName));
        //                cmd.Parameters.Add(new SqlParameter("@LastName", customer.LastName));
        //                cmd.Parameters.Add(new SqlParameter("@CreationDate", customer.CreationDate));
        //                cmd.Parameters.Add(new SqlParameter("@LastActiveDate", customer.LastActiveDate));
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
        //        if (!CustomerExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}

        ////// DELETE api/customers/5
        ////[HttpDelete("{id}")]
        ////public async Task<IActionResult> Delete(int id)
        ////{
        ////    throw new NotImplementedException("This method isn't implemented...yet.");
        ////}

        //private bool CustomerExists(int id)
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = "SELECT Id FROM Customer WHERE Id = @id";
        //            cmd.Parameters.Add(new SqlParameter("@id", id));

        //            SqlDataReader reader = cmd.ExecuteReader();

        //            return reader.Read();
        //        }
        //    }
        //}
    }
}
