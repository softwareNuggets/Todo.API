namespace Todo.API.Entities
{
    public class UserTaskDTO
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public int? AssignedTo { get; set; }
        public string StatusCode { get; set; }
        public DateTime? DateCompleted { get; set; }

        public UserTaskDTO()
        {

        }
        public UserTaskDTO(int taskId, string title,
                    DateTime dueDate, int? assignedTo,
                    string? statusCode, DateTime? dateCompleted)
        {
            this.TaskId = taskId;
            this.Title = title;
            this.DueDate = dueDate;
            this.AssignedTo = assignedTo;
            this.StatusCode = statusCode;
            this.DateCompleted = dateCompleted;
        }

    }


    

}
