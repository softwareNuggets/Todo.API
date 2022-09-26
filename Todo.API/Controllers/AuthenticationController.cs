using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Todo.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        
        IConfiguration _configuration = null;

        public AuthenticationController(IConfiguration config)
        {
            _configuration = config ?? 
                throw new ArgumentNullException(nameof(config));
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody request)
        {
            //step1: validate the username/password
            var user = ValidateUserCredentials(
                request.UserName, request.Password);

            if(user == null)
            {
                return Unauthorized();
            }

            //Microsoft.IdentityModel.Tokens
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretKey"]));

            var signingCredentials = new SigningCredentials(
                securityKey,SecurityAlgorithms.HmacSha256);

            // the token will contain information about the user
            var claimsInfo = new List<Claim>();
            claimsInfo.Add(new Claim(ClaimTypes.GivenName, user.FirstName));
            claimsInfo.Add(new Claim(ClaimTypes.Surname, user.LastName));
            claimsInfo.Add(new Claim(ClaimTypes.Email, user.Email));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsInfo,
                DateTime.UtcNow,                //start time, cannot use token before this time
                DateTime.UtcNow.AddMinutes(20), //expired time, cannot use after this time
                signingCredentials);

            //view token at jwt.io or https://www.jstoolset.com/

            var userToken = new JwtSecurityTokenHandler()
                                    .WriteToken(jwtSecurityToken);

            return Ok(userToken);


        }

        private SiteUser ValidateUserCredentials(string? userName, string? password)
        {
            
            // pass the username and password to a database procedure
            // have the call query the database table tbl_users_m
            // have the return value contain, (id, loginName, firstName,lastName,email)

            // since we have not setup tbl_users_m, or, have written the
            // stored procedure, let's just imagine we have called that procedure
            // and the procedure returned
            //
            // SiteUser user = DbContext.tbl_users_m.where(p=>p.username==userName &&
            //                        p.password==password).FirstOrDefault();


            return new SiteUser(100, "LoginName", 
                "firstname", "lastname","username@domain.com");
        }
    }

    public class AuthenticationRequestBody
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }

    }

    public class SiteUser
    {
        public int Id { get; set; } 
        public string LoginName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public SiteUser(int id, string loginName, 
            string firstName, string lastName, string email)
        {
            this.Id = id;
            this.LoginName = loginName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
        }
    }

}
