using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vue.Splash_API.Models
{
    public class Photo
    {
        [Key] public int Id { get; set; }
        [Required] [MaxLength(100)] public string Path { get; set; }
        [Required] [MaxLength(100)] public string Label { get; set; }
        [Required] [Column(TypeName = "text")] public string Description { get; set; }
        [Required] public string ApplicationUserId { get; set; }
    }
}