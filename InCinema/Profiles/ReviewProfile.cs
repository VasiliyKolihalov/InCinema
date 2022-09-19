using AutoMapper;
using InCinema.Models.Reviews;

namespace InCinema.Profiles;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, ReviewView>();
        CreateMap<ReviewCreate, Review>();
        CreateMap<ReviewUpdate, Review>();
    }
}