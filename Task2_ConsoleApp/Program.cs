using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Task2_ConsoleApp.Model;

namespace EmployeeClient
{
    class Program
    {
        private static readonly HttpClientHandler handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        };

        private static readonly HttpClient client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://localhost:7046/")
        };

        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Employee API Console Client ===");

            while (true)
            {
                Console.WriteLine("\nChoose an action:");
                Console.WriteLine("1. Get all employees");
                Console.WriteLine("2. Get employee by ID");
                Console.WriteLine("3. Get employees by department");
                Console.WriteLine("4. Add new employee");
                Console.WriteLine("5. Update employee");
                Console.WriteLine("6. Update employee email");
                Console.WriteLine("7. Delete employee");
                Console.WriteLine("8. Exit");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await GetAllEmployees();
                        break;
                    case "2":
                        await GetEmployeeById();
                        break;
                    case "3":
                        await GetEmployeesByDept();
                        break;
                    case "4":
                        await AddEmployee();
                        break;
                    case "5":
                        await UpdateEmployee();
                        break;
                    case "6":
                        await UpdateEmployeeEmail();
                        break;
                    case "7":
                        await DeleteEmployee();
                        break;
                    case "8":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }



        static async Task GetAllEmployees()
        {
            var res = await client.GetAsync("api/Employee");
            if (!res.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {res.StatusCode}");
                return;
            }

            var json = await res.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<Employee>>(json);
            Console.WriteLine("\n--- Employee List ---");
            foreach (var e in list)
                Console.WriteLine($"{e.Id} | {e.Name} | {e.Department} | {e.MobileNo} | {e.Email}");
        }

        static async Task GetEmployeeById()
        {
            Console.Write("Enter Employee ID: ");
            var id = Console.ReadLine();
            var res = await client.GetAsync($"api/Employee/{id}");

            if (!res.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {res.StatusCode}");
                return;
            }

            var json = await res.Content.ReadAsStringAsync();
            var emp = JsonConvert.DeserializeObject<Employee>(json);
            Console.WriteLine($"\n{emp.Id} | {emp.Name} | {emp.Department} | {emp.MobileNo} | {emp.Email}");
        }

        static async Task GetEmployeesByDept()
        {
            Console.Write("Enter Department: ");
            var dept = Console.ReadLine();
            var res = await client.GetAsync($"api/Employee/bydept?department={dept}");

            if (!res.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {res.StatusCode}");
                return;
            }

            var json = await res.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<Employee>>(json);

            Console.WriteLine($"\n--- {dept.ToUpper()} Department ---");
            foreach (var e in list)
                Console.WriteLine($"{e.Id} | {e.Name} | {e.MobileNo} | {e.Email}");
        }

        static async Task AddEmployee()
        {
            Console.WriteLine("Enter new employee details:");
            var emp = new Employee();

            Console.Write("Id: ");
            emp.Id = int.Parse(Console.ReadLine());

            Console.Write("Name: ");
            emp.Name = Console.ReadLine();

            Console.Write("Department: ");
            emp.Department = Console.ReadLine();

            Console.Write("Mobile (10 digits): ");
            emp.MobileNo = Console.ReadLine();

            Console.Write("Email: ");
            emp.Email = Console.ReadLine();

            var json = JsonConvert.SerializeObject(emp);
            var res = await client.PostAsync("api/Employee",
                new StringContent(json, Encoding.UTF8, "application/json"));

            Console.WriteLine($"Response: {res.StatusCode}");
        }

        static async Task UpdateEmployee()
        {
            Console.Write("Enter Employee ID to update: ");
            var id = int.Parse(Console.ReadLine());

            Console.Write("Name: ");
            var name = Console.ReadLine();

            Console.Write("Department: ");
            var dept = Console.ReadLine();

            Console.Write("Mobile: ");
            var mob = Console.ReadLine();

            Console.Write("Email: ");
            var email = Console.ReadLine();

            var emp = new Employee { Id = id, Name = name, Department = dept, MobileNo = mob, Email = email };
            var json = JsonConvert.SerializeObject(emp);

            var res = await client.PutAsync($"api/Employee/{id}",
                new StringContent(json, Encoding.UTF8, "application/json"));

            Console.WriteLine($"Response: {res.StatusCode}");
        }

        static async Task UpdateEmployeeEmail()
        {
            Console.Write("Enter Employee ID to update email: ");
            var id = Console.ReadLine();

            Console.Write("New Email: ");
            var email = Console.ReadLine();

            var json = JsonConvert.SerializeObject(new { Email = email });

            var res = await client.PatchAsync($"api/Employee/{id}/email",
                new StringContent(json, Encoding.UTF8, "application/json"));

            Console.WriteLine($"Response: {res.StatusCode}");
        }

        static async Task DeleteEmployee()
        {
            Console.Write("Enter Employee ID to delete: ");
            var id = Console.ReadLine();

            var res = await client.DeleteAsync($"api/Employee/{id}");
            Console.WriteLine($"Response: {res.StatusCode}");
        }
    }

    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = content
            };
            return await client.SendAsync(request);
        }
    }
}
