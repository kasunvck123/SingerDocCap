using SS.DocCap.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SS.DocCap.Services
{
    public class CurrentDateTimeService : ICurrentDateTimeService
    {
        public CurrentDateTimeService()
        {
            dateToday = DateTime.UtcNow.AddHours(5).AddMinutes(30).Date;
            dateTimeToday = DateTime.Now.ToUniversalTime().AddHours(5).AddMinutes(30);
        }

        public DateTime dateToday { get; }
        public DateTime dateTimeToday { get; }
    }
}
