using Microsoft.EntityFrameworkCore;
using PatientManagement.Api.Models;


namespace PatientManagement.Api.Data
{
    public class PatientDbContext : DbContext
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options) { }
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Condition> Conditions => Set<Condition>();
        public DbSet<PatientCondition> PatientConditions => Set<PatientCondition>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
                e.Property(x => x.LastName).IsRequired().HasMaxLength(100);
                e.Property(x => x.Gender).IsRequired().HasMaxLength(10);
                e.Property(x => x.City).IsRequired().HasMaxLength(100);
                e.Property(x => x.Email).IsRequired().HasMaxLength(255);
                e.Property(x => x.Phone).IsRequired().HasMaxLength(20);
                e.HasIndex(x => x.Email).IsUnique();
                e.HasIndex(x => x.Phone).IsUnique();
                e.HasIndex(x => x.City);
                e.HasMany(x => x.PatientConditions)
                .WithOne(pc => pc.Patient)
                .HasForeignKey(pc => pc.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Condition>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.HasIndex(x => x.Name).IsUnique();
            });


            modelBuilder.Entity<PatientCondition>(e =>
            {
                e.HasKey(x => new { x.PatientId, x.ConditionId });
                e.HasOne(x => x.Patient).WithMany(p => p.PatientConditions).HasForeignKey(x => x.PatientId);
                e.HasOne(x => x.Condition).WithMany(c => c.PatientConditions).HasForeignKey(x => x.ConditionId);
                e.Property(x => x.DiagnosedDate).IsRequired();
                e.HasIndex(x => x.ConditionId);
                e.HasIndex(x => x.DiagnosedDate);
            });
        }
    }
}