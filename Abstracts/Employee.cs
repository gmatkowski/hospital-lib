using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Abstracts
{
    public abstract class Employee: User
    {
        [Index("user_pesel", 0, IsUnique = true)]
        [Required, MaxLength(11, ErrorMessage = "Nieprawidłowy numer PESEL") ,MinLength(11, ErrorMessage = "Nieprawidłowy numer PESEL")]
        public string pesel { get; set; }

        public override string ToString()
        {
            return String.Format("{0}, {1}", base.ToString(), this.pesel);
        }
    }
}
