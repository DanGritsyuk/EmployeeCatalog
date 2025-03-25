using EmployeeCatalog.BLL.Logic.Contracts;
using EmployeeCatalog.DAL.Repository.Contracts;

namespace EmployeeCatalog.BLL.Logic.Commands
{
    public class CreateTableCommand : ICreateTableCommand
    {
        private readonly IEmployeeRepository _employeeRepository;

        public CreateTableCommand(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task CreateTableAsync() =>
            await _employeeRepository.CreateTableAsync();
    }
}
