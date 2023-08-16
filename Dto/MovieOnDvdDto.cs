using WebApi.Models;

namespace WebApi.Dto
{
    public class MovieOnDvdDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        //public ICollection<Genre> Genres { get; set; }
    }
}
