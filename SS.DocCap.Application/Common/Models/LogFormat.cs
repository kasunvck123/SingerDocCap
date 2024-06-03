using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS.DocCap.Application.Common.Models
{
    public class LogFormat
    {
        public string Request { get; set; }
        public string RequestType { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now.ToUniversalTime().AddHours(5).AddMinutes(30);
        public int StatusCode { get; set; } = 0;
        public string Severity { get; set; }
        public string Message { get; set; }
        public object Description { get; set; }
        public string UserID { get; set; }
        public string Email { get; set; }




    }
}
