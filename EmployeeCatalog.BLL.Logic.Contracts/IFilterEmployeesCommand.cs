namespace EmployeeCatalog.BLL.Logic.Commands
{
    public interface IFilterEmployeesCommand<TInput, TResult>
    {
        Task<TResult> GetByFilterAsync(TInput input);
    }
}
