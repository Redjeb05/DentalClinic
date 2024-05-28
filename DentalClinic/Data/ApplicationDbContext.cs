using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DentalClinic.Data;

namespace DentalClinic.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DentalClinic.Data.Dentist>? Dentist { get; set; }
        public DbSet<DentalClinic.Data.Patient>? Patient { get; set; }
        public DbSet<DentalClinic.Data.Examination>? Examination { get; set; }
    }
}