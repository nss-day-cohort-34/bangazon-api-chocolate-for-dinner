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
    public class TestPaymentTypes
    {
        [Fact]
        public async Task Test_Get_All_PaymentTypes()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/paymentTypes");


                string responseBody = await response.Content.ReadAsStringAsync();
                var paymentTypes = JsonConvert.DeserializeObject<List<PaymentType>>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(paymentTypes.Count > 0);
            }
        }
        [Fact]
        public async Task Test_Get_One_PaymentType()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/paymentTypes/1");


                string responseBody = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<PaymentType>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(customer.Id > 0);
            }
        }

        [Fact]
        public async Task Test_Create_PaymentType()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */

                PaymentType newPaymentType = new PaymentType
                {
                    AcctNumber = "1111222233334444",
                    Name = "A Test PaymentType",
                    CustomerId = 1
                };

                var newPaymentTypeAsJSON = JsonConvert.SerializeObject(newPaymentType);

                /*
                    ACT
                */
                var response = await client.PostAsync("/api/paymentTypes",
                    new StringContent(newPaymentTypeAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();
                var paymentType = JsonConvert.DeserializeObject<PaymentType>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("1111222233334444", paymentType.AcctNumber);
                Assert.Equal("A Test PaymentType", paymentType.Name);
                Assert.Equal(1, paymentType.CustomerId);
            }
        }

        [Fact]
        public async Task Test_Modify_PaymentType()
        {
            using (var client = new APIClientProvider().Client)
            {
                string newPaymentTypeAcctNumber = "9999888877776666";
                string newPaymentTypeName = "BIGGER TEST";
                int newCustomerId = 2;

                PaymentType modifiedPaymentType = new PaymentType
                {
                    AcctNumber = newPaymentTypeAcctNumber,
                    Name = newPaymentTypeName,
                    CustomerId = newCustomerId
                };

                var newPaymentTypeAsJSON = JsonConvert.SerializeObject(modifiedPaymentType);

                /*
                    ACT
                */
                var response = await client.PutAsync("/api/paymentTypes/1",
                    new StringContent(newPaymentTypeAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                    Get Section
                */

                var getPaymentType = await client.GetAsync("/api/paymentTypes/1");
                getPaymentType.EnsureSuccessStatusCode();

                string getPaymentTypeBody = await getPaymentType.Content.ReadAsStringAsync();

                PaymentType paymentType = JsonConvert.DeserializeObject<PaymentType>(getPaymentTypeBody);

                Assert.Equal(HttpStatusCode.OK, getPaymentType.StatusCode);
                Assert.Equal(newPaymentTypeAcctNumber, paymentType.AcctNumber);
                Assert.Equal(newPaymentTypeName, paymentType.Name);
                Assert.Equal(newCustomerId, paymentType.CustomerId);

            }
        }

        [Fact]
        public async Task Task_Delete_PaymentType()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.DeleteAsync(
                    "/api/paymentTypes/12"
                    );
                string responseBody = await response.Content.ReadAsStringAsync();
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

    }
}
