using Hospital.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    [Table("shifts")]
    public class Shift
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Index("user_id", 0, IsUnique = false)]
        [Index("user_id_date", 1, IsUnique = true)]
        [Required]
        public int user_id { get; set; }

        [Column(TypeName = "date")]
        [Index("user_id_date", 2, IsUnique = true)]
        [Required]
        public DateTime date { get; set; }

        [ForeignKey("user_id")]
        public User user { get; set; }
    }
}
