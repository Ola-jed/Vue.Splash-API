namespace Vue.Splash_API.Dtos;

public record PhotoReadDto
{
    public int Id { get; init; }
    public string Label { get; init; }
    public string Description { get; init; }
}