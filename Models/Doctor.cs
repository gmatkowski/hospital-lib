using Hospital.Abstracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    public class Doctor : Employee
    {
        [Index("user_specialization", 0, IsUnique = false)]
        [Required, MinLength(3, ErrorMessage = "Pole {0} musi zawierać minimum {1} znaków")]
        public string specialization  {get;set;}

        [Index("user_pwz", 0, IsUnique = true)]
        [Required, Range(7,7, ErrorMessage = "Nieprawidłowy numer PWZ")]
        public int pwz { get; set; }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}", base.ToString(), this.pwz.ToString(), this.specialization);
        }
    }
}
