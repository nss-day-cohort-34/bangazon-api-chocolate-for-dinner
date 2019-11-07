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
    public class TestEmployees
    {
        [Fact]
        public async Task Test_Get_All_Employees()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/employees");


                string responseBody = await response.Content.ReadAsStringAsync();
                var employees = JsonConvert.DeserializeObject<List<Employee>>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(employees.Count > 0);
            }
        }
        [Fact]
        public async Task Test_Get_One_Employee()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/employees/1");


                string responseBody = await response.Content.ReadAsStringAsync();
                var employee = JsonConvert.DeserializeObject<Employee>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(employee.Id > 0);
            }
        }

        [Fact]
        public async Task Test_Create_Employee()
        {
            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */

                Employee newEmployee = new Employee
                {
                    FirstName = "Bryan",
                    LastName = "Nilsen",
                    DepartmentId = 3,
                    IsSuperVisor = false
                };

                var newEmployeeAsJSON = JsonConvert.SerializeObject(newEmployee);

                /*
                    ACT
                */
                var response = await client.PostAsync("/api/employees",
                    new StringContent(newEmployeeAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();
                var employee = JsonConvert.DeserializeObject<Customer>(responseBody);
                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Bryan", employee.FirstName);
                Assert.Equal("Nilsen", employee.LastName);
            }
        }

        [Fact]
        public async Task Test_Modify_Employee()
        {
            using (var client = new APIClientProvider().Client)
            {
                string newEmployeeFirstName = "Jenna";
                string newEmployeeLastName = "Solis";

                Employee modifiedEmployee = new Employee
                {
                    FirstName = newEmployeeFirstName,
                    LastName = newEmployeeLastName,
                    DepartmentId = 1,
                    IsSuperVisor = false
                };

                var newEmployeeAsJSON = JsonConvert.SerializeObject(modifiedEmployee);

                /*
                    ACT
                */
                var response = await client.PutAsync("/api/employees/5",
                    new StringContent(newEmployeeAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                    Get Section
                */

                var getEmployee = await client.GetAsync("/api/employees/5");
                getEmployee.EnsureSuccessStatusCode();

                string getEmployeeBody = await getEmployee.Content.ReadAsStringAsync();

                Employee employee = JsonConvert.DeserializeObject<Employee>(getEmployeeBody);

                Assert.Equal(HttpStatusCode.OK, getEmployee.StatusCode);
                Assert.Equal(newEmployeeFirstName, employee.FirstName);
                Assert.Equal(newEmployeeLastName, employee.LastName);

            }
        }

    }
}
