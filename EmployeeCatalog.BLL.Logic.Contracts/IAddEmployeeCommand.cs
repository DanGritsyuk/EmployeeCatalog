namespace EmployeeCatalog.BLL.Logic.Contracts
{
    public interface IAddEmployeeCommand<T> where T : class
    {
        Task AddEmployeeWithEventAsync(T inputModel);
    }
}