using EmployeeCatalog.Common.Entities.Enums;

namespace EmployeeCatalog.Common.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public Gender Gender { get; set; }
        public DateOnly BirthDate { get; set; }


        public int GetAge()
        {
            var now = DateTime.Today;
            return now.Year - BirthDate.Year - 1 +
                ((now.Month > BirthDate.Month || now.Month == BirthDate.Month && now.Day >= BirthDate.Day) ? 1 : 0);
        }
    }
}