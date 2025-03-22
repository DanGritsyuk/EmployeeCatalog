using EmployeeCatalog.Common.Entities;
using EmployeeCatalog.DAL.Repository.Contracts;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EmployeeCatalog.BLL.Logic.Commands
{
    public class FilterEmployeesCommand : ICommandWithArgsAndResult<EmployeeFilterCriteria, IEnumerable<Employee>>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public FilterEmployeesCommand(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Employee>> ExecuteAsync(EmployeeFilterCriteria criteria)
        {
            var filter = Builders<Employee>.Filter.And(
                Builders<Employee>.Filter.Eq(e => e.Gender, criteria.Gender),
                Builders<Employee>.Filter.Regex(e => e.FullName, new BsonRegularExpression($"^{criteria.StartsWithLetter}"))
            );

            return await _employeeRepository.GetByFilterAsync(filter);
        }
    }
}
