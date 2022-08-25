using AutoMapper;
using InCinema.Exceptions;
using InCinema.Models.Careers;
using InCinema.Models.Countries;
using InCinema.Models.MoviePersons;
using InCinema.Models.Movies;
using InCinema.Repositories;

namespace InCinema.Services;

public class MoviePersonsService
{
    private readonly IApplicationContext _applicationContext;
    private readonly IMapper _mapper;

    public MoviePersonsService(IApplicationContext applicationContext, IMapper mapper)
    {
        _applicationContext = applicationContext;
        _mapper = mapper;
    }

    public IEnumerable<MoviePersonPreview> GetAll()
    {
        IEnumerable<MoviePerson> moviePersons = _applicationContext.MoviePersons.GetAll();
        return _mapper.Map<IEnumerable<MoviePersonPreview>>(moviePersons);
    }

    public MoviePersonView GetById(int moviePersonId)
    {
        MoviePerson moviePerson = _applicationContext.MoviePersons.GetById(moviePersonId);
        IEnumerable<Career> careers = _applicationContext.Careers.GetByMoviePerson(moviePersonId); 
        
        var moviePersonView = _mapper.Map<MoviePersonView>(moviePerson);
        moviePersonView.Careers = _mapper.Map<IEnumerable<CareerView>>(careers);
        
        return moviePersonView;
    }

    public MoviePersonPreview Create(MoviePersonCreate moviePersonCreate)
    {
        Country country = _applicationContext.Countries.GetById(moviePersonCreate.CountryId);
        var moviePerson = _mapper.Map<MoviePerson>(moviePersonCreate);
        moviePerson.Country = country;
        
        _applicationContext.MoviePersons.Add(moviePerson);

        return _mapper.Map<MoviePersonPreview>(moviePerson);
    }

    public MoviePersonPreview Update(MoviePersonUpdate moviePersonUpdate)
    {
        _applicationContext.MoviePersons.GetById(moviePersonUpdate.Id);
        Country country = _applicationContext.Countries.GetById(moviePersonUpdate.CountryId);
        var moviePerson = _mapper.Map<MoviePerson>(moviePersonUpdate);
        moviePerson.Country = country;

        _applicationContext.MoviePersons.Update(moviePerson);

        return _mapper.Map<MoviePersonPreview>(moviePerson);
    }

    public MoviePersonPreview Delete(int moviePersonId)
    {
        MoviePerson moviePerson = _applicationContext.MoviePersons.GetById(moviePersonId);

        try
        {
            _applicationContext.MoviePersons.Delete(moviePersonId);
        }
        catch (Exception exception)
        {
            throw new BadRequestException(exception.Message);
        }

        return _mapper.Map<MoviePersonPreview>(moviePerson);
    }
    
    #region Careers

    public MoviePersonPreview AddCareer(int moviePersonId, int careerId)
    {
        MoviePerson moviePerson = _applicationContext.MoviePersons.GetById(moviePersonId);
        _applicationContext.Careers.GetById(careerId);

        IEnumerable<Career> careers = _applicationContext.Careers.GetByMoviePerson(moviePersonId);
        if (careers.Any(x => x.Id == careerId))
            throw new BadRequestException("Movie-person already have this career");
        
        _applicationContext.Careers.AddToMoviePerson(careerId, moviePersonId);

        return _mapper.Map<MoviePersonPreview>(moviePerson);
    }

    public MoviePersonPreview DeleteCareer(int moviePersonId, int careerId)
    {
        MoviePerson moviePerson = _applicationContext.MoviePersons.GetById(moviePersonId);
        _applicationContext.Careers.GetById(careerId);

        IEnumerable<Career> careers = _applicationContext.Careers.GetByMoviePerson(moviePersonId);
        if (careers.All(x => x.Id != careerId))
            throw new BadRequestException("Movie-person does not have this career");
        
        _applicationContext.Careers.DeleteFromMoviePerson(careerId, moviePersonId);
        
        return _mapper.Map<MoviePersonPreview>(moviePerson);
    }
    
    #endregion
}