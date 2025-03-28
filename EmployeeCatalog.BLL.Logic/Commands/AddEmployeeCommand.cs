﻿using EmployeeCatalog.BLL.Logic.Contracts;
using EmployeeCatalog.BLL.Logic.Mapping;
using EmployeeCatalog.Common.Entities;
using EmployeeCatalog.DAL.Repository.Contracts;

namespace EmployeeCatalog.BLL.Logic.Commands
{
    public class AddEmployeeCommand : IAddEmployeeCommand<EmployeeInputModel>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public AddEmployeeCommand(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task AddEmployeeWithEventAsync(EmployeeInputModel inputModel)
        {
            var employee = new Employee();
            employee.Map(inputModel);
            await _employeeRepository.AddEmployeeWithEventAsync(employee);
        }
    }
}
