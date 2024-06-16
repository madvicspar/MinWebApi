namespace MinWebApi.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public float Rating { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}