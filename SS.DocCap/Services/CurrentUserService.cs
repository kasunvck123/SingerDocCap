using Microsoft.AspNetCore.Http;
using SS.DocCap.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SS.DocCap.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var identity = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IList<Claim> cliam = identity.Claims.ToList();
                if (cliam.Count > 0)
                {
                    Email = cliam[1].Value;
                    CurrentUserId = cliam[3].Value;
                    Name = cliam[2].Value;
                    UserId = cliam[3].Value;
                    BranchID = cliam[4].Value;
                    BranchCode = cliam[5].Value;
                    BranchName = cliam[6].Value;
                    HierachyID = cliam[7].Value;
                    Department = cliam[8].Value;
                    EmployeeType = cliam[9].Value;
                }
            }
        }
        public string UserId { get; }
        public string CurrentUserId { get; }
        public string Name { get; }
        public string Email { get; }

        public string EmployeeType { get; }
        public string BranchID { get; }
        public string BranchCode { get; }
        public string BranchName { get; }
        public string HierachyID { get; }
        public string Department { get; }



    }
}
