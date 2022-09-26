using Todo.API.Entities;

namespace Todo.API.Entities
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }

        public EmployeeDTO()
        {

        }

        public EmployeeDTO(
            int employeeId, string fname,             
            string lname,   bool isActive)
        {
            EmployeeId  = employeeId;
            FullName    = $"{fname} {lname}";
            IsActive    = isActive;
        }
    }
}
