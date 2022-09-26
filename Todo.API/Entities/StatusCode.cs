using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.API.Entities
{
    public class StatusCode
    {
        [Key]
        [MaxLength(3)]
        public string Code { get; set; }= string.Empty;

        [Required]
        [MaxLength(32)]
        public string Description { get; set; }= string.Empty;
    }
}
