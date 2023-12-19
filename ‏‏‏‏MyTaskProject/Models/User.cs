using System;

namespace core.Models
{
    public class User
    {
        public long UserId {get;set;}
        public string Name { get; set; }
        public string Password { get; set; }
        public bool TaskManager {get;set;}

    }
}
