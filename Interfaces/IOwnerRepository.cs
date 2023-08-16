using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwnerById(int ownerId);
        Owner GetOwnerByName(string name);
        ICollection<Owner> GetOwnersByCountryName(string countryName);
        ICollection<Owner> GetOwnersByCountryId(int countryId);
        ICollection<Owner> GetOwnersByMovieName(string movieName);
        ICollection<Owner> GetOwnersByMovieId(int movieId);
        bool OwnerExist(string ownerName);
        bool OwnerExist(int ownerId);
        bool Save();
        bool AddOwner(int countryId, Owner newOwner);
        bool DeleteOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteWners(ICollection<Owner> owners);
        bool GetPossession(Owner owner, ICollection<MovieOnDvd> movies);
        bool Dispossession(Owner owner, ICollection<MovieOnDvd> movies);

    }
}
