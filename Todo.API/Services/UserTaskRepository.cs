using Todo.API.DbContexts;
using Todo.API.Entities;
using System.Linq;


namespace Todo.API.Services
{
    public class UserTaskRepository : IUserTaskRepository
    {

        private DataContext _context;

        public UserTaskRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<UserTask>> GetAllActiveUserTaskAsync()
        {
            var results = await _context.UserTasks
                                .Where(p => p.DateCompleted == null &&
                                       p.StatusCode != "COM")
                                .OrderBy(p => p.DueDate)
                                .ToListAsync();

            return results;
        }

 
        public async Task<IEnumerable<UserTask>> GetAllActiveTaskByEmployeeAsync(int assignedTo)
        {
            var results = await _context.UserTasks
                                .Where(p => p.DateCompleted == null &&
                                       p.StatusCode != "COM" &&
                                       p.AssignedTo == assignedTo)
                                .OrderBy(p => p.DueDate)
                                .ToListAsync();

            return results;
        }


        public async Task<UserTask?> GetUserTaskAsync(int task_id)
        {
            var result = await _context.UserTasks
                                .FirstOrDefaultAsync(p => p.TaskId == task_id);

            return result;
        }

        public async Task<UserTask?> InsertUserTaskAsync(UserTask userTask)
        {
            var entry = await _context.UserTasks
                    .FirstOrDefaultAsync(p => p.Title == userTask.Title &&
                                            p.DueDate == userTask.DueDate);
            if (entry == null)
            {
                await _context.UserTasks.AddAsync(userTask);
                await _context.SaveChangesAsync();
                return userTask;
            }

            return null;
        }




        public UserTask? RemoveUserTask(UserTask userTask)
        {
            var entry = _context.UserTasks.
                    FirstOrDefault(p => p.TaskId == userTask.TaskId);

            if (entry != null)
            {
                _context.UserTasks.Remove(entry);
                _context.SaveChanges();

                return entry;
            }

            return null;
        }









        public UserTask? UpdateUserTask(UserTask userTask)
        {
            var entry = _context.UserTasks.
                    FirstOrDefault(p => p.TaskId == userTask.TaskId);

            if (entry != null)
            {
                entry.DateCompleted = userTask.DateCompleted;
                entry.StatusCode = userTask.StatusCode;
                entry.Title = userTask.Title;
                entry.DueDate = userTask.DueDate;
                entry.AssignedTo = userTask.AssignedTo;
                _context.SaveChanges();

                return entry;
            }

            return null;
        }

    }
}
