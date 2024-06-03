using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS.DocCap.Application.Common.Interfaces
{
    public interface ICurrentDateTimeService
    {
        DateTime dateToday { get; }
        DateTime dateTimeToday { get; }
    }
}
