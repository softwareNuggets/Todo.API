using Todo.API.DbContexts;
using Todo.API.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Xunit;

namespace Todo.API.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {

        // we are making _context private, so can not access this outside our EmployeeRepository
        // we don't want to be able to change this object, so we make it readonly.
        // _context uses standard naming cenvention
        private readonly DataContext _context;


        public EmployeeRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        
        public  List<EmployeeTasks?> GetActiveTaskForEmployees()
        {
            var qry = from emp in _context.Employees
                        join tsk in _context.UserTasks on
                            emp.EmployeeId equals tsk.AssignedTo
                    where tsk.StatusCode == "CR8"
                    select new EmployeeTasks()
                    {
                        EmployeeId = emp.EmployeeId,
                        FirstName  = emp.EmployeeId,
                        LastName   = emp.LastName,
                        TaskId     = tsk.TaskId,
                        Title      = tsk.Title,
                        DueDate    = tsk.DueDate,
#pragma warning disable CS8601 // Possible null reference assignment.
                        StatusCode = tsk.StatusCode
#pragma warning restore CS8601 // Possible null reference assignment.
                    };

            var results = qry.ToList();

            if(qry != null)
                return qry.ToList();
            else
                return null;

            
        }


        [Fact]
        public async Task<IEnumerable<Employee>> GetAllEmployeeAsync()
        {
            var results = await _context.Employees
                            .Where(p => p.IsActive == true)
                            .OrderBy(p => p.FirstName)
                            .ToListAsync();

            return results;
        }

        public async Task<Employee?> GetEmployeeAsync(int empId)
        {
            var result = await _context.Employees
                                .Where(p => p.EmployeeId == empId)
                                .FirstOrDefaultAsync();

            return result;
        }

        
        public async Task<Employee?> InsertEmployeeAsync(Employee employee)
        {
            var entry = await _context.Employees
                                    .FirstOrDefaultAsync(p => p.FirstName == employee.FirstName &&
                                                              p.LastName == employee.LastName);
            if (entry == null)
            {
                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                return employee;
            }

            return null;
        }

        

        public Employee? SoftDeleteEmployee(Employee emp)
        {
            var employee = _context.Employees
                       .FirstOrDefault(p => p.EmployeeId == emp.EmployeeId);

            if (employee != null)
            {
                employee.IsActive = false;
                _context.SaveChanges();

                return employee;
            }

            return null;
        }

        public Employee? UpdateEmployee(Employee emp)
        {
            var employee = _context.Employees
                             .FirstOrDefault(p => p.EmployeeId == emp.EmployeeId);

            if (employee != null)
            {
                employee.FirstName = emp.FirstName;
                employee.LastName = emp.LastName;
                employee.IsActive = emp.IsActive;

                _context.SaveChanges();

                return employee;
            }

            return null;
        }


        public Employee? PutEmployee(Employee employee)
        {

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_context.Employees.Any(p => p.EmployeeId == employee.EmployeeId))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return employee;
        }
    }
}
