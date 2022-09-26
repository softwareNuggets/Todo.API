namespace Todo.API.Services
{
    public interface IEmployeeRepository
    {
        List<EmployeeTasks?> GetActiveTaskForEmployees();

        Task<IEnumerable<Employee>> GetAllEmployeeAsync();
        Task<Employee?> GetEmployeeAsync(int empId);

        Task<Employee?> InsertEmployeeAsync(Employee employee);
        Employee? UpdateEmployee(Employee employee);
        Employee? SoftDeleteEmployee(Employee employee);

        //added 5/14/2022
        Employee? PutEmployee(Employee employee);
    }
}
