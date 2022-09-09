using AutoMapper;
using InCinema.Exceptions;
using InCinema.Models.Roles;
using InCinema.Models.Users;
using InCinema.Repositories;
using Scriban;
using BCryptNet = BCrypt.Net.BCrypt;

namespace InCinema.Services;

public class AccountService
{
    private readonly IApplicationContext _applicationContext;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;
    private readonly IConfirmCodeGenerator _confirmCodeGenerator;

    public AccountService(IApplicationContext applicationContext,
        IMapper mapper,
        IJwtService jwtService,
        IEmailService emailService,
        IConfirmCodeGenerator confirmCodeGenerator)
    {
        _applicationContext = applicationContext;
        _mapper = mapper;
        _jwtService = jwtService;
        _emailService = emailService;
        _confirmCodeGenerator = confirmCodeGenerator;
    }

    public string Register(UserCreate userCreate)
    {
        User? user = _applicationContext.Users.GetByEmail(userCreate.Email);
        if (user != null)
            throw new BadRequestException("User with this email already exist");

        User newUser = _mapper.Map<User>(userCreate);
        newUser.PasswordHash = BCryptNet.HashPassword(userCreate.Password);

        _applicationContext.Users.Add(newUser);

        return _jwtService.GenerateJwt(newUser);
    }

    public string Login(UserLogin userLogin)
    {
        User? user = _applicationContext.Users.GetByEmail(userLogin.Email);
        if (user == null)
            throw new BadRequestException("User with this email does not exist");

        if (!BCryptNet.Verify(userLogin.Password, user.PasswordHash))
            throw new BadRequestException("Incorrect login or password");

        IEnumerable<Role> roles = _applicationContext.Roles.GetByUserId(user.Id);

        return _jwtService.GenerateJwt(user, roles);
    }

    public string ChangePassword(ChangeUserPassword changeUserPassword)
    {
        User? user = _applicationContext.Users.GetByEmail(changeUserPassword.Email);
        if (user == null)
            throw new BadRequestException("User with this email does not exist");

        if (!BCryptNet.Verify(changeUserPassword.OldPassword, user.PasswordHash))
            throw new BadRequestException("Incorrect login or password");

        user.PasswordHash = BCryptNet.HashPassword(changeUserPassword.NewPassword);
        _applicationContext.Users.ChangePasswordHash(user);

        IEnumerable<Role> roles = _applicationContext.Roles.GetByUserId(user.Id);

        return _jwtService.GenerateJwt(user, roles);
    }

    public string ChangeEmail(ChangeUserEmail changeUserEmail)
    {
        User? user = _applicationContext.Users.GetByEmail(changeUserEmail.OldEmail);
        if (user == null)
            throw new BadRequestException("User with this email does not exist");

        if (!BCryptNet.Verify(changeUserEmail.Password, user.PasswordHash))
            throw new BadRequestException("Incorrect login or password");

        user.IsConfirmEmail = false;
        user.Email = changeUserEmail.NewEmail;
        _applicationContext.Users.ChangeEmail(user);

        IEnumerable<Role> roles = _applicationContext.Roles.GetByUserId(user.Id);

        return _jwtService.GenerateJwt(user, roles);
    }

    public string GenerateEmailConfirmCode(int userId)
    {
        User user = _applicationContext.Users.GetById(userId);

        if (user.IsConfirmEmail)
            throw new BadRequestException("User email is already confirm");

        if (_applicationContext.Users.GetEmailConfirmCode(userId) != null)
            throw new BadRequestException("Confirm code is already send");

        string code = _confirmCodeGenerator.GenerateEmailConfirmCode();
        _applicationContext.Users.AddEmailConfirmCode(userId, code);
        return code;
    }

    public void SendEmailConfirmCode(int userId, string callbackUrl)
    {
        User user = _applicationContext.Users.GetById(userId);

        var htmlString = File.ReadAllText("HtmlTemplates/EmailConfirm.html");
        Template template = Template.Parse(htmlString);
        string message = template.Render(new { callback_url = callbackUrl });

        _emailService.Send(user.Email, "Email confirm", message);
    }

    public void ConfirmEmail(int userId, string code)
    {
        User user = _applicationContext.Users.GetById(userId);

        if (user.IsConfirmEmail)
            throw new BadRequestException("User email is already confirm");

        string? confirmCode = _applicationContext.Users.GetEmailConfirmCode(userId);

        if (confirmCode == null)
            throw new BadRequestException("User didn't ask for confirm code");

        if (confirmCode != code)
            throw new BadRequestException("Incorrect confirm code");

        _applicationContext.Users.ConfirmEmail(userId);
        _applicationContext.Users.DeleteEmailConfirmCode(userId);
    }
}