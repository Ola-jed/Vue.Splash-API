using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Vue.Splash_API.Models;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        Photos = new HashSet<Photo>();
    }

    [Required]
    public DateTime RegisterDate { get; set; }

    public virtual ICollection<Photo> Photos { get; set; }
}