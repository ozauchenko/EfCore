using EfCoreApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bogus;

namespace EfCoreApp.Data
{
    public class DbTestContext : DbContext
    {
        public DbTestContext(DbContextOptions<DbTestContext> options) : base(options) {}

        public DbSet<Student> Students { get; set; }

        public DbSet<Mark> Marks { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Lecturer> Lecturers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            BuildStudent(modelBuilder.Entity<Student>());
            BuildSubject(modelBuilder.Entity<Subject>());
            BuildLecturer(modelBuilder.Entity<Lecturer>());
            BuildMark(modelBuilder.Entity<Mark>());
        }

        private static void BuildStudent(EntityTypeBuilder<Student> studentTypeBuilder)
        {
            studentTypeBuilder.ToTable("Student");
            studentTypeBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            studentTypeBuilder.Property(x => x.Name).IsRequired();
        }

        private static void BuildSubject(EntityTypeBuilder<Subject> subjectTypeBuilder)
        {
            subjectTypeBuilder.ToTable("Subject");
            subjectTypeBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            subjectTypeBuilder.Property(x => x.Name).IsRequired();
        }

        private static void BuildLecturer(EntityTypeBuilder<Lecturer> lecturerTypeBuilder)
        {
            lecturerTypeBuilder.ToTable("Lecturer");
            lecturerTypeBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            lecturerTypeBuilder.Property(x => x.Name).IsRequired();
        }

        private static void BuildMark(EntityTypeBuilder<Mark> markTypeBuilder)
        {
            markTypeBuilder.ToTable("Mark");
            markTypeBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            markTypeBuilder.Property(x => x.Grade).IsRequired();
            markTypeBuilder.HasOne(x => x.Student).WithMany(x => x.Marks);
            markTypeBuilder.HasOne(x => x.Subject).WithMany(x => x.Marks);
            markTypeBuilder.HasOne(x => x.Lecturer).WithMany(x => x.Marks);
        }
    }
}