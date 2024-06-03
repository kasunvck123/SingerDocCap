using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS.DocCap.Application.Auth.Queries.GetAuth
{
    public class AuthDetailVm
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string JwtToken { get; set; }
        public string EmployeeType { get; set; }
        public string BranchID { get; set; }
        public string UserId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string HierachyID { get; set; }
        public string Department { get; set; }

    }
}
