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
                        cmd.CommandText = @"SELECT Id, Name, StartDate, EndDate, MaxAttendees, IsDeleted
                                              FROM TrainingProgram
                                             WHERE (StartDate >= SYSDATETIME()) AND IsDeleted = 0";
                    }

                    else
                    {
                        cmd.CommandText = @"SELECT Id, Name, StartDate, EndDate, MaxAttendees, IsDeleted
                                              FROM TrainingProgram
                                             WHERE IsDeleted = 0";
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
                            MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees"))
                        };
                        TrainingPrograms.Add(newtrainingProgram);
                    }

                    reader.Close();

                    return Ok(TrainingPrograms);
                }
            }
        }

        // GET api/trainingPrograms/1
        [HttpGet("{id}", Name = "GettrainingProgram")]
        public async Task<IActionResult> Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = @"SELECT Id, Name, StartDate, EndDate, MaxAttendees
                                          FROM TrainingProgram
                                         WHERE Id = @Id";

                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    TrainingProgram selectedtrainingProgram = null;

                    while (reader.Read())
                    {
                        if (selectedtrainingProgram == null)
                        {
                            selectedtrainingProgram = new TrainingProgram
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees"))
                            };
                        }

                    }
                    reader.Close();
                    return Ok(selectedtrainingProgram);
                }
            }
        }

        //// POST api/trainingPrograms
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TrainingProgram trainingProgram)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO TrainingProgram (
                                    Name, 
                                    StartDate, 
                                    EndDate, 
                                    MaxAttendees,
                                    IsDeleted
                                    )
                        OUTPUT INSERTED.Id
                        VALUES (@Name, @StartDate, @EndDate, @MaxAttendees, @IsDeleted)
                    ";
                    cmd.Parameters.Add(new SqlParameter("@Name", trainingProgram.Name));
                    cmd.Parameters.Add(new SqlParameter("@StartDate", trainingProgram.StartDate));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", trainingProgram.EndDate));
                    cmd.Parameters.Add(new SqlParameter("@MaxAttendees", trainingProgram.MaxAttendees));
                    cmd.Parameters.Add(new SqlParameter("@IsDeleted", trainingProgram.IsDeleted));

                    trainingProgram.Id = (int)await cmd.ExecuteScalarAsync();

                    return CreatedAtRoute("GettrainingProgram", new { id = trainingProgram.Id }, trainingProgram);
                }
            }
        }

        // PUT api/trainingPrograms/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] TrainingProgram trainingProgram)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            UPDATE TrainingProgram
                               SET Name = @Name, 
                                   StartDate = @StartDate, 
                                   EndDate = @EndDate, 
                                   MaxAttendees = @MaxAttendees,
                                   IsDeleted = @IsDeleted
                             WHERE Id = @Id";
                        cmd.Parameters.Add(new SqlParameter("@Name", trainingProgram.Name));
                        cmd.Parameters.Add(new SqlParameter("@StartDate", trainingProgram.StartDate));
                        cmd.Parameters.Add(new SqlParameter("@EndDate", trainingProgram.EndDate));
                        cmd.Parameters.Add(new SqlParameter("@MaxAttendees", trainingProgram.MaxAttendees));
                        cmd.Parameters.Add(new SqlParameter("@IsDeleted", trainingProgram.IsDeleted));
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
                if (!TrainingProgramExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE TrainingProgram 
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

        private bool TrainingProgramExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM trainingProgram WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    return reader.Read();
                }
            }
        }
    }
}