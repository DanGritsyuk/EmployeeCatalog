using EmployeeCatalog.Common.Entities.Enums;

namespace EmployeeCatalog.Common.Entities
{
    public class EmployeeFilterCriteria
    {
        public string StartsWithLetter { get; set; }
        public Gender Gender { get; set; }
    }
}
