using Hospital.Abstracts;
using Hospital.Models;
using System;
using System.Collections.Generic;

namespace Hospital.Interfaces
{
    public interface IShiftRepository : IRepository<Shift>
    {
        List<Shift> GetForEmployee(Employee employee, DateTime date);
    }
}
