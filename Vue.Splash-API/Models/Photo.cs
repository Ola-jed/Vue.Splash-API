using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vue.Splash_API.Models;

public class Photo : Model
{
    [Required]
    [MaxLength(150)]
    public string Path { get; set; } = null!;

    [Required]
    [MaxLength(150)]
    public string Thumbnail { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Label { get; set; } = null!;

    [Required]
    [Column(TypeName = "text")]
    public string Description { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(ApplicationUser))]
    public int ApplicationUserId { get; set; }
}