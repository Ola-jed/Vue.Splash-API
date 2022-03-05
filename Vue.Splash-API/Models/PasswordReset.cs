using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vue.Splash_API.Models;

public class PasswordReset : Model
{
    [Required]
    [StringLength(150)]
    public string Token { get; set; } = null!;

    [ForeignKey(nameof(ApplicationUser))]
    public int ApplicationUserId { get; set; }

    public ApplicationUser ApplicationUser { get; set; } = null!;
}