using SS.DocCap.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS.DocCap.Domain.Entities
{
    public class RSLPeriodTBL : AuditableEntity
    {
        public int Id { get; set; }
        public string RSLID { get; set; }
        public string RSLPeriodTitle { get; set; }

        [NotMapped]
        public bool IsCurrent { get; set; }
    }
}
