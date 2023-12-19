using System.Collections.Generic;
using core.Models;
using core.Interfaces;
using System.Linq;
using System.IO;
using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace core.Interfaces   
{
    public interface ITaskService           
    {            
        List<Task> GetAll(long userId);     
        Task Get(long userId, int id);     
        void Add(long userId, Task Task);   
        void Delete(long userId, int id);   
        void Update(long userId, Task Task);
        int Count(long userId) ;           
    }     
}         
