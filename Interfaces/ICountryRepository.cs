using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountryById(int id);
        Country GetCountryByName(string name);
        bool CountryExist(string name);
        bool CountryExist(int id);
        bool AddCountry(Country country);
        bool Save();
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
    }
}
