using System;

namespace Vue.Splash_API.Dtos;

public record UserReadDto(string Id, string UserName, string Email, DateTime RegisterDate);