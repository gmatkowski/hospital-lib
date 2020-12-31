using Hospital.Abstracts;
using Hospital.Models;
using System;
using System.Collections.Generic;

namespace Hospital.Interfaces
{
    public interface IShiftRepository : IRepository<Shift>
    {
        List<Shift> GetForEmployee(Employee employee, DateTime date);
        bool CanHaveAnotherInThisMont(Employee employee, DateTime date, int max);
        bool CanHaveAtPointedDate(Employee employee, DateTime date);
        bool CanHaveAtPointedDateDoctorLookup(Doctor doctor, DateTime date);
        bool CanHaveAtPointedDateExact(Employee employee, DateTime date);
    }
}
