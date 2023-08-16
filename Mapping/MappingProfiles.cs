using AutoMapper;
using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<MovieOnDvd, MovieOnDvdDto>();
            CreateMap<MovieOnDvdDto, MovieOnDvd>();
            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>();
            CreateMap <Genre, GenreDto>();
            CreateMap<GenreDto, Genre>();
            CreateMap <Owner, OwnerDto>();
            CreateMap<OwnerDto, Owner>();
            CreateMap <Review, ReviewDto>();
            CreateMap<ReviewDto, Review>();
            CreateMap <Reviewer, ReviewerDto>();
            CreateMap<ReviewerDto, Reviewer>();
        }
    }
}
