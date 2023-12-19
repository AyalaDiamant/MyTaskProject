using core.Interfaces;
using core.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace core.Services
{
    public class EmployeeService : IEmployeeService
    {

        List<User> _employees;


        private IWebHostEnvironment webHost;
        private string filePath;
        public EmployeeService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Employee.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                _employees = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            System.Console.WriteLine(_employees);
        }
        public void Add(User Employee)
        {
            Employee.UserId = _employees.Count() + 1;
            Employee.TaskManager = false;
            _employees.Add(Employee);
            saveToFile();
        }

        public int Count()
        {
            return _employees.Count();
        }

        public void Delete(int id)
        {
            var Employee = Get(id);
            if (Employee is null)
                return;

            _employees.Remove(Employee);
            saveToFile();
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(_employees));
        }

        public User Get(int id)
        {
            return _employees.FirstOrDefault(e => e.UserId == id);
        }

        public List<User> GetAll()
        {
            return _employees;
        }

        public void Update(User Employee)
        {
            var index = _employees.FindIndex(e =>  e.UserId == Employee.UserId);
            if (index == -1)
                return;

            _employees[index] = Employee;
            saveToFile();
        }
    }
}
