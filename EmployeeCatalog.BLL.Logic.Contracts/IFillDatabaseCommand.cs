namespace EmployeeCatalog.BLL.Logic.Contracts
{
    public interface IFillDatabaseCommand<T> where T : class
    {
        Task BulkInsertAsync(T employeeBatches);
    }
}
