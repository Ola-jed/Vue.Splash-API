using System;
using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos
{
    public record UserReadDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public DateTime RegisterDate { get; set; }
    }
}