namespace Todo.API.Services
{
    public interface IUserTaskRepository
    {
        Task<IEnumerable<UserTask>> GetAllActiveUserTaskAsync();
        Task<UserTask?> GetUserTaskAsync(int task_id);
        Task<IEnumerable<UserTask>> GetAllActiveTaskByEmployeeAsync(int assignedTo);

        Task<UserTask?> InsertUserTaskAsync(UserTask userTask);
        UserTask? UpdateUserTask(UserTask userTask);
        UserTask? RemoveUserTask(UserTask userTask);
    }
}
