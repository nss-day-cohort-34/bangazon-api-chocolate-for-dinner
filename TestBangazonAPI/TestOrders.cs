
using BangazonAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestBangazonAPI
{
    public class TestOrders
    {
        [Fact]
        public async Task Test_Get_All_Orders()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/orders");


                string responseBody = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<Order>>(responseBody);

                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(orders.Count > 0);
            }
        }

      

        [Fact]
        public async Task Test_Get_One_Order()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/orders/1");


                string responseBody = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(responseBody);

                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(1, order.Id);
                Assert.Equal(1, order.CustomerId);
                Assert.Equal(1, order.PaymentTypeId);
             
            }
        }

       
        [Fact]
        public async Task Test_Create_And_Delete_Order()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */
                Order order = new Order
                {
                    CustomerId = 3,
                    PaymentTypeId = 3
                };

                var OrderAsJSON = JsonConvert.SerializeObject(order);


                /*
                    ACT
                */

                var response = await client.PostAsync(
                    "/api/orders",
                    new StringContent(OrderAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();
                var NewOrder = JsonConvert.DeserializeObject<Order>(responseBody);

                /*
                    ASSERT
                */

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(order.CustomerId, NewOrder.CustomerId);
                Assert.Equal(order.PaymentTypeId, NewOrder.PaymentTypeId);

                /*
                    ACT
                */
                var deleteResponse = await client.DeleteAsync($"/api/orders/{NewOrder.Id}");

                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            }
        }
        [Fact]
        public async Task Modify_Order()
        {
            int NewCustomerId = 1;
            int NewPaymentTypeId = 1;

            using (var client = new APIClientProvider().Client)
            {
                /*
                    PUT
                */
                Order modifiedOrder = new Order
                {
                    CustomerId = NewCustomerId,
                    PaymentTypeId = NewPaymentTypeId
                };

                var modifiedOrderAsJSON = JsonConvert.SerializeObject(modifiedOrder);

                var response = await client.PutAsync(
                    "/api/orders/8",
                    new StringContent(modifiedOrderAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                   GET section
                */
                var getOrder = await client.GetAsync("/api/orders/8");
                getOrder.EnsureSuccessStatusCode();

                string getOrderBody = await getOrder.Content.ReadAsStringAsync();
                Order newOrder = JsonConvert.DeserializeObject<Order>(getOrderBody);

                /*
                   ASSERT
                */

                Assert.Equal(HttpStatusCode.OK, getOrder.StatusCode);
                Assert.Equal(NewCustomerId, newOrder.CustomerId);
                Assert.Equal(NewPaymentTypeId, newOrder.PaymentTypeId);
            }
        }
    }

}
