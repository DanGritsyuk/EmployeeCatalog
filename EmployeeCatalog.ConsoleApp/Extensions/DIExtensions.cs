using EmployeeCatalog.BLL.Logic.Commands;
using EmployeeCatalog.ConsoleApp.Services;
using EmployeeCatalog.DAL.Repository.Contracts;
using EmployeeCatalog.DAL.Repository;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;

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
            services.AddTransient<CreateTableCommand>();
            services.AddTransient<AddEmployeeCommand>();
            services.AddTransient<ListEmployeesCommand>();
            services.AddTransient<FillDatabaseCommand>();
            services.AddTransient<FilterEmployeesCommand>();

            // Регистрация основных узлов
            services.AddSingleton<Application>();
            services.AddSingleton(mongoDatabase);
            services.AddSingleton<IEmployeeRepository, EmployeeRepository>();

            return services;
        }
    }
}
