using Microsoft.EntityFrameworkCore;

namespace Ambulance.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {

        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public virtual DbSet<UserInfo>? UserInfos { get; set; }
        public virtual DbSet<UserRole>? UserRoles { get; set; }
        public virtual DbSet<Hospital>? Hospitals { get; set; }
        public virtual DbSet<IncidentDetail>? IncidentDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>(entity => {
                entity.ToTable("user");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Contact).HasColumnName("contact");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.Is_active).HasColumnName("is_active");
                entity.Property(e => e.User_role).HasColumnName("user_role");
                entity.HasOne(p => p.UserRole).WithMany(b => b.Users);
                entity.HasMany(p => p.Ambulances).WithOne(b => b.Ambulance);
                entity.HasMany(p => p.DischargedDoctors).WithOne(b => b.DischargedDoctor);
                entity.HasMany(p => p.Doctors).WithOne(b => b.Doctor);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_role");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<IncidentDetail>(entity => 
            {
                entity.ToTable("incident");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.PatientName).HasColumnName("patient_name");
                entity.Property(e => e.PatientAge).HasColumnName("patient_age");
                entity.Property(e => e.PatientContact).HasColumnName("patient_contact_number");
                entity.Property(e => e.Gender).HasColumnName("gender");
                entity.Property(e => e.HospitalId).HasColumnName("admited_hospital");
                entity.Property(e => e.PatientStatus).HasColumnName("patient_status");
                entity.Property(e => e.PatientAddress).HasColumnName("patient_address");
                entity.Property(e => e.GardianName).HasColumnName("gardian_name");
                entity.Property(e => e.GardianContact).HasColumnName("gardian_contact");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.PickupTime).HasColumnName("pickup_time");
                entity.Property(e => e.DischargedTime).HasColumnName("discharged_time");
                entity.Property(e => e.ImportantNotes).HasColumnName("important_notice");
                entity.Property(e => e.DropOffTime).HasColumnName("dropoff_time");
                entity.Property(e => e.AmbulanceId).HasColumnName("ambulance_id");
                entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
                entity.Property(e => e.DischargedDoctorId).HasColumnName("discharged_doctor_id");
                entity.HasOne(p => p.Hospital).WithMany(b => b.Incidents);
                entity.HasOne(p => p.Ambulance).WithMany(b => b.Ambulances);
                entity.HasOne(p => p.Doctor).WithMany(b => b.Doctors);
                entity.HasOne(p => p.DischargedDoctor).WithMany(b => b.DischargedDoctors);
            });

            modelBuilder.Entity<Hospital>(entity =>
            {
                entity.ToTable("hospital");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");

            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
