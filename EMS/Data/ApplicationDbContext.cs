using Microsoft.EntityFrameworkCore;
using EMS.Models;

namespace EMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassStudent> ClassStudents { get; set; }
        public DbSet<Attendence> Attendences { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Announcement> Announcements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Teacher>()
                .HasIndex(t => t.PhoneNumber)
                .IsUnique();

            
            modelBuilder.Entity<User>()
                .HasOne(u => u.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<User>()
                .HasOne(u => u.Teacher)
                .WithOne(t => t.User)
                .HasForeignKey<Teacher>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<Class>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Classes)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<ClassStudent>()
                .HasKey(cs => new { cs.ClassId, cs.StudentId }); // composite key

            modelBuilder.Entity<ClassStudent>()
                .HasOne(cs => cs.Class)
                .WithMany(c => c.ClassStudents)
                .HasForeignKey(cs => cs.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassStudent>()
                .HasOne(cs => cs.Student)
                .WithMany(s => s.ClassStudents)
                .HasForeignKey(cs => cs.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<Attendence>()
                .HasOne(a => a.Class)
                .WithMany(c => c.Attendences)
                .HasForeignKey(a => a.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Attendence>()
                .HasOne(a => a.Student)
                .WithMany(s => s.Attendences)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<Announcement>()
                .HasOne(a => a.Class)
                .WithMany(c => c.Announcements)
                .HasForeignKey(a => a.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Result>()
                .HasOne(r => r.Class)
                .WithMany(c => c.Results)
                .HasForeignKey(r => r.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Result>()
                .HasOne(r => r.Student)
                .WithMany(s => s.Results)
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
