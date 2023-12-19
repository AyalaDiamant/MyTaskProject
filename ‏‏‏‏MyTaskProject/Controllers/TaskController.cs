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

    [Authorize(Policy = "Agent")]
    public class TaskController : ControllerBase
    {
        private long _userId;
        private ITaskService _taskService;
        public TaskController(ITaskService taskService, IHttpContextAccessor httpContextAccessor)
        {
            _taskService = taskService;
            _userId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value);
        }


        [HttpGet]
        [Authorize(Policy = "Agent")]
        public List<Task> GetAll() =>
            _taskService.GetAll(_userId);

        [HttpGet("{id}")]
        [Authorize(Policy = "Agent")]
        public ActionResult<Task> Get(int id)
        {
            var Task = _taskService.Get(_userId, id);

            if (Task == null)
                return NotFound();
            return Task;
        }

        [HttpPost]
        [Authorize(Policy = "Agent")]
        public IActionResult Create(Task Task)
        {
            _taskService.Add(_userId, Task);
            return CreatedAtAction(nameof(Create), new { id = Task.Id }, Task);

        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Agent")]
        public IActionResult Update(long userId, int id, Task Task)
        {
            if (id != Task.Id)
                return BadRequest();

            var existingTask = _taskService.Get(_userId, id);
            if (existingTask is null)
                return NotFound();

            _taskService.Update(_userId, Task);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Agent")]
        public IActionResult Delete(int id)
        {
            var Task = _taskService.Get(_userId, id);
            if (Task is null)
                return NotFound();

            _taskService.Delete(_userId, id);

            return Content(_taskService.Count(_userId).ToString());
        }
    }
}
