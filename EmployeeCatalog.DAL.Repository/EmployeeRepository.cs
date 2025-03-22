using EmployeeCatalog.Common.Entities;
using EmployeeCatalog.Common.Entities.Enums;
using EmployeeCatalog.DAL.Repository.Contracts;
using MongoDB.Driver;

namespace EmployeeCatalog.DAL.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(100);

        private readonly IMongoCollection<Employee> _employees;
        private readonly IMongoClient _client;

        public EmployeeRepository(IMongoDatabase database)
        {
            _employees = database.GetCollection<Employee>("Employees");
            _client = database.Client;
        }

        public async Task CreateTableAsync()
        {
            var collections = await (await _client
                .GetDatabase(_employees.Database.DatabaseNamespace.DatabaseName)
                .ListCollectionNamesAsync())
                .ToListAsync();

            if (!collections.Contains("Employees"))
            {
                await _employees.Database.CreateCollectionAsync("Employees");
                Console.WriteLine("Collection 'Employees' created.");

                var compoundIndexKeys = Builders<Employee>.IndexKeys
                    .Ascending(e => e.Gender)
                    .Ascending(e => e.FullName);

                await _employees.Indexes.CreateOneAsync(new CreateIndexModel<Employee>(compoundIndexKeys));

                Console.WriteLine("Compound index created on 'Gender' and 'FullName' fields.");
            }
            else
            {
                Console.WriteLine("Collection 'Employees' already exists.");
            }
        }

        public async Task<Employee?> GetByIdAsync(Guid employeeId)
        {
            try
            {
                return await _employees.Find(u => u.Id == employeeId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to retrieve employee by ID.", ex);
            }
        }

        public async Task<IEnumerable<Employee>> GetByFilterAsync(FilterDefinition<Employee> filter, SortDefinition<Employee> sort = null)
        {
            try
            {
                var projection = Builders<Employee>.Projection
                    .Include(e => e.FullName)
                    .Include(e => e.BirthDate);

                var query = _employees.Find(filter).Project<Employee>(projection);
                if (sort != null)
                {
                    query = query.Sort(sort);
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to retrieve employees by filter.", ex);
            }
        }

        public async IAsyncEnumerable<Employee> GetAllStreamingAsync()
        {
            using var cursor = await _employees.Find(_ => true).ToCursorAsync();
            while (await cursor.MoveNextAsync())
            {
                foreach (var employee in cursor.Current)
                {
                    yield return employee;
                }
            }
        }

        public async Task BulkInsertAsync(IEnumerable<Employee> employees)
        {
            if (employees == null || employees.Count() == 0) return;
            await ExecuteInTransactionAsync(employees, (session, employees) =>
            _employees.InsertManyAsync(session, employees));
        }

        public async Task AddEmployeeWithEventAsync(Employee employee) =>
            await ExecuteInTransactionAsync(employee, ChangeEventType.Created, (session) =>
                _employees.InsertOneAsync(session, employee));

        public async Task UpdateEmployeeWithEventAsync(Employee employee) =>
            await ExecuteInTransactionAsync(employee, ChangeEventType.Updated, (session) =>
                _employees.ReplaceOneAsync(session, u => u.Id == employee.Id, employee));

        public async Task DeleteEmployeeWithEventAsync(Employee employee) =>
            await ExecuteInTransactionAsync(employee, ChangeEventType.Deleted, (session) =>
                _employees.DeleteOneAsync(session, u => u.Id == employee.Id));

        private async Task ExecuteInTransactionAsync(Employee employee, ChangeEventType eventType, Func<IClientSessionHandle, Task> operation)
        {
            using var session = await _client.StartSessionAsync();
            session.StartTransaction();

            try
            {
                await operation(session);
                await session.CommitTransactionAsync();
            }
            catch
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }

        private async Task ExecuteInTransactionAsync<T>(IEnumerable<T> items, Func<IClientSessionHandle, IEnumerable<T>, Task> operation)
        {
            using var session = await _client.StartSessionAsync();
            await _semaphore.WaitAsync();
            session.StartTransaction();

            try
            {
                await operation(session, items);
                await session.CommitTransactionAsync();
            }
            catch
            {
                await session.AbortTransactionAsync();
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
