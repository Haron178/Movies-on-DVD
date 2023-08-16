namespace WebApi.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
        public string? Email { get; set; }
        public ICollection<MovieOnDvd> Movies { get; set; }
    }
}
