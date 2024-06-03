using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS.DocCap.Application.Common.Exceptions
{
    public class RecordExitsException : Exception
    {
        public RecordExitsException()
            : base()
        {
        }
        public RecordExitsException(string message)
            : base(message)
        {
        }
        public RecordExitsException(string message, Exception innerException)
           : base(message, innerException)
        {
        }
    }
}
