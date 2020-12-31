using Hospital.Abstracts;
using Hospital.DB;
using Hospital.Interfaces;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Hospital.Repositories
{
    public class ShiftRepository : BaseRepository<Shift>, IShiftRepository
    {
        public ShiftRepository(HospitalContext context) : base(context) { }

        public List<Shift> GetForEmployee(Employee employee, DateTime date)
        {
            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return _dataContext
                .Set<Shift>()
                .Where(o => o.user_id == employee.id)
                .Where(o => DbFunctions.TruncateTime(o.date) >= firstDayOfMonth.Date && DbFunctions.TruncateTime(o.date) <= lastDayOfMonth)
                .OrderBy(o => o.date)
                .ToList();
        }

        public bool CanHaveAnotherInThisMont(Employee employee, DateTime date, int max)
        {
            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return _dataContext
                .Set<Shift>()
                .Where(o => o.user_id == employee.id)
                .Where(o => DbFunctions.TruncateTime(o.date) >= firstDayOfMonth.Date && DbFunctions.TruncateTime(o.date) <= lastDayOfMonth)
                .Count() < max;
        }

        public bool CanHaveAtPointedDate(Employee employee, DateTime date)
        {
            DateTime before = date.AddDays(-1);
            DateTime after = date.AddDays(1);

            return _dataContext
                .Set<Shift>()
                .Where(o => (o.user_id == employee.id))
                .Where((o => DbFunctions.TruncateTime(o.date) == before.Date || DbFunctions.TruncateTime(o.date) == after.Date))
                .Count() == 0;
        }

        public bool CanHaveAtPointedDateExact(Employee employee, DateTime date)
        {
              return _dataContext
                .Set<Shift>()
                .Where(o => o.user_id == employee.id && DbFunctions.TruncateTime(o.date) == date.Date)
                .Count() == 0;
        }

        public bool CanHaveAtPointedDateDoctorLookup(Doctor doctor, DateTime date)
        {
            return _dataContext
                .Set<Shift>()
                .Where(o => o.user is Doctor && o.doctor.specialization == doctor.specialization && DbFunctions.TruncateTime(o.date) == date.Date)
                .Count() == 0;
        }
    }
}
