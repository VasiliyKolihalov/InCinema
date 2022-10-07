using AutoMapper;
using InCinema.Exceptions;
using InCinema.Models.Careers;
using InCinema.Repositories;

namespace InCinema.Services;

public class CareersService
{
    private readonly IApplicationContext _applicationContext;
    private readonly IMapper _mapper;

    public CareersService(IApplicationContext applicationContext, IMapper mapper)
    {
        _applicationContext = applicationContext;
        _mapper = mapper;
    }

    public IEnumerable<CareerView> GetAll()
    {
        IEnumerable<Career> careers = _applicationContext.Careers.GetAll();
        return _mapper.Map<IEnumerable<CareerView>>(careers);
    }

    public CareerView GetById(int careerId)
    {
        Career career = _applicationContext.Careers.GetById(careerId);
        return _mapper.Map<CareerView>(career);
    }

    public CareerView Create(CareerCreate careerCreate)
    {
        Career? career = _applicationContext.Careers.GetByName(careerCreate.Name);
        if(career != null)
            throw new BadRequestException("Career with this name already exist");

        var newCareer = _mapper.Map<Career>(careerCreate);
        
        _applicationContext.Careers.Add(newCareer);
        
        return _mapper.Map<CareerView>(newCareer);
    }

    public CareerView Update(CareerUpdate careerUpdate)
    {
        _applicationContext.Careers.GetById(careerUpdate.Id);
        
        Career? career = _applicationContext.Careers.GetByName(careerUpdate.Name);
        if(career != null && career.Id != careerUpdate.Id)
            throw new BadRequestException("Career with this name already exist");

        var updateCareer = _mapper.Map<Career>(careerUpdate);
        
        _applicationContext.Careers.Update(updateCareer);

        return _mapper.Map<CareerView>(updateCareer);
    }

    public CareerView Delete(int careerId)
    {
        Career career = _applicationContext.Careers.GetById(careerId);
        
        _applicationContext.Careers.Delete(careerId);

        return _mapper.Map<CareerView>(career);
    }
}