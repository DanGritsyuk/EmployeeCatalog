namespace EmployeeCatalog.BLL.Logic.Contracts
{
    public interface IListEmployeesCommand<TResult>
    {
        Task<TResult> GetAllStreamingAsync();
    }
}
