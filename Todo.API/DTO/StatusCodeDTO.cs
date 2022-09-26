using System;

namespace Todo.API.Entities
{
    public class StatusCodeDTO
    {
        
        public string Code { get; set; }

        public string Description { get; set; }

        // add this empty constructor to get rid of InvalidDataContractException
        public StatusCodeDTO()
        {

        }

        public StatusCodeDTO(string code, string description)
        {
            this.Code = code;
            this.Description = description;
        }
    }
}
