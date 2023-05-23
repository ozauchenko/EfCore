using Bogus;
using EfCoreApp.Data;
using EfCoreApp.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EfCoreApp.ConfigurationExtensions
{
    internal static class DbConfiguration
    {
        public static void RegisterDbServices(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException($"Parameter {nameof(connectionString)} is null or empty.");
            }

            RegisterSqlLite(services, connectionString);
        }

        public static void EnsureDbIsReady(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                MigrateDb(serviceScope);
                SeedData(serviceScope);
            }
        }

        private static void MigrateDb(IServiceScope serviceScope)
        {
            var context = serviceScope.ServiceProvider.GetService<DbTestContext>();
            context.Database.Migrate();

            if (context.Database.GetDbConnection() is SqliteConnection conn)
            {
                SqliteConnection.ClearPool(conn);
            }
        }

        private static void RegisterSqlLite(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DbTestContext>(
                options =>
                {
                    options.UseSqlite(connectionString);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    options.EnableDetailedErrors();
                });
        }

        private static void SeedData(IServiceScope serviceScope)
        {
            var context = serviceScope.ServiceProvider.GetService<DbTestContext>();
            context.Database.EnsureCreated();

            if (context.Students.Any() && context.Lecturers.Any() && context.Subjects.Any() && context.Marks.Any())
            {
                return;
            }

            List<Student> students = new();
            List<Subject> subjects = new();
            List<Lecturer> lecturers = new();
            List<Mark> marks = new();

            int markId = 1;

            for (int i = 1; i < 100; i++)
            {
                var fakeStudent = new Faker<Student>()
                    .RuleFor(u => u.Id, f => i)
                    .RuleFor(u => u.Name, f => f.Name.FirstName());

                students.Add(fakeStudent);

                var fakeSubject = new Faker<Subject>()
                    .RuleFor(u => u.Id, f => i)
                    .RuleFor(u => u.Name, f => f.PickRandom<SubjectType>());
                subjects.Add(fakeSubject);

                var fakeLecturer = new Faker<Lecturer>()
                    .RuleFor(u => u.Id, f => i)
                    .RuleFor(u => u.Name, f => f.Name.FirstName());
                lecturers.Add(fakeLecturer);

                for (int j = 1; j < 100; j++)
                {
                    var fakeMark = new Faker<Mark>()
                        .RuleFor(u => u.Id, f => markId++)
                        .RuleFor(u => u.Grade, f => f.PickRandom<MarkType>())
                        .RuleFor(u => u.StudentId, f => i)
                        .RuleFor(u => u.SubjectId, f => i)
                        .RuleFor(u => u.LecturerId, f => i);

                    marks.Add(fakeMark);
                }
            }

            context.Students.AddRange(students);
            context.Lecturers.AddRange(lecturers);
            context.Subjects.AddRange(subjects);
            context.Marks.AddRange(marks);

            context.SaveChanges();
        }
    }
}
