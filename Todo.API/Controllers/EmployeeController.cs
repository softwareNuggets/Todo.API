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
    //[Consumes("application/json")]
    //[Produces("application/json")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository ??
                throw new ArgumentNullException(nameof(employeeRepository));
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EmployeeDTO>))]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeeAsync();

            var results = new List<EmployeeDTO>();

            foreach (var entry in employees)
            {
                results.Add(new EmployeeDTO(
                    entry.EmployeeId, entry.FirstName, entry.LastName, entry.IsActive));
            }

            return Ok(results);
        }






        [HttpGet("GetEmployee/{emp_id}")]
        //[ProducesResponseType(200, Type = typeof(EmployeeDTO))]
        //[ProducesResponseType(404)]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeAsync(int emp_id)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(emp_id);

            if (employee == null)
                return NotFound();

            var result = ConvertFromEntity(employee);

            return Ok(result);
        }









        [HttpPost("InsertEmployeeAsync")]
        [Authorize]
        public async Task<ActionResult<EmployeeDTO>> InsertNewEmployeeAsync(EmployeeDTO empDTO)
        {
            Employee rec = ConvertFromDTO(empDTO);

            var empResults = await _employeeRepository.InsertEmployeeAsync(rec);

            if (empResults != null)
            {
                return Created($"~/api/Employee/GetStatusCode/{empResults.EmployeeId}",
                    empResults);
            }

            return BadRequest(empDTO);

        }






        [HttpPut("UpdateEmployee")]
        public ActionResult<EmployeeDTO?> UpdateEmployee(EmployeeDTO empDTO)
        {
            Employee entry = ConvertFromDTO(empDTO);
            var Result = _employeeRepository.UpdateEmployee(entry);

            if (Result != null)
            {
                return Ok(Result);
            }

            return BadRequest(empDTO);
        }



        [HttpPut("/PutEmployee/{employeeId}")]
        public ActionResult<EmployeeDTO?> PutEmployee(int employeeId,EmployeeDTO empDTO)
        {
            if(employeeId != empDTO.EmployeeId)
            {
                return BadRequest();
            }

            Employee emp = ConvertFromDTO(empDTO);
            var result = _employeeRepository.PutEmployee(emp);
            if (result != null)
                return Ok(result);
            else
                return BadRequest(emp);

        }




        [HttpDelete("DeleteEmployee")]
        public ActionResult<EmployeeDTO?> DeleteEmployee(EmployeeDTO empDTO)
        {
            Employee entry = ConvertFromDTO(empDTO);
            var Result = _employeeRepository.SoftDeleteEmployee(entry);

            if (Result != null)
            {
                return Ok(Result);
            }

            return BadRequest(empDTO);

        }





        private EmployeeDTO ConvertFromEntity(Employee input)
        {
            var result = new EmployeeDTO(   input.EmployeeId,   input.FirstName,
                                            input.LastName,     input.IsActive);
            return (result);
        }

        private Employee ConvertFromDTO(EmployeeDTO input)
        {
            var names = SplitIntoFN_and_LN(input.FullName);

            var result = new Employee();

            result.EmployeeId = input.EmployeeId;

            switch (names.Length)
            {
                case 2:
                    {
                        result.FirstName = names[0];
                        result.LastName = names[1];
                    }
                    break;
                case 1:
                    {
                        result.FirstName = names[0];
                        result.LastName = "";
                    }
                    break;
                default:
                    {
                        result.FirstName = "";
                        result.LastName = "";
                    }
                    break;
            }

            result.IsActive = input.IsActive;

            return (result);
        }

        private string[] SplitIntoFN_and_LN(string fullName)
        {
            char[] sep = { ' ' };
            var buckets = fullName.Split(sep, StringSplitOptions.RemoveEmptyEntries);

            return (buckets);
            
        }

        private List<EmployeeDTO> ConvertFromList(List<Employee> input)
        {
            var results = new List<EmployeeDTO>();
            foreach (var entry in input)
            {
                results.Add(ConvertFromEntity(entry));
            }

            return results;
        }
    }
}
