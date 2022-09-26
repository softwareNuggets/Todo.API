namespace Todo.API.Entities
{
    public class EmployeeTasks
    {
        public int EmployeeId { get; set; }
        public int FirstName { get; set; }
        public string LastName { get; set; }    
        public int TaskId { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public string StatusCode { get; set; }
    }
}
