using AutoMapper;
using InCinema.Exceptions;
using InCinema.Models.Genres;
using InCinema.Repositories;

namespace InCinema.Services;

public class GenresService
{
    private readonly IApplicationContext _applicationContext;
    private readonly IMapper _mapper;

    public GenresService(IApplicationContext applicationContext, IMapper mapper)
    {
        _applicationContext = applicationContext;
        _mapper = mapper;
    }

    public IEnumerable<GenreView> GetAll()
    {
        IEnumerable<Genre> genres = _applicationContext.Genres.GetAll();
        return _mapper.Map<IEnumerable<GenreView>>(genres);
    }

    public GenreView GetById(int genreId)
    {
        Genre genre = _applicationContext.Genres.GetById(genreId);
        return _mapper.Map<GenreView>(genre);
    }

    public GenreView Create(GenreCreate genreCreate)
    {
        Genre? genre = _applicationContext.Genres.GetByName(genreCreate.Name);
        if (genre != null)
            throw new BadRequestException("Genre with with name already exist");

        var newGenre = _mapper.Map<Genre>(genreCreate);
        _applicationContext.Genres.Add(newGenre);
        
        return _mapper.Map<GenreView>(newGenre);
    }

    public GenreView Update(GenreUpdate genreUpdate)
    {
        _applicationContext.Genres.GetById(genreUpdate.Id);
        
        Genre? genre = _applicationContext.Genres.GetByName(genreUpdate.Name);
        if (genre != null)
            throw new BadRequestException("Genre with with name already exist");
        
        var updateGenre = _mapper.Map<Genre>(genreUpdate);

        _applicationContext.Genres.Update(updateGenre);

        return _mapper.Map<GenreView>(updateGenre);
    }

    public GenreView Delete(int genreId)
    {
        Genre genre = _applicationContext.Genres.GetById(genreId);

        _applicationContext.Genres.Delete(genreId);

        return _mapper.Map<GenreView>(genre);
    }
}