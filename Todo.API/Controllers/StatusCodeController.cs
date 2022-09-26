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
    public class StatusCodeController : ControllerBase
    {
        private readonly IStatusCodeRepository _statusCodeRepository;

        public StatusCodeController(IStatusCodeRepository statusCodeRepository)
        {
            _statusCodeRepository = statusCodeRepository ??
                throw new ArgumentNullException(nameof(statusCodeRepository));
        }

        
        [HttpGet]
        //[Authorize]

        public async Task<ActionResult<IEnumerable<StatusCodeDTO>>> GetAllAsync()
        {
            var statusCodes = await _statusCodeRepository.GetAllStatusCodesAsync();

            var results = new List<StatusCodeDTO>();

            foreach (var entry in statusCodes)
            {

                results.Add(new StatusCodeDTO(entry.Code, entry.Description));
            }

            return Ok(results);

        }





        //[HttpGet("api/StatusCode/GetStatusCode/{code}")]
        [HttpGet("GetStatusCodeAsync/{code}")]
        
        public async Task<ActionResult<StatusCodeDTO>> GetStatusCodeAsync(string code)
        {
            var statusCode = await _statusCodeRepository.GetStatusCodeAsync(code);

            if (statusCode == null)
                return NotFound();


            var result = new StatusCodeDTO(statusCode.Code, statusCode.Description);


            return Ok(result);

        }





        [HttpPost("InsertNewStatusCodeAsync")]
        public async Task<ActionResult<StatusCodeDTO>> InsertNewStatusCodeAsync(StatusCodeDTO statusCode)
        {
            StatusCode rec = new StatusCode();
            rec.Code = statusCode.Code;
            rec.Description = statusCode.Description;

            var statusCodeResults = await _statusCodeRepository.InsertStatusCodeAsync(rec);

            if (statusCodeResults != null)
            {
                return Created($"~/api/StatusCode/GetStatusCode/{rec.Code}", statusCode);
            }

            return BadRequest(statusCode);
        }















        [HttpPut("UpdateStatusCode")]
        public ActionResult<StatusCodeDTO?> UpdateStatusCode(StatusCodeDTO statusCode)
        {
            var entry = ConvertFromDTO(statusCode);
            var Result = _statusCodeRepository.UpdateStatusCode(entry);

            if (Result != null)
            {
                return Ok(statusCode);
            }

            return BadRequest(statusCode);
        }













        [HttpDelete("RemoveStatusCode")]
        public ActionResult<StatusCodeDTO?> RemoveStatusCode(StatusCodeDTO statusCode)
        {
            var entry = ConvertFromDTO(statusCode);
            var Result = _statusCodeRepository.RemoveStatusCode(entry);

            if (Result != null)
            {
                return Ok(statusCode);
            }

            return StatusCode(409, $"Status Code '{statusCode.Code}' does not exists.");
        }










        private StatusCodeDTO ConvertFromEntity(StatusCode input)
        {
            var result = new StatusCodeDTO(input.Code, input.Description);
            return (result);
        }


        private StatusCode ConvertFromDTO(StatusCodeDTO input)
        {
            var result = new StatusCode();
            result.Code = input.Code;
            result.Description = input.Description;

            return (result);
        }

        private List<StatusCodeDTO> ConvertFromList(List<StatusCode> input)
        {
            var results = new List<StatusCodeDTO>();
            foreach (var entry in input)
            {
                results.Add(ConvertFromEntity(entry));
            }

            return results;
        }
    }
}
