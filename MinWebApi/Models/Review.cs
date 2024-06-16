using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MinWebApi.Models
{
    public class Review
    {
        public int Id { get; set; }
        [ForeignKey("ReviewerId")]
        public int ReviewerId { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser? Reviewer { get; set; }
        public string Comment { get; set; }
        [ForeignKey("MovieId")]
        public int MovieId { get; set; }
        [JsonIgnore]
        public virtual Movie? Movie { get; set; }
    }
}
