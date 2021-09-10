using System;

namespace Vue.Splash_API.Dtos
{
    public record UserReadDto
    {
        public string Id { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
        public DateTime RegisterDate { get; init; }
    }
}