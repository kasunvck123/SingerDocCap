using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS.DocCap.Application.Common.Models
{
    public class ConfigurationManager
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public string RequestKey { get; set; }
        public string ServiceAccountemail { get; set; }
        public string ServiceAccountpassword { get; set; }
        public string pkey { get; set; }
        public string LdapServer { get; set; }
        public string Domain { get; set; }
        public string Port { get; set; }
        public string ServiceAccountDn { get; set; }
        public string ServiceAccountUserName { get; set; }
        public string ServiceAccountPassword { get; set; }
        public string SearchBase { get; set; }
        public string RSLPeriodAPI { get; set; }
        public string BMSAuth { get; set; }

    }
}
