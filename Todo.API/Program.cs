using Microsoft.EntityFrameworkCore;
using Todo.API.DbContexts;
using System.Configuration;
using Todo.API.Services;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
    


//dependency injection
//everytime i need an IStatusCodeRepository, give me a StatusCodeRepository
//these are lifetime of service declarations
//AddScope = Scoped lifetime services are created once per request
builder.Services.AddScoped<IStatusCodeRepository, StatusCodeRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IUserTaskRepository, UserTaskRepository>();


//added to project to provide authenication
builder.Services
        .AddAuthentication("Bearer")
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Authentication:Issuer"],
                ValidAudience = builder.Configuration["Authentication:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(
                        builder.Configuration["Authentication:SecretKey"]))
            };
        });

// Add services to the container.
builder.Services
    .AddControllers()
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{

    //This provides additional compile-time safety while
    //using reference types and protects against possible
    //null reference exceptions.
    options.SupportNonNullableReferenceTypes();

    // apply auth to any API with the [Authorize] attribute
    // (see AuthorizeOperationFilter class for more info)
    options.OperationFilter<AuthorizeOperationFilter>();

    // add the top level Authorize button to swagger
    // uses Microsoft.OpenApi.Models
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    var info = new OpenApiInfo()
    {
        Version = "1",
        Title = "ToDo.API",
        Description = "Learn how to use ASP.NET Core Web API",
        Contact = new OpenApiContact() { Name = "Software Nuggets", Email = "??@domain.com" },
        License = new OpenApiLicense() { Name = "No license required" }
    };

    // register the swagger generator, defining 1 or more swagger documents
    options.SwaggerDoc("v1", info);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
// Properties -> Debug -> General
// ASPNETCORE_ENVIRONMENT=Development|Staging|Production
// if(app.Environment.IsStaging() || app.Environment.IsProduction())
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
