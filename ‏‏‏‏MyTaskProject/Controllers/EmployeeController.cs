using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using core.Interfaces;
using core.Models;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Http;

namespace core.Controllers
{
    [ApiController]
    [Route("[controller]")]

    [Authorize(Policy = "TaskManager")]
    public class EmployeeController : ControllerBase
    {
        private long _userId;
        private IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService, IHttpContextAccessor httpContextAccessor)
        {
            _employeeService = employeeService;
            _userId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value);
        }


        [HttpGet]
        public List<User> GetAll() =>
            _employeeService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            var Employee = _employeeService.Get(id);

            if (Employee == null)
                return NotFound();
            return Employee;
        }

        [HttpPost]
        public IActionResult Create(User Employee)
        {
            _employeeService.Add(Employee);
            return CreatedAtAction(nameof(Create), new { id = Employee.UserId }, Employee);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User Employee)
        {
            if (id != Employee.UserId)
                return BadRequest();

            var existingEmployee = _employeeService.Get(id);
            if (existingEmployee is null)
                return NotFound();

            _employeeService.Update(Employee);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var Employee = _employeeService.Get(id);
            if (Employee is null)
                return NotFound();

            _employeeService.Delete(id);

            return Content(_employeeService.Count().ToString());
        }
    }
}
