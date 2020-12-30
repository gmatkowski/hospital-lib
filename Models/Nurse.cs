using Hospital.Abstracts;

namespace Hospital.Models
{
    public class Nurse : Employee
    {
        public override string GetRoleName()
        {
            return "Pielęgniarka";
        }
    }
}
