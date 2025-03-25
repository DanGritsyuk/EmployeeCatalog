using EmployeeCatalog.BLL.Logic.Commands;
using EmployeeCatalog.ConsoleApp.Services;
using EmployeeCatalog.DAL.Repository.Contracts;
using EmployeeCatalog.DAL.Repository;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using EmployeeCatalog.BLL.Logic.Contracts;
using EmployeeCatalog.Common.Entities;

namespace EmployeeCatalog.ConsoleApp.Extensions
{
    internal static class DIExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, string[] args)
        {
            

            // Парсинг аргументов командной строки
            Dictionary<string, string> arguments = CommandLineArgumentsParser.Parse(args);
            services.AddSingleton(arguments);

            // Настройка MongoDB
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            var mongoClient = new MongoClient("mongodb://root:example@localhost:27017/?authSource=admin");
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase("EmployeeCatalog");

            // Регистрация команд
            services.AddTransient<ICreateTableCommand, CreateTableCommand>();
            services.AddTransient<IAddEmployeeCommand<EmployeeInputModel>, AddEmployeeCommand>();
            services.AddTransient<IListEmployeesCommand<IAsyncEnumerable<Employee>>, ListEmployeesCommand>();
            services.AddTransient<IFillDatabaseCommand<IAsyncEnumerable<IEnumerable<Employee>>>, FillDatabaseCommand>();
            services.AddTransient<IFilterEmployeesCommand<EmployeeFilterCriteria, IEnumerable<Employee>>, FilterEmployeesCommand>();

            // Регистрация основных узлов
            services.AddSingleton<Application>();
            services.AddSingleton(mongoDatabase);
            services.AddSingleton<IEmployeeRepository, EmployeeRepository>();

            return services;
        }
    }
}
