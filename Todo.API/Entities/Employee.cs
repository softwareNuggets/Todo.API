using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Todo.API.Entities
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(32)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(32)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }


    }
}
