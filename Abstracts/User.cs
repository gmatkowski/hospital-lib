using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Models;
using Hospital.Rules;

namespace Hospital.Abstracts
{
    [Table("users")]
    public abstract class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [
            Required,
            Unique,
            MinLength(3, ErrorMessage = "Pole {0} musi zawierać minimum {1} znaków"),
            RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Tylko znaki alfa numeryczne"),
            MaxLength(40, ErrorMessage = "Pole {0} jest za długie (max {1} znaków)")
        ]
        [Index("user_username", 0, IsUnique = true)]
        public string username { get; set; }

        [Required, MinLength(3, ErrorMessage = "Pole {0} musi zawierać minimum {1} znaków"), MaxLength(40, ErrorMessage = "Pole {0} jest za długie (max {1} znaków)")]
        public string first_name { get; set; }

        [Required, MinLength(3, ErrorMessage = "Pole {0} musi zawierać minimum {1} znaków"), MaxLength(40, ErrorMessage = "Pole {0} jest za długie (max {1} znaków)")]
        public string last_name { get; set; }

        [Required, MinLength(5, ErrorMessage = "Pole {0} musi zawierać minimum {1} znaków"), MaxLength(120, ErrorMessage = "Pole {0} jest za długie (max {1} znaków)")]
        public string password { get; set; }

        [NotMapped]
        public string full_name
        {
            get
            {
                return String.Format("{0} {1}", this.first_name, this.last_name);
            }
        }

        [NotMapped]
        public string role
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public virtual string GetRoleName()
        {
            return "Użytkownik";
        }

        public override string ToString()
        {
            return String.Format("{0} - {1} {2} {3}", this.id, this.username, this.first_name, this.last_name);
        }
    }
}
