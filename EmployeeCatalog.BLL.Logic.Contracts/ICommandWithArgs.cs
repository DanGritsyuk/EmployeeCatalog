namespace EmployeeCatalog.BLL.Logic.Contracts
{
    public interface ICommandWithArgs<T> where T : class
    {
        Task ExecuteAsync(T inputModel);
    }
}