using EmployeeCatalog.DAL.Repository.Contracts;
using EmployeeCatalog.BLL.Logic.Contracts;
using EmployeeCatalog.Common.Entities;

namespace EmployeeCatalog.BLL.Logic.Commands
{
    public class FillDatabaseCommand : IFillDatabaseCommand<IAsyncEnumerable<IEnumerable<Employee>>>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public FillDatabaseCommand(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task BulkInsertAsync(IAsyncEnumerable<IEnumerable<Employee>> employeeBatches)
        {
            Console.WriteLine("Start filling the database...");

            var tasks = new List<Task>();

            try
            {
                await foreach (var batch in employeeBatches)
                {
                    var task = _employeeRepository.BulkInsertAsync(batch);
                    tasks.Add(task);
                }

                await Task.WhenAll(tasks);

                Console.WriteLine("Database filled successfully.");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
