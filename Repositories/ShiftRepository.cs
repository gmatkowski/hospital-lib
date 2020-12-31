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


            //EntityFunctions.TruncateTime(x.DateTimeStart) == currentDate.Date

            return _dataContext
                .Set<Shift>()
                .Where(o => o.user_id == employee.id)
                .Where(o => DbFunctions.TruncateTime(o.date) >= firstDayOfMonth.Date && DbFunctions.TruncateTime(o.date) <= lastDayOfMonth)
                .ToList();
        }

    }
}
