using core.Models;
using System.Collections.Generic;

namespace core.Interfaces
{
    public interface IEmployeeService
    {
        List<User> GetAll();
        User Get(int id);
        void Add(User Employee);
        void Delete(int id);
        void Update(User Employee);
        int Count ();
    }
}
