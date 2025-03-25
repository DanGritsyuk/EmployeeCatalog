using EmployeeCatalog.BLL.Logic.Commands;
using EmployeeCatalog.BLL.Logic.Contracts;
using EmployeeCatalog.Common.Entities;
using EmployeeCatalog.Common.Entities.Enums;
using EmployeeCatalog.ConsoleApp.Services;
using MongoDB.Driver;
using System.Diagnostics;

namespace EmployeeCatalog.ConsoleApp
{
    public class Application
    {
        private readonly IDictionary<string, string> _arguments;
        private readonly ICreateTableCommand _createTableCommand;
        private readonly IAddEmployeeCommand<EmployeeInputModel> _addEmployeeCommand;
        private readonly IListEmployeesCommand<IAsyncEnumerable<Employee>> _listEmployeesCommand;
        private readonly IFillDatabaseCommand<IAsyncEnumerable<IEnumerable<Employee>>> _fillDatabaseCommand;
        private readonly IFilterEmployeesCommand<EmployeeFilterCriteria, IEnumerable<Employee>> _filterEmployeesCommand;

        public Application(
            IDictionary<string, string> arguments,
            ICreateTableCommand createTableCommand,
            IAddEmployeeCommand<EmployeeInputModel> addEmployeeCommand,
            IListEmployeesCommand<IAsyncEnumerable<Employee>> listEmployeesCommand,
            IFillDatabaseCommand<IAsyncEnumerable<IEnumerable<Employee>>> fillDatabaseCommand,
            IFilterEmployeesCommand<EmployeeFilterCriteria, IEnumerable<Employee>> filterEmployeesCommand)
        {
            _arguments = arguments;
            _createTableCommand = createTableCommand;
            _addEmployeeCommand = addEmployeeCommand;
            _listEmployeesCommand = listEmployeesCommand;
            _fillDatabaseCommand = fillDatabaseCommand;
            _filterEmployeesCommand = filterEmployeesCommand;
        }


        /// <summary>
        /// Запуск приложения.
        /// </summary>
        public async Task RunAsync()
        {
            try
            {
                if (_arguments.ContainsKey("mode"))
                {
                    byte mode = byte.Parse(_arguments["mode"]);

                    switch (mode)
                    {
                        case 1:
                            await RunCreateTable(); // Создание таблицы
                            break;

                        case 2:
                            await RunAddEmployee(); // Добавление сотрудника
                            break;

                        case 3:
                            await RunListEmployees(); // Вывод всех сотрудников
                            break;

                        case 4:
                            await RunBulkInsertEmployees(); // Массовое добавление сотрудников
                            break;
                        case 5:
                            await RunFilterEmployees(); // Вывод по фильтру
                            break;

                        default:
                            throw new ArgumentException("Unknown mode.");
                    }
                }
                else
                {
                    throw new ArgumentException("Mode parameter is required.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed run application.", ex);
            }
        }

        /// <summary>
        /// Режим 1: Создание таблицы.
        /// </summary>
        private async Task RunCreateTable()
        {
            await _createTableCommand.CreateTableAsync();
            Console.WriteLine("Command 'create table' executed.");
        }

        /// <summary>
        /// Режим 2: Добавление сотрудника.
        /// </summary>
        private async Task RunAddEmployee()
        {
            if (!_arguments.ContainsKey("fullName") || !_arguments.ContainsKey("gender") || !_arguments.ContainsKey("birthDate"))
            {
                throw new ArgumentException("Parameters 'fullName', 'gender', and 'birthDate' are required for mode 2.");
            }

            var inputModel = new EmployeeInputModel()
            {
                FullName = _arguments["fullName"],
                Gender = _arguments["gender"],
                BirthDate = _arguments["birthDate"]
            };

            await _addEmployeeCommand.AddEmployeeWithEventAsync(inputModel);
        }

        /// <summary>
        /// Режим 3: Вывод всех сотрудников.
        /// </summary>
        private async Task RunListEmployees()
        {
            IAsyncEnumerable<Employee> employeesStream = await _listEmployeesCommand.GetAllStreamingAsync();

            await foreach (var employee in employeesStream)
                PrintEmployee(employee);
        }

        /// <summary>
        /// Режим 4: Массовое добавление сотрудников.
        /// </summary>
        private async Task RunBulkInsertEmployees()
        {
            if (!_arguments.ContainsKey("count"))
            {
                throw new ArgumentException("Parameter 'count' is required for mode 4.");
            }

            int count;
            if (!int.TryParse(_arguments["count"], out count) || count <= 0)
            {
                throw new ArgumentException("The 'count' parameter must be a positive integer.");
            }

            var dataGenerator = new EmployeeDataGenerator();

            await _fillDatabaseCommand.BulkInsertAsync(dataGenerator.GenerateEmployees(count, 100));
        }

        /// <summary>
        /// Режим 5: Выборка по критерию: пол, буква с которой фамилия начинается.
        /// </summary>
        private async Task RunFilterEmployees()
        {
            if (!_arguments.ContainsKey("letter") || !_arguments.ContainsKey("gender"))
            {
                throw new ArgumentException("Parameters 'letter' and 'gender' are required for mode 5.");
            }

            var letter = _arguments["letter"];
            var gender = Enum.Parse<Gender>(_arguments["gender"]);

            var criteria = new EmployeeFilterCriteria
            {
                StartsWithLetter = letter,
                Gender = gender
            };

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var employees = await _filterEmployeesCommand.GetByFilterAsync(criteria);

            stopwatch.Stop();

            //foreach (var employee in employees)
            //    PrintEmployee(employee);

            Console.WriteLine($"Found {employees.Count()} employees matching the criteria.");
            Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");
        }

        private void PrintEmployee(Employee employee)
        {
            Console.WriteLine($"ID: {employee.Id}");
            Console.WriteLine($"Full Name: {employee.FullName}");
            Console.WriteLine($"Gender: {employee.Gender}");
            Console.WriteLine($"Birth Date: {employee.BirthDate}");
            Console.WriteLine($"Age: {employee.GetAge()}");
            Console.WriteLine(new string('-', 30));
        }
        }
}