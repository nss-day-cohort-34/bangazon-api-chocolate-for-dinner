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
//    public class DepartmentsController : ControllerBase
//    {
//        private readonly IConfiguration _config;

//        public DepartmentsController(IConfiguration config)
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

//        // GET api/departments
//        [HttpGet]
//        public async Task<IActionResult> Get()
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = "Write your SQL statement here to get all departments";
//                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

//                    List<Department> departments = new List<Department>();
//                    while (reader.Read())
//                    {
//                        Department department = new Department
//                        {
//                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
//                            // You might have more columns
//                        };

//                        departments.Add(department);
//                    }

//                    reader.Close();

//                    return Ok(departments);
//                }
//            }
//        }

//        // GET api/departments/5
//        [HttpGet("{id}")]
//        public async Task<IActionResult> Get(int id)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = "Write your SQL statement here to get a single department";
//                    cmd.Parameters.Add(new SqlParameter("@id", id));
//                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

//                    Department department = null;
//                    if (reader.Read())
//                    {
//                        department = new Department
//                        {
//                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
//                            // You might have more columns
//                        };
//                    }

//                    reader.Close();

//                    return Ok(department);
//                }
//            }
//        }

//        // POST api/departments
//        [HttpPost]
//        public async Task<IActionResult> Post([FromBody] Department department)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    // More string interpolation
//                    cmd.CommandText = @"
//                        INSERT INTO Department ()
//                        OUTPUT INSERTED.Id
//                        VALUES ()
//                    ";
//                    cmd.Parameters.Add(new SqlParameter("@firstName", department.FirstName));

//                    department.Id = (int) await cmd.ExecuteScalarAsync();

//                    return CreatedAtRoute("GetDepartment", new { id = department.Id }, department);
//                }
//            }
//        }

//        // PUT api/departments/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Put(int id, [FromBody] Department department)
//        {
//            try
//            {
//                using (SqlConnection conn = Connection)
//                {
//                    conn.Open();
//                    using (SqlCommand cmd = conn.CreateCommand())
//                    {
//                        cmd.CommandText = @"
//                            UPDATE Department
//                            SET FirstName = @firstName
//                            -- Set the remaining columns here
//                            WHERE Id = @id
//                        ";
//                        cmd.Parameters.Add(new SqlParameter("@id", department.Id));
//                        cmd.Parameters.Add(new SqlParameter("@firstName", department.FirstName));

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
//                if (!DepartmentExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//        }

//        // DELETE api/departments/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            throw new NotImplementedException("This method isn't implemented...yet.");
//        }

//        private bool DepartmentExists(int id)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = "SELECT Id FROM Department WHERE Id = @id";
//                    cmd.Parameters.Add(new SqlParameter("@id", id));

//                    SqlDataReader reader = cmd.ExecuteReader();

//                    return reader.Read();
//                }
//            }
//        }
//    }
//}
