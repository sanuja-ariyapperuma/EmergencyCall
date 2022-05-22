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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>(entity => {
                //entity.HasNoKey();
                entity.ToTable("user");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Contact).HasColumnName("contact");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.Is_active).HasColumnName("is_active");
                entity.Property(e => e.User_role).HasColumnName("user_role");
                entity.HasOne(p => p.UserRole).WithMany(b => b.Users);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                //entity.HasNoKey();
                entity.ToTable("user_role");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
