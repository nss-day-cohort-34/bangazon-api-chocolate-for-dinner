using System.Net;
using Newtonsoft.Json;
using Xunit;
using BangazonAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Text;

namespace TestBangazonAPI
{
    public class TestTrainingPrograms
    {
        [Fact]
        public async Task Test_Get_All_TrainingPrograms()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/trainingPrograms");

                string responseBody = await response.Content.ReadAsStringAsync();
                var trainingPrograms = JsonConvert.DeserializeObject<List<TrainingProgram>>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(trainingPrograms.Count > 0);
            }
        }
        [Fact]
        public async Task Test_Get_One_TrainingProgram()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/trainingPrograms/1");


                string responseBody = await response.Content.ReadAsStringAsync();
                var trainingProgram = JsonConvert.DeserializeObject<TrainingProgram>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(trainingProgram.Id > 0);
            }
        }

        [Fact]
        public async Task Test_Create_TrainingProgram()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */

                TrainingProgram newTrainingProgram = new TrainingProgram
                {
                    Name = "Safety Training",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Now,
                    MaxAttendees = 25,
                    IsDeleted = false
                };

                var newTrainingProgramAsJSON = JsonConvert.SerializeObject(newTrainingProgram);

                /*
                    ACT
                */
                var response = await client.PostAsync("/api/trainingPrograms",
                new StringContent(newTrainingProgramAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();
                var trainingProgram = JsonConvert.DeserializeObject<TrainingProgram>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Safety Training", trainingProgram.Name);
                Assert.Equal(25, trainingProgram.MaxAttendees);
                Assert.False(trainingProgram.IsDeleted);
            }
        }

        [Fact]
        public async Task Test_Modify_TrainingProgram()
        {
            using (var client = new APIClientProvider().Client)
            {
                string newTrainingProgramName = "Safety Training 2.0";

                TrainingProgram modifiedTrainingProgram = new TrainingProgram
                {
                    Name = newTrainingProgramName,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Now,
                    MaxAttendees = 20,
                    IsDeleted = false
                };

                var newTrainingProgramAsJSON = JsonConvert.SerializeObject(modifiedTrainingProgram);

                /*
                    ACT
                */
                var response = await client.PutAsync("/api/trainingPrograms/1",
                    new StringContent(newTrainingProgramAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                    Get Section
                */

                var getTrainingProgram = await client.GetAsync("/api/trainingPrograms/1");
                getTrainingProgram.EnsureSuccessStatusCode();

                string getTrainingProgramBody = await getTrainingProgram.Content.ReadAsStringAsync();

                TrainingProgram trainingProgram = JsonConvert.DeserializeObject<TrainingProgram>(getTrainingProgramBody);

                Assert.Equal(HttpStatusCode.OK, getTrainingProgram.StatusCode);
                Assert.Equal(newTrainingProgramName, modifiedTrainingProgram.Name);
                Assert.Equal(20, modifiedTrainingProgram.MaxAttendees);

            }
        }

        [Fact]
        public async Task Test_Delete_TrainingProgram()
        {
            using (var client = new APIClientProvider().Client)
            {
                bool trainingProgramIsDeleted = true;
                string newTrainingProgramName = "Safety Training 2.0";

                TrainingProgram softDeletedTrainingProgram = new TrainingProgram
                {
                    Name = newTrainingProgramName,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Now,
                    MaxAttendees = 20,
                    IsDeleted = trainingProgramIsDeleted
                };

                var newTrainingProgramAsJSON = JsonConvert.SerializeObject(softDeletedTrainingProgram);

                /*
                    ACT
                */
                var response = await client.PutAsync("/api/trainingPrograms/2",
                    new StringContent(newTrainingProgramAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                    Get Section
                */

                var getTrainingProgram = await client.GetAsync("/api/trainingPrograms/1");
                getTrainingProgram.EnsureSuccessStatusCode();

                string getTrainingProgramBody = await getTrainingProgram.Content.ReadAsStringAsync();

                TrainingProgram trainingProgram = JsonConvert.DeserializeObject<TrainingProgram>(getTrainingProgramBody);

                Assert.Equal(HttpStatusCode.OK, getTrainingProgram.StatusCode);
                Assert.Equal(trainingProgramIsDeleted, softDeletedTrainingProgram.IsDeleted);
            }
        }
    }
}
