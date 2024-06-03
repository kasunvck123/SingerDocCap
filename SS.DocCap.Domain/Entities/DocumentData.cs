using SS.DocCap.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS.DocCap.Domain.Entities
{
    public class DocumentData : AuditableEntity
    {
        public int Id { get; set; }
        public string DocumentType { get; set; }
        public bool IsChequeIssue { get; set; }
        public string RSLPeriod { get; set; }
        public string DocumentNo { get; set; }
        public decimal DocumentAmount { get; set; }
        public string DocumentUrl { get; set; }
        public string RootUrl { get; set; }
        public string Remark { get; set; }
        public string RSLPeriodId { get; set; }
        public string Folder { get; set; }
        public string CaptureType { get; set; }

        public string EmployeeType { get; set; }
        public string BranchID { get; set; }
        public string UserId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string HierachyID { get; set; }
        public string Department { get; set; }


    }
}
