﻿using EmployeeCatalog.BLL.Logic.Contracts;
using EmployeeCatalog.Common.Entities;
using EmployeeCatalog.DAL.Repository.Contracts;

namespace EmployeeCatalog.BLL.Logic.Commands
{
    public class ListEmployeesCommand : IListEmployeesCommand<IAsyncEnumerable<Employee>>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public ListEmployeesCommand(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IAsyncEnumerable<Employee>> GetAllStreamingAsync()
        {
            try
            {
                return _employeeRepository.GetAllStreamingAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while streaming employees: {ex.Message}");
                throw;
            }
        }
    }
}
