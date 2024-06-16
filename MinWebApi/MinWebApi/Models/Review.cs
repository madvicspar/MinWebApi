using System.ComponentModel.DataAnnotations.Schema;

namespace MinWebApi.Models
{
    public class Review
    {
        public int Id { get; set; }
        [ForeignKey("ReviewerId")]
        public int ReviewerId { get; set; }
        public ApplicationUser Reviewer { get; set; }
        public string Comment { get; set; }
        [ForeignKey("MovieId")]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
