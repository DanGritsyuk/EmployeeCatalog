using EmployeeCatalog.Common.Entities.Enums;
using System.Text.Json;

namespace EmployeeCatalog.Common.Entities
{
    public class EmployeeEvent
    {
        public EmployeeEvent(ChangeEventType eventType, Employee employee, int version = 1)
        {
            EmployeeId = employee.Id;
            EmployeeState = JsonSerializer.Serialize(employee);
            CreatedAt = DateTime.UtcNow;
            EventType = eventType;
            Version = version;
        }

        public int Id { get; set; }
        public Guid EmployeeId { get; private set; }
        public string EmployeeState { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public ChangeEventType EventType { get; set; }
        public int Version { get; private set; }

        public Employee GetEmployeeData()
        {
            return JsonSerializer.Deserialize<Employee>(EmployeeState);
        }
    }
}
