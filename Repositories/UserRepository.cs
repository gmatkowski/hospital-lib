using Hospital.Abstracts;
using Hospital.DB;
using Hospital.Interfaces;
using Hospital.Models;
using System.Data.Entity;
using System.Linq;

namespace Hospital.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(HospitalContext context) : base(context) { }

        public bool HasAdmins()
        {
            return _dataContext.Set<User>().Where(o => (o is Admin)).Count() > 0;
        }
    }
}
