using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Todo.API.DbContexts;
using Todo.API.Entities;
using Todo.API.Services;
using Xunit;

namespace Todo.Api.UnitTest
{
    public  class TestEmployeeRepository
    {
        /*
            truncate table employees
              
              
            insert into employees(firstname, lastname, isactive)
            values
            ('software', 'nuggets', 'true'),
            ('someone', 'smart', 'true'),
            ('abscent', 'minded', 'true')

            select * from employees
        */

        private EmployeeRepository? employeeRepository = null;


        [Fact]
        public async void TestGetByType()
        {
            

            var optionBuilder = new DbContextOptionsBuilder<DataContext>();
            optionBuilder.UseSqlServer(GetConnectionString())
                .EnableSensitiveDataLogging(true);
            Todo.API.DbContexts.DataContext context = new DataContext(optionBuilder.Options);

            employeeRepository = new EmployeeRepository(context);

            var output = await employeeRepository.GetEmployeeAsync(1);

            Assert.NotNull(output);

            if (output != null)
                Assert.Equal("software", output.FirstName.ToString());
        }

        
        public async void TestInsertMethod()
        {
            var optionBuilder = new DbContextOptionsBuilder<DataContext>();
            optionBuilder.UseSqlServer(GetConnectionString())
                .EnableSensitiveDataLogging(true);
            Todo.API.DbContexts.DataContext context = new DataContext(optionBuilder.Options);

            employeeRepository = new EmployeeRepository(context);

            Employee ins = new Employee();
            ins.FirstName = "test";
            ins.LastName = "insert";
            ins.IsActive = true;

            var output = await employeeRepository.InsertEmployeeAsync(ins);

            if (output != null)
            {
                Assert.Equal("insert", output.LastName.ToString());
            }
        }


        
        public void TestUpdateMethod()
        {

            var optionBuilder = new DbContextOptionsBuilder<DataContext>();
            optionBuilder.UseSqlServer(GetConnectionString())
                .EnableSensitiveDataLogging(true);
            Todo.API.DbContexts.DataContext context = new DataContext(optionBuilder.Options);

            employeeRepository = new EmployeeRepository(context);

            Employee upd = new Employee();
            upd.EmployeeId = 4;
            upd.FirstName = "test";
            upd.LastName = "update";
            upd.IsActive = true;

            var output = employeeRepository.UpdateEmployee(upd);

            Assert.NotNull(output);

            if (output != null)
                Assert.Equal("update", output.LastName.ToString());

        }

        
        public void TestRemoveMethod()
        {
            var optionBuilder = new DbContextOptionsBuilder<DataContext>();
            optionBuilder.UseSqlServer(GetConnectionString())
                .EnableSensitiveDataLogging(true);
            Todo.API.DbContexts.DataContext context = new DataContext(optionBuilder.Options);

            employeeRepository = new EmployeeRepository(context);

            Employee upd = new Employee();
            upd.EmployeeId = 4;
            upd.FirstName = "test";
            upd.LastName = "update";
            upd.IsActive = false;

            var output = employeeRepository.SoftDeleteEmployee(upd);


            //Assert.Equal(false, output.IsActive);
        }






        public string GetConnectionString()
        {
            return ("server=SCOTTWIN10-2\\SQLHOME; Database=TaskManager; MultipleActiveResultSets=true; user=sa;password=secret_password");
        }
    }
}
