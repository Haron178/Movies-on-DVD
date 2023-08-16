using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
        }
        public bool AddOwner(int countryId, Owner newOwner)
        {
            var country = _context.Countries.FirstOrDefault(c => c.Id == countryId);
            newOwner.Country = country;
            if(country.Owners == null)
                country.Owners = new List<Owner>();
            country.Owners.Add(newOwner);
            return Save();
        }

        public bool OwnerExist(string ownerName)
        {
            return _context.Owners.Any(o => o.Name.Trim().ToUpper() == ownerName.Trim().ToUpper());
        }

        public bool OwnerExist(int ownerId)
        {
            return _context.Owners.Any(o => o.Id == ownerId);
        }

        public Owner GetOwnerById(int ownerId)
        {
            return _context.Owners.FirstOrDefault(o => o.Id == ownerId);
        }

        public Owner GetOwnerByName(string name)
        {
            return _context.Owners.FirstOrDefault(o => o.Name.Trim().ToUpper() == name.Trim().ToUpper());
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.OrderBy(o => o.Id).ToList();
        }

        public ICollection<Owner> GetOwnersByCountryName(string countryName)
        {
            return _context.Owners.Where(o => o.Country.Name.Trim().ToUpper() == countryName.Trim().ToUpper()).ToList();
        }

        public ICollection<Owner> GetOwnersByMovieName(string movieName)
        {
            return _context.Owners.Where(o => o.Movies.Any(m => m.Name.Trim().ToUpper() == movieName.Trim().ToUpper())).ToList();
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Owners.Remove(owner);
            return Save();
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }

        public ICollection<Owner> GetOwnersByCountryId(int countryId)
        {
            return _context.Owners.Where(o => o.Country.Id == countryId).ToList();
        }

        public ICollection<Owner> GetOwnersByMovieId(int movieId)
        {
            return _context.Owners.Where(o => o.Movies.Any(m => m.Id == movieId)).ToList();
        }

        public bool DeleteWners(ICollection<Owner> owners)
        {
            if (owners == null)
                return true;
            _context.RemoveRange(owners);
            return Save();
        }

        public bool GetPossession(Owner owner, ICollection<MovieOnDvd> movies)
        {
            if (owner.Movies == null)
            {
                owner.Movies = new List<MovieOnDvd>();
            }
            foreach (MovieOnDvd movie in movies)
            { 
                owner.Movies.Add(movie);
                if (movie.Owners == null)
                {
                    movie.Owners = new List<Owner>();
                }
                movie.Owners.Add(owner);
            }
            return Save();
        }

        public bool Dispossession(Owner owner, ICollection<MovieOnDvd> movies)// realise
        {
            var ownerWithMovies = _context.Owners.Include(o => o.Movies).FirstOrDefault(o => o.Id == owner.Id);
            foreach (MovieOnDvd movie in movies)
            {
                ownerWithMovies.Movies.Remove(movie);
            }
            return Save();
        }
    }
}
