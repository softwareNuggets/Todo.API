using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Todo.API.DbContexts;
using Todo.API.Entities;
using Todo.API.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Todo.Api.UnitTest
{
    public class TestUserTaskRepository
    {
        private UserTaskRepository? userTaskRepository = null;

        
        public async void TestGetAllActiveUserTaskAsync()
        {
            /*
                truncate table usertasks
             
                insert into UserTasks(Title, DueDate, AssignedTo, StatusCode, DateCompleted)
                values
                ('Write Test for UserTaks', '5/4/2022', 1, 'CR8', null),
                ('Write Test for UserTaks', '5/5/2022', 1, 'CR8', null);
            */

            var optionBuilder = new DbContextOptionsBuilder<DataContext>();
            optionBuilder.UseSqlServer(GetConnectionString())
                .EnableSensitiveDataLogging(true);
            Todo.API.DbContexts.DataContext context = new DataContext(optionBuilder.Options);

            userTaskRepository = new UserTaskRepository(context);

            var output = await userTaskRepository.GetAllActiveUserTaskAsync() as IEnumerable<UserTask>;

            if (output.Count() > 0)
            {
                foreach(UserTask test in output)
                {
                    Assert.NotEqual("COM", test.StatusCode);
                }
            }
        }

        
        public void TestUpdateUserTask()
        {
            /*
                truncate table usertasks
             
                insert into UserTasks(Title, DueDate, AssignedTo, StatusCode, DateCompleted)
                values
                ('Write Test for UserTask', '5/3/2022', 2, 'COM', null),
                ('Write Test for UserTask', '5/4/2022', 1, 'CR8', null),
                ('Write Test for UserTask', '5/5/2022', 1, 'CR8', null);
            */

            UserTask upd = new UserTask();
            upd.TaskId = 1;
            upd.Title = "Write Test for UserTask";
            upd.DueDate = System.DateTime.Parse("5/3/2022");
            upd.StatusCode = "COM";
            upd.DateCompleted = System.DateTime.Parse("5/5/2022");


            var optionBuilder = new DbContextOptionsBuilder<DataContext>();
            optionBuilder.UseSqlServer(GetConnectionString())
                .EnableSensitiveDataLogging(true);
            Todo.API.DbContexts.DataContext context = new DataContext(optionBuilder.Options);

            userTaskRepository = new UserTaskRepository(context);

            var output = userTaskRepository.UpdateUserTask(upd) as UserTask;

            if(output != null)
                Assert.Equal("COM", output.StatusCode);
        }

        
        public async void TestInsertUserTaskAsync()
        {
            /*
                truncate table usertasks
             
                insert into UserTasks(Title, DueDate, AssignedTo, StatusCode, DateCompleted)
                values
                ('Write Test for UserTask', '5/3/2022', 2, 'COM', null),
                ('Write Test for UserTask', '5/4/2022', 1, 'CR8', null),
                ('Write Test for UserTask', '5/5/2022', 1, 'CR8', null);
            */

            UserTask ins = new UserTask();
            ins.Title = "xUnit insert test";
            ins.DueDate = System.DateTime.Parse("5/1/2022");
            ins.StatusCode = "CR8";
            ins.AssignedTo = 1;


            var optionBuilder = new DbContextOptionsBuilder<DataContext>();
            optionBuilder.UseSqlServer(GetConnectionString())
                .EnableSensitiveDataLogging(true);
            Todo.API.DbContexts.DataContext context = new DataContext(optionBuilder.Options);

            userTaskRepository = new UserTaskRepository(context);

            var output = await userTaskRepository.InsertUserTaskAsync(ins) as UserTask;

            if (output != null)
                Assert.Equal("xUnit insert test", output.Title);
        }




        public string GetConnectionString()
        {
            return ("server=SCOTTWIN10-2\\SQLHOME; Database=TaskManager; MultipleActiveResultSets=true; user=sa;password=a12qrt?");
        }
    }
}
