using AutoMapper;
using InCinema.Exceptions;
using InCinema.Models.Roles;
using InCinema.Repositories;

namespace InCinema.Services;

public class RolesService
{
    private readonly IApplicationContext _applicationContext;
    private readonly IMapper _mapper;

    public RolesService(IApplicationContext applicationContext, IMapper mapper)
    {
        _applicationContext = applicationContext;
        _mapper = mapper;
    }

    public IEnumerable<RoleView> GetAll()
    {
        IEnumerable<Role> roles = _applicationContext.Roles.GetAll();
        return _mapper.Map<IEnumerable<RoleView>>(roles);
    }

    public RoleView GetById(int roleId)
    {
        Role role = _applicationContext.Roles.GetById(roleId);
        return _mapper.Map<RoleView>(role);
    }

    public RoleView Create(RoleCreate roleCreate)
    {
        Role? role = _applicationContext.Roles.GetByName(roleCreate.Name);
        if (role != null)
            throw new BadRequestException("Role with this name already exist");

        var newRole = _mapper.Map<Role>(roleCreate);
        _applicationContext.Roles.Add(newRole);

        return _mapper.Map<RoleView>(newRole);
    }

    public RoleView Update(RoleUpdate roleUpdate)
    {
        _applicationContext.Roles.GetById(roleUpdate.Id);
        
        Role? role = _applicationContext.Roles.GetByName(roleUpdate.Name);
        if (role != null && role.Id != roleUpdate.Id)
            throw new BadRequestException("Role with this name already exist");

        var updateRole = _mapper.Map<Role>(roleUpdate);
        _applicationContext.Roles.Update(updateRole);

        return _mapper.Map<RoleView>(updateRole);
    }

    public RoleView Delete(int roleId)
    {
        Role role = _applicationContext.Roles.GetById(roleId);
        
        _applicationContext.Roles.Delete(roleId);

        return _mapper.Map<RoleView>(role);
    }
}