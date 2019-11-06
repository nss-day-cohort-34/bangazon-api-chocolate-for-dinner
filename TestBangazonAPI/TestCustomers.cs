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
    public class TestCustomers
    {
        [Fact]
        public async Task Test_Get_All_Customers()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/customers");


                string responseBody = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<Customer>>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(customers.Count > 0);
            }
        }
        [Fact]
        public async Task Test_Get_One_Customer()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/customers/1");


                string responseBody = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(customer.Id > 0);
            }
        }

        [Fact]
        public async Task Test_Create_Customer()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */

                Customer newCustomer = new Customer
                {
                    FirstName = "Bryan",
                    LastName = "Nilsen",
                    CreationDate = DateTime.Now,
                    LastActiveDate = DateTime.Now
                };

                var newCustomerAsJSON = JsonConvert.SerializeObject(newCustomer);

                /*
                    ACT
                */
                var response = await client.PostAsync("/api/customers",
                    new StringContent(newCustomerAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Bryan", customer.FirstName);
                Assert.Equal("Nilsen", customer.LastName);
            }
        }

        [Fact]
        public async Task Test_Modify_Customer()
        {
            using (var client = new APIClientProvider().Client)
            {
                string newCustomerFirstName = "Jenna";
                string newCustomerLastName = "Solis";

                Customer modifiedCustomer = new Customer
                {
                    FirstName = newCustomerFirstName,
                    LastName = newCustomerLastName,
                    CreationDate = DateTime.Now,
                    LastActiveDate = DateTime.Now
                };

                var newCustomerAsJSON = JsonConvert.SerializeObject(modifiedCustomer);

                /*
                    ACT
                */
                var response = await client.PutAsync("/api/customers/6",
                    new StringContent(newCustomerAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                    Get Section
                */

                var getCustomer = await client.GetAsync("/api/customers/6");
                getCustomer.EnsureSuccessStatusCode();

                string getCustomerBody = await getCustomer.Content.ReadAsStringAsync();

                Customer customer = JsonConvert.DeserializeObject<Customer>(getCustomerBody);

                Assert.Equal(HttpStatusCode.OK, getCustomer.StatusCode);
                Assert.Equal(newCustomerFirstName, customer.FirstName);
                Assert.Equal(newCustomerLastName, customer.LastName);

            }
        }

    }
}
