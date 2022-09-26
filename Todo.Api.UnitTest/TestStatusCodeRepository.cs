using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Todo.API.DbContexts;
using Todo.API.Entities;
using Todo.API.Services;
using Xunit;

namespace Todo.Api.UnitTest
{
    public class TestStatusCodeRepository
    {

        private StatusCodeRepository? statusRepository = null;

        
        public async void TestGetByType()
        {
            var optionBuilder = new DbContextOptionsBuilder<DataContext>();
            optionBuilder.UseSqlServer(GetConnectionString())
                .EnableSensitiveDataLogging(true);
            Todo.API.DbContexts.DataContext context = new DataContext(optionBuilder.Options);

            statusRepository = new StatusCodeRepository(context);

            var output = await statusRepository.GetStatusCodeAsync("COM");

            Assert.NotNull(output);

            if (output != null)
                Assert.Equal("COM", output.Code.ToString());
        }

        
        public async void TestCodeNotInTable()
        {
            var optionBuilder = new DbContextOptionsBuilder<DataContext>();
            optionBuilder.UseSqlServer(GetConnectionString())
                .EnableSensitiveDataLogging(true);
            Todo.API.DbContexts.DataContext context = new DataContext(optionBuilder.Options);

            statusRepository = new StatusCodeRepository(context);

            var output = await statusRepository.GetStatusCodeAsync("AAA");

            Assert.Null(output);

            if (output != null)
                Assert.Equal("AAA", output.Code.ToString());
        }


        
        public async void TestInsertMethod()
        {
            var optionBuilder = new DbContextOptionsBuilder<DataContext>();
            optionBuilder.UseSqlServer(GetConnectionString())
                .EnableSensitiveDataLogging(true);
            Todo.API.DbContexts.DataContext context = new DataContext(optionBuilder.Options);

            statusRepository = new StatusCodeRepository(context);

            StatusCode upd = new StatusCode();
            upd.Code = "WR4";
            upd.Description = "Working";

            var output = await statusRepository.InsertStatusCodeAsync(upd);

            if (output != null)
            {
                Assert.Equal("Working", output.Description.ToString());
            }
        }

        
        public void TestUpdateMethod()
        {

            var optionBuilder = new DbContextOptionsBuilder<DataContext>();
            optionBuilder.UseSqlServer(GetConnectionString())
                .EnableSensitiveDataLogging(true);
            Todo.API.DbContexts.DataContext context = new DataContext(optionBuilder.Options);

            statusRepository = new StatusCodeRepository(context);

            StatusCode upd = new StatusCode();
            upd.Code = "WR4";
            upd.Description = "Software Nuggets";

            var output = statusRepository.UpdateStatusCode(upd);

            Assert.NotNull(output);

            if(output != null)
                Assert.Equal("Software Nuggets", output.Description.ToString());

        }

        
        public void TestRemoveMethod()
        {
            var optionBuilder = new DbContextOptionsBuilder<DataContext>();
            optionBuilder.UseSqlServer(GetConnectionString())
                .EnableSensitiveDataLogging(true);
            Todo.API.DbContexts.DataContext context = new DataContext(optionBuilder.Options);

            statusRepository = new StatusCodeRepository(context);

            StatusCode upd = new StatusCode();
            upd.Code = "WR4";
            upd.Description = "Software Nuggets";

            var output = statusRepository.RemoveStatusCode(upd);


            Assert.Equal("Working", output.Description.ToString());
        }



        public string GetConnectionString()
        {
            return ("server=SCOTTWIN10-2\\SQLHOME; Database=TaskManager; MultipleActiveResultSets=true; user=sa;password=a12qrt?");
        }

    }
}