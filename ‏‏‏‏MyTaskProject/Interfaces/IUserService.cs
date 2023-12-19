
using System.Collections.Generic;
using System.Linq;
namespace core.Interfaces
{
    using core.Models;
    public interface IUserService
    {
        List<User> GetAll();
        User Get(int id);
        void Post(User user);
        void Delete(int id);
    }
}
