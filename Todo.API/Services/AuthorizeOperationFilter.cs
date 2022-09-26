using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Todo.API.Services
{
    public class AuthorizeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // discover which methods have been decorated with the attribute [Authorize]

            // filter on [Authorize] attribute
            var authAttributes = context.MethodInfo
                       .GetCustomAttributes(true)
                       .OfType<AuthorizeAttribute>()
                       .Distinct();

            if (authAttributes.Any())
            {
                // add possible responses to swagger docs
                //operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                //operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                // enable swagger to send auth header token
                operation.Security = new List<OpenApiSecurityRequirement>();

                var secReq = new List<OpenApiSecurityRequirement>();


                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Description = "Adds token to header",
                            Name = "Authorization",
                            Type = SecuritySchemeType.Http,
                            In = ParameterLocation.Header,
                            Scheme = JwtBearerDefaults.AuthenticationScheme,
                            Reference = new OpenApiReference 
                            { 
                                Type = ReferenceType.SecurityScheme, 
                                Id = JwtBearerDefaults.AuthenticationScheme 
                            }
                        }, new List<string>()
                    }
                });
            }
        }
    }
}
