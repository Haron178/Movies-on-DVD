namespace WebApi.Models
{
    public class MovieOnDvd
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Owner>? Owners { get; set; }
    }
}
