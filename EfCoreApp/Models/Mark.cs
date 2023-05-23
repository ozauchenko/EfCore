namespace EfCoreApp.Models
{
    public class Mark
    {
        public int Id { get; set; }

        public MarkType Grade { get; set; }

        public int StudentId {get; set; }

        public int SubjectId { get; set; }

        public int LecturerId { get; set; }

        public Student Student { get; set; }

        public Subject Subject { get; set; }

        public Lecturer Lecturer { get; set; }
    }
}
