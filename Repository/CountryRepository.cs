using WebApi.Data;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context)
        {
            _context = context;
        }

        public bool AddCountry(Country country)
        {
            _context.Countries.Add(country);
            return Save();
        }

        public bool CountryExist(string name)
        {
            return _context.Countries.Any(c => c.Name.Trim().ToUpper() == name.Trim().ToUpper());
        }

        public bool CountryExist(int id)
        {
            return _context.Countries.Any(c => c.Id == id);
        }

        public bool DeleteCountry(Country country)
        {
            _context.Remove(country);
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.OrderBy(c => c.Id).ToList();
        }

        public Country GetCountryById(int id)
        {
            return _context.Countries.FirstOrDefault(c => c.Id == id);
        }

        public Country GetCountryByName(string name)
        {
            return _context.Countries.FirstOrDefault(c => c.Name.Trim().ToUpper() == name.Trim().ToUpper());
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }
    }
}
