using SS.DocCap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS.DocCap.Infastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed, if necessary
            if (!context.AppUser.Any())
            {
                var appUser = new AppUser
                {
                    Name = "Global Admin",
                    IsActive = true,
                    Designation = "Admin",
                    Email = "brs@sits.lk",
                    Password = "123",
                    MobileNumber = "0117619619",
                    OfficeNumber = "0112432234",
                    Remark = "",
                };
                context.AppUser.Add(appUser);
                await context.SaveChangesAsync();
            }
            context.SaveChanges();
        }
    }
}
