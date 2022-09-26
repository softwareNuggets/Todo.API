using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.API.Entities
{
    public class UserTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [Required]
        [MaxLength(64)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public DateTime DueDate { get; set; }
        public int? AssignedTo { get; set; }

        [Required]
        [MaxLength(3)]
        public string? StatusCode { get; set; }

        public DateTime? DateCompleted { get; set; }
    }
}
