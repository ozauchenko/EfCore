using Bogus.Bson;
using EfCoreApp.Data;
using EfCoreApp.Models;
using EfCoreApp.Servicies.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace EfCoreApp.Servicies
{
    public class DataService : IDataService
    {
        private readonly DbTestContext _context;

        public DataService(DbTestContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DataViewModel>> GetAsync()
        {
            var query = from m in _context.Marks.AsNoTracking()
                        join st in _context.Students.AsNoTracking() on m.StudentId equals st.Id
                        join d in _context.Lecturers.AsNoTracking() on m.LecturerId equals d.Id
                        join s in _context.Subjects.AsNoTracking() on m.SubjectId equals s.Id
                        orderby st.Name
                        select new GetDataModel { Mark = m, Lecturer = d, Student = st, Subject = s };

            var result = await query.ToListAsync();
            return result.Select(x => new DataViewModel
            {
                Grade = x.Mark.Grade,
                SubjectName = Enum.GetName(typeof(SubjectType), x.Subject.Name),
                StudentName = x.Student.Name,
                LecturerName = x.Lecturer.Name
            });
        }
    }
}
