using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MinWebApi.Models
{
    public class Movie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public float Rating { get; set; }
    }
}