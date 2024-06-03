using Microsoft.Extensions.Logging;
using SS.DocCap.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS.DocCap.Application.Common.Interfaces
{
    public interface IApplicationLogger
    {
        /// <summary>
        /// Logging all the details using serilog
        /// </summary>
        /// <param name="level"></param>
        /// <param name="logFormat"></param>
        /// <returns>void</returns>
        public Task<object> Log(LogLevel level, LogFormat logFormat);
    }
}
