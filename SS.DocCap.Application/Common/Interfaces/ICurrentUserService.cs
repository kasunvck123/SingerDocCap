using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS.DocCap.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }

        string CurrentUserId { get; }

        string Name { get; }

        public string EmployeeType { get; }
        public string BranchID { get; }
        public string BranchCode { get;}
        public string BranchName { get;}
        public string HierachyID { get;}
        public string Department { get;}


    }
}
