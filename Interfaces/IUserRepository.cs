using Hospital.Abstracts;

namespace Hospital.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        bool HasAdmins();
    }
}
