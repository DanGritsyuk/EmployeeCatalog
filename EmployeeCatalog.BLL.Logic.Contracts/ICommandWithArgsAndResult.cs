namespace EmployeeCatalog.BLL.Logic.Contracts
{
    public interface ICommandWithArgsAndResult<TInput, TResult>
    {
        Task<TResult> ExecuteAsync(TInput input);
    }
}
