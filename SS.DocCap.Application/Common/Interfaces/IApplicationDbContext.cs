using Microsoft.EntityFrameworkCore;
using SS.DocCap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SS.DocCap.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<DocumentData> DocumentData { get; set; }
        public DbSet<RSLPeriodTBL> RSLPeriodTBL { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
