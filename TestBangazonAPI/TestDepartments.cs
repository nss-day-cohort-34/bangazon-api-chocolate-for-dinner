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
    public class TestDepartments
    {
        [Fact]
        public async Task Test_Get_All_Departments()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/departments");


                string responseBody = await response.Content.ReadAsStringAsync();
                var departments = JsonConvert.DeserializeObject<List<Department>>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(departments.Count > 0);
            }
        }
        [Fact]
        public async Task Test_Get_One_Department()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/departments/1");


                string responseBody = await response.Content.ReadAsStringAsync();
                var department = JsonConvert.DeserializeObject<Department>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(department.Id > 0);
            }
        }

        [Fact]
        public async Task Test_Create_Department()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */

                string newDepartmentName = "HR";
                int newDepartmentBudget = 322600;

                Department newDepartment = new Department
                {
                    Name = newDepartmentName,
                    Budget = newDepartmentBudget
                };

                var newDepartmentAsJSON = JsonConvert.SerializeObject(newDepartment);

                /*
                    ACT
                */
                var response = await client.PostAsync("/api/departments",
                    new StringContent(newDepartmentAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();
                var department = JsonConvert.DeserializeObject<Department>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(newDepartmentName, department.Name);
                Assert.Equal(newDepartmentBudget, department.Budget);
            }
        }

        [Fact]
        public async Task Test_Modify_Department()
        {
            using (var client = new APIClientProvider().Client)
            {
                string newDepartmentName = "Animal Wrangling";
                int newDepartmentBudget = 450200;

                Department modifiedDepartment = new Department
                {
                    Name = newDepartmentName,
                    Budget = newDepartmentBudget
                };

                var newDepartmentAsJSON = JsonConvert.SerializeObject(modifiedDepartment);

                /*
                    ACT
                */
                var response = await client.PutAsync("/api/departments/4",
                    new StringContent(newDepartmentAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                    Get Section
                */

                var getDepartment = await client.GetAsync("/api/departments/4");
                getDepartment.EnsureSuccessStatusCode();

                string getDepartmentBody = await getDepartment.Content.ReadAsStringAsync();

                Department department = JsonConvert.DeserializeObject<Department>(getDepartmentBody);

                Assert.Equal(HttpStatusCode.OK, getDepartment.StatusCode);
                Assert.Equal(newDepartmentName, department.Name);
                Assert.Equal(newDepartmentBudget, department.Budget);

            }
        }

    }
}
