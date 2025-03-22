using EmployeeCatalog.Common.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCatalog.Common.Entities
{
    public class EmployeeFilterCriteria
    {
        public string StartsWithLetter { get; set; }
        public Gender Gender { get; set; }
    }
}
