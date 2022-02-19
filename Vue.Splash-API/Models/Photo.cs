using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vue.Splash_API.Models;

public class Photo
{
    public Photo()
    {
        CreatedAt = DateTime.Now;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Path { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Thumbnail { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Label { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "text")]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    [Required]
    public string ApplicationUserId { get; set; } = string.Empty;
}