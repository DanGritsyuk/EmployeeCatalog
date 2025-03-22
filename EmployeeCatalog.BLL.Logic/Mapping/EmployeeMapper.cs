using EmployeeCatalog.Common.Entities;
using EmployeeCatalog.Common.Entities.Enums;

namespace EmployeeCatalog.BLL.Logic.Mapping
{
    public static class EmployeeMapper
    {
        public static void Map(this Employee employee, EmployeeInputModel inputModel)
        {
            if (inputModel == null)
            {
                throw new ArgumentNullException(nameof(inputModel));
            }

            employee.FullName = inputModel.FullName;
            employee.Gender = Enum.Parse<Gender>(inputModel.Gender);
            employee.BirthDate = DateOnly.Parse(inputModel.BirthDate);
        }
    }
}
