using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Models;

public class ApplicationUser : Model
{
    [Required]
    [StringLength(150)]
    public string UserName { get; set; } = null!;

    [Required]
    [StringLength(150)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(255)]
    public string Password { get; set; } = null!;
    
    public DateTime? EmailVerifiedAt { get; set; } = null;
    public ICollection<Photo> Photos { get; set; } = new HashSet<Photo>();
}