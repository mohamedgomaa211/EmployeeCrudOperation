using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeCrud.Models
{
    public class Employee
    {
        [Key]
        public int EmpoyeeID { get; set; }
        [Required(ErrorMessage ="This Field is Required-you must Enter EmpName")]
        [Column(TypeName ="nvarchar(250)")]
        [DisplayName("Full Name")]
        public string FullName { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Address { get; set; }
        public DateTime Brirhday { get; set; }

    }
}
