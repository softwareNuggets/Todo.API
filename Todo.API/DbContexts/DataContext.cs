global using Microsoft.EntityFrameworkCore;
global using Todo.API.Entities;

namespace Todo.API.DbContexts
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
            
        }

        //dbContext teaches EntityFrameWork about our database through a collection of DbSets
        //DbSet is a collection of list that represents database tables or views.
        public DbSet<StatusCode> StatusCodes { get; set; } = null!;
        public DbSet<UserTask> UserTasks { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;

    }
}
