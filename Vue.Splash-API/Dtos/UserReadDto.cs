using System;

namespace Vue.Splash_API.Dtos;

public record UserReadDto(int Id, string UserName, string Email,DateTime EmailVerifiedAt, DateTime CreatedAt);