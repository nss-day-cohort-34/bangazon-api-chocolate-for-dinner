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
    public class TestProductTypes
    {
        [Fact]
        public async Task Test_Get_All_ProductTypes()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/productTypes");


                string responseBody = await response.Content.ReadAsStringAsync();
                var productTypes = JsonConvert.DeserializeObject<List<ProductType>>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(productTypes.Count > 0);
            }
        }
        [Fact]
        public async Task Test_Get_One_ProductType()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/productTypes/1");


                string responseBody = await response.Content.ReadAsStringAsync();
                var productType = JsonConvert.DeserializeObject<ProductType>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(productType.Id > 0);
            }
        }

        [Fact]
        public async Task Test_Create_ProductType()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */

                ProductType newProductType = new ProductType
                {
                    Name = "Kitchen"
                };

                var newProductTypeAsJSON = JsonConvert.SerializeObject(newProductType);

                /*
                    ACT
                */
                var response = await client.PostAsync("/api/productTypes",
                    new StringContent(newProductTypeAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();
                var productType = JsonConvert.DeserializeObject<ProductType>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Kitchen", productType.Name);
            }
        }

        [Fact]
        public async Task Test_Modify_ProductType()
        {
            using (var client = new APIClientProvider().Client)
            {
                string newProductTypeName = "Active";

                ProductType modifiedProductType = new ProductType
                {
                    Name = newProductTypeName
                };

                var newProductTypeAsJSON = JsonConvert.SerializeObject(modifiedProductType);

                /*
                    ACT
                */
                var response = await client.PutAsync("/api/productTypes/3",
                    new StringContent(newProductTypeAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                    Get Section
                */

                var getProductType = await client.GetAsync("/api/productTypes/3");
                getProductType.EnsureSuccessStatusCode();

                string getProductTypeBody = await getProductType.Content.ReadAsStringAsync();

                ProductType productType = JsonConvert.DeserializeObject<ProductType>(getProductTypeBody);

                Assert.Equal(HttpStatusCode.OK, getProductType.StatusCode);
                Assert.Equal(newProductTypeName, productType.Name);

            }
        }
        [Fact]
        public async Task Task_Delete_ProductType()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.DeleteAsync(
                    "/api/productTypes/4"
                    );
                string responseBody = await response.Content.ReadAsStringAsync();
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

    }
}
