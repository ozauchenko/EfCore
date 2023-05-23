namespace EfCoreApp.Models
{
    public class Lecturer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Mark> Marks { get; set; }
    }
}
