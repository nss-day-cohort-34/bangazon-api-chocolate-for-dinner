using Newtonsoft.Json;
using BangazonAPI.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System;

namespace TestBangazonAPI
{
    public class TestComputer
    {


       

        public async Task<Computer> createComputer(HttpClient client)
        {
            Computer computer = new Computer
            {
                PurchaseDate = new DateTime(2019, 05, 13),
                Make = "Test Computer",
                Manufacturer = "Dell"


            };
            string computerAsJSON = JsonConvert.SerializeObject(computer);


            HttpResponseMessage response = await client.PostAsync(
                "api/computers",
                new StringContent(computerAsJSON, Encoding.UTF8, "application/json")
            );

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            Computer newComputer = JsonConvert.DeserializeObject<Computer>(responseBody);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            return newComputer;

        }

        [Fact]
        public async Task Test_Get_All_Computers()
        {

            // Use the http client
            using (HttpClient client = new APIClientProvider().Client)
            {

                // Call the route to get all our computers; wait for a response object
                HttpResponseMessage response = await client.GetAsync("api/computers");

                // Make sure that a response comes back at all
                response.EnsureSuccessStatusCode();

                // Read the response body as JSON
                string responseBody = await response.Content.ReadAsStringAsync();

                // Convert the JSON to a list of computer instances
                List<Computer> computerList = JsonConvert.DeserializeObject<List<Computer>>(responseBody);

                // Did we get back a 200 OK status code?
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                // Are there any computers in the list?
                Assert.True(computerList.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Get_Single_Computer()
        {

            using (HttpClient client = new APIClientProvider().Client)
            {

                // Create a new computer
                Computer newComputer = await createComputer(client);

                // Try to get that computer from the database
                HttpResponseMessage response = await client.GetAsync($"api/computers/{newComputer.Id}");

                response.EnsureSuccessStatusCode();

                // Turn the response into JSON
                string responseBody = await response.Content.ReadAsStringAsync();

                // Turn the JSON into C#
                Computer computer = JsonConvert.DeserializeObject<Computer>(responseBody);

                // Did we get back what we expected to get back? 
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("Test Computer", newComputer.Make);


                // Clean up after ourselves- delete computer!
                await deleteComputer(newComputer, client);
            }
        }

       
        [Fact]
        public async Task Test_Create_And_Delete_Computer()
        {
            using (var client = new APIClientProvider().Client)
            {

                // Create a new Computer
                Computer newComputer = await createComputer(client);

                // Make sure the info checks out
                Assert.Equal("Test Computer", newComputer.Make);



                // Clean up after ourselves - delete Computer!
                await deleteComputer(newComputer, client);
            }
        }

     
        [Fact]
        public async Task Test_Modify_Computer()
        {

            // We're going to change a computer's make! This is their new make.
            string newMake = "Cool Computer";

            using (HttpClient client = new APIClientProvider().Client)
            {

                // Create a new computer
                Computer newComputer = await createComputer(client);

                // Change their first make
                newComputer.Make = newMake;

                // Convert them to JSON
                string modifiedComputerAsJSON = JsonConvert.SerializeObject(newComputer);

                // Make a PUT request with the new info
                HttpResponseMessage response = await client.PutAsync(
                    $"api/computers/{newComputer.Id}",
                    new StringContent(modifiedComputerAsJSON, Encoding.UTF8, "application/json")
                );


                response.EnsureSuccessStatusCode();

                // Convert the response to JSON
                string responseBody = await response.Content.ReadAsStringAsync();

                // We should have gotten a no content status code
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                    GET section
                 */
                // Try to GET the computer we just edited
                HttpResponseMessage getComputer = await client.GetAsync($"api/computers/{newComputer.Id}");
                getComputer.EnsureSuccessStatusCode();

                string getComputerBody = await getComputer.Content.ReadAsStringAsync();
                Computer modifiedComputer = JsonConvert.DeserializeObject<Computer>(getComputerBody);

                Assert.Equal(HttpStatusCode.OK, getComputer.StatusCode);

                // Make sure the Make was in fact updated
                Assert.Equal(newMake, modifiedComputer.Make);

                // Clean up after ourselves- delete it
                await deleteComputer(modifiedComputer, client);
            }
        }
        // Delete a computer in the database and make sure we get a no content status code back
        private async Task deleteComputer(Computer computer, HttpClient client)
        {
            HttpResponseMessage deleteResponse = await client.DeleteAsync($"api/computers/{computer.Id}");
            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        }

    }
}
