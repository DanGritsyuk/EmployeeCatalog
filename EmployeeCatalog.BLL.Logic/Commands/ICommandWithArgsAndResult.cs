using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCatalog.BLL.Logic.Commands
{
    public interface ICommandWithArgsAndResult<TInput, TResult>
    {
        Task<TResult> ExecuteAsync(TInput input);
    }
}
