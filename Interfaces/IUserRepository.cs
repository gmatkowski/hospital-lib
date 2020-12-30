using Hospital.Abstracts;
using System.Collections.Generic;

namespace Hospital.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        bool HasAdmins();
        User GetByUsername(string username);
        List<User> GetListingForUser(User user);
    }
}
