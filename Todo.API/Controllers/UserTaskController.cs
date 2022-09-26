using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo.API.DbContexts;
using Todo.API.Entities;
using Todo.API.Services;

namespace Todo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskController : ControllerBase
    {
        private readonly IUserTaskRepository _userTaskRepository;

        public UserTaskController(IUserTaskRepository userTaskRepository)
        {
            _userTaskRepository = userTaskRepository ??
                throw new ArgumentNullException(nameof(UserTaskRepository));
        }













        [HttpGet]
        [Authorize]

        public async Task<ActionResult<IEnumerable<UserTaskDTO>>> GetAllActiveUserTaskAsync()
        {
            var activeUserTasks = await _userTaskRepository.GetAllActiveUserTaskAsync();

            var results = new List<UserTaskDTO>();

            foreach (var entry in activeUserTasks)
            {

                results.Add(new UserTaskDTO(
                    entry.TaskId, entry.Title, entry.DueDate, 
                    entry.AssignedTo, entry.StatusCode, 
                    entry.DateCompleted));
            }

            return Ok(results);

        }







        [HttpGet("GetUserTaskAsync/{taskId}")]
        public async Task<ActionResult<UserTaskDTO>> GetUserTaskAsync(int taskId)
        {
            var userTask = await _userTaskRepository.GetUserTaskAsync(taskId);

            if (userTask == null)
                return NotFound();


            var result = ConvertFromEntity(userTask);


            return Ok(result);

        }





        
        [HttpGet("GetAllActiveTaskByEmployeeAsync/{assignedTo}")]
        public async Task<ActionResult<IEnumerable<UserTaskDTO>>> GetAllActiveTaskByEmployeeAsync(int assignedTo)
        {
            var userTask = await _userTaskRepository.GetAllActiveTaskByEmployeeAsync(assignedTo);

            if (userTask == null)
                return NotFound();

            var results = new List<UserTaskDTO>();

            foreach (var entry in userTask)
            {

                results.Add(new UserTaskDTO(
                    entry.TaskId, entry.Title, entry.DueDate,
                    entry.AssignedTo, entry.StatusCode,
                    entry.DateCompleted));
            }


            return Ok(results);

        }



        [HttpPost("InsertUserTaskAsync")]
        public async Task<ActionResult<UserTaskDTO>> InsertUserTaskAsync(UserTaskDTO userTask)
        {
            UserTask rec = this.ConvertFromDTO(userTask);

            var results = await _userTaskRepository.InsertUserTaskAsync(rec);

            if (results != null)
            {
                return Created($"~/api/userTask/GetUserTaskAsync/{rec.TaskId}", userTask);
            }

            return BadRequest(userTask);
        }










        [HttpPut("UpdateUserTask")]
        public ActionResult<UserTaskDTO?> UpdateUserTask(UserTaskDTO userTask)
        {
            var entry = ConvertFromDTO(userTask);
            var Result = _userTaskRepository.UpdateUserTask(entry);

            if (Result != null)
            {
                return Ok(userTask);
            }

            return BadRequest(userTask);
        }








        [HttpDelete("RemoveUserTask")]
        public ActionResult<UserTaskDTO?> RemoveUserTask(UserTaskDTO userTask)
        {
            var entry = ConvertFromDTO(userTask);
            var Result = _userTaskRepository.RemoveUserTask(entry);

            if (Result != null)
            {
                return Ok(userTask);
            }

            return StatusCode(409, $"User Task '{userTask.Title}' does not exists.");
        }









        private UserTask ConvertFromDTO(UserTaskDTO input)
        {
            var result = new UserTask();
            result.TaskId = input.TaskId;
            result.Title = input.Title;
            result.DueDate = input.DueDate;
            result.AssignedTo = input.AssignedTo;
            result.StatusCode = input.StatusCode;
            result.DateCompleted = input.DateCompleted;

            return (result);
        }

        private UserTaskDTO ConvertFromEntity(UserTask input)
        {
            var result = new UserTaskDTO(
                    input.TaskId, input.Title, input.DueDate,
                    input.AssignedTo, input.StatusCode,
                    input.DateCompleted);

            return (result);
        }

    }
}
