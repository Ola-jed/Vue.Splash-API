using System;
using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Models;

public abstract class Model
{
    [Key]
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }
}