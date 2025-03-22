namespace EmployeeCatalog.BLL.Logic.Contracts
{
    public interface ICommandWithResult<TResult>
    {
        Task<TResult> ExecuteAsync();
    }
}
