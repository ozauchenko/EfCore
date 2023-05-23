namespace EfCoreApp.Models
{
    public class Subject
    {
        public int Id { get; set; }

        public SubjectType Name { get; set; }

        public ICollection<Mark> Marks { get; set; }
    }
}
