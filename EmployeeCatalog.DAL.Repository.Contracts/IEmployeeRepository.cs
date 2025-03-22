using EmployeeCatalog.Common.Entities;
using MongoDB.Driver;

namespace EmployeeCatalog.DAL.Repository.Contracts
{
    public interface IEmployeeRepository
    {
        Task CreateTableAsync();
        Task<Employee?> GetByIdAsync(Guid employeeId);
        IAsyncEnumerable<Employee> GetAllStreamingAsync();
        Task<IEnumerable<Employee>> GetByFilterAsync(FilterDefinition<Employee> filter, SortDefinition<Employee> sort = null);
        Task AddEmployeeWithEventAsync(Employee employee);
        Task UpdateEmployeeWithEventAsync(Employee employee);
        Task DeleteEmployeeWithEventAsync(Employee employee);
        Task BulkInsertAsync(IEnumerable<Employee> employees);
    }
}
