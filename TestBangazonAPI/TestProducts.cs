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
    public class TestProducts
    {
        [Fact]
        public async Task Test_Get_All_Products()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/products");


                string responseBody = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(products.Count > 0);
            }
        }
        [Fact]
        public async Task Test_Get_One_Product()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/products/1");


                string responseBody = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Product>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(product.Id > 0);
            }
        }

        [Fact]
        public async Task Test_Create_Product()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */

                Product newProduct = new Product
                {
                    ProductTypeId = 1,
                    CustomerId = 1,
                    Price = 13.99M,
                    Title = "New Product",
                    Description = "This product will make your life better",
                    Quantity = 5
                };

                var newProductAsJSON = JsonConvert.SerializeObject(newProduct);

                /*
                    ACT
                */
                var response = await client.PostAsync("/api/products",
                    new StringContent(newProductAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Product>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(1, product.ProductTypeId);
                Assert.Equal(1, product.CustomerId);
                Assert.Equal(13.99M, product.Price);
                Assert.Equal("New Product", product.Title);
                Assert.Equal("This product will make your life better", product.Description);
                Assert.Equal(5, product.Quantity);

            }
        }

        [Fact]
        public async Task Test_Modify_Product()
        {
            using (var client = new APIClientProvider().Client)
            {
                string newProductTitle = "New Product v 2.0";
                string newProductDescription = "This is the most recent version of the New Product. It will change your life forever!";

                Product modifiedProduct = new Product
                {
                    ProductTypeId = 1,
                    CustomerId = 1,
                    Price = 13.99M,
                    Title = newProductTitle,
                    Description = newProductDescription,
                    Quantity = 5
                };

                var newProductAsJSON = JsonConvert.SerializeObject(modifiedProduct);

                /*
                    ACT
                */
                var response = await client.PutAsync("/api/products/1",
                    new StringContent(newProductAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                    Get Section
                */

                var getProduct = await client.GetAsync("/api/products/1");
                getProduct.EnsureSuccessStatusCode();

                string getProductBody = await getProduct.Content.ReadAsStringAsync();

                Product product = JsonConvert.DeserializeObject<Product>(getProductBody);

                Assert.Equal(HttpStatusCode.OK, getProduct.StatusCode);
                Assert.Equal(newProductTitle, modifiedProduct.Title);
                Assert.Equal(newProductDescription, modifiedProduct.Description);

            }
        }

        [Fact]
        public async Task Test_Delete_Product()
        {
            using (var client = new APIClientProvider().Client)
            {

                /*
                    ACT
                */
                var response = await client.DeleteAsync("/api/products/1");

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

    }
}
