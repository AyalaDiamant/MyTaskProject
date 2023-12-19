using core.Interfaces;
using core.Models;
using System.Linq;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System;
using System.Net;


namespace core.Services
{
    public class TaskService : ITaskService
    {
        List<Task> Tasks { get; }
        private IWebHostEnvironment  webHost;
        private string filePath;
        public TaskService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Task.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                Tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            System.Console.WriteLine(Tasks);
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(Tasks));
        }
        public List<Task> GetAll(long userId) 
        {
            return Tasks.Where(p => p.AgentId == userId).ToList();
        }

        public Task Get(long userId, int id) 
        { 
            return Tasks.FirstOrDefault(p => p.AgentId == userId && p.Id == id);
        }

        public void Add(long userId, Task Task)
        {
            Task.Id = Tasks.Count() + 1;
            Task.AgentId = userId;
            Tasks.Add(Task);
            saveToFile();
        }

        public void Delete(long userId, int id)
        {
            var Task = Get(userId, id);
            if (Task is null)
                return;

            Tasks.Remove(Task);
            saveToFile();
        }

        public void Update(long userId, Task Task)
        {
            var index = Tasks.FindIndex(t => t.AgentId == userId &&  t.Id == Task.Id);
            if (index == -1)
                return;

            Tasks[index] = Task;
            saveToFile();
        }

        
        public int Count(long userId) 
        { 
            return GetAll(userId).Count();
        }
    }
}
