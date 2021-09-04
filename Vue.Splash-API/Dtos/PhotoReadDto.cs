namespace Vue.Splash_API.Dtos
{
    public record PhotoReadDto
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
    }
}