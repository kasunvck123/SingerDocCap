using SS.DocCap.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS.DocCap.Domain.Entities
{
    public class DocCountData: AuditableEntity
    {
       
        public string DocumentType { get; set; }
        public string RSLPeriod { get; set; }
        public string RSLPeriodId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public int  DocumentCount { get; set; }
    }
}
