using EmployeeCatalog.ConsoleApp;
using Microsoft.Extensions.DependencyInjection;
using EmployeeCatalog.ConsoleApp.Extensions;

var services = new ServiceCollection();
services.ConfigureServices(args);
using var serviceProvider = services.BuildServiceProvider();

try
{
    var app = serviceProvider.GetRequiredService<Application>();
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex}");
}