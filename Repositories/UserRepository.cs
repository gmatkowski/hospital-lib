using Hospital.Abstracts;
using Hospital.DB;
using Hospital.Exceptions;
using Hospital.Interfaces;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Hospital.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(HospitalContext context) : base(context) { }

        public bool HasAdmins()
        {
            return _dataContext.Set<User>().Where(o => (o is Admin)).Count() > 0;
        }

        public User GetByUsername(string username)
        {
            try
            {
                return _dataContext.Set<User>().Where(o => o.username == username).First();
            }
            catch(Exception ex)
            {
                throw new UserNotFoundException();
            }
        }

        public List<User> GetListingForUser(User user)
        { 
            IQueryable<User> query = _dataContext.Set<User>();

            if(user is Employee)
            {
                query = query.Where(o => o is Employee);
            }

            return query.ToList();
        }
    }
}
