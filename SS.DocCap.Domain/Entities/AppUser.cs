using SS.DocCap.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS.DocCap.Domain.Entities
{
    public class AppUser : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public string MobileNumber { get; set; }
        public string OfficeNumber { get; set; }
        public string Password { get; set; }
        public string Remark { get; set; }


    }
}
