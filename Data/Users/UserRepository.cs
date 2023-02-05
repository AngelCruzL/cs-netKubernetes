using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetKubernetes.Dtos.UserDtos;
using NetKubernetes.Middleware;
using NetKubernetes.Models;
using NetKubernetes.Token;

namespace NetKubernetes.Data.Users;

public class UserRepository : IUserRepository
{
  private readonly UserManager<User> _userManager;
  private readonly SignInManager<User> _signInManager;
  private readonly IJwtGenerator _jwtGenerator;
  private readonly AppDbContext _context;
  private readonly IUserSession _userSession;

  public UserRepository(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IJwtGenerator jwtGenerator,
    AppDbContext context,
    IUserSession userSession
  )
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _jwtGenerator = jwtGenerator;
    _context = context;
    _userSession = userSession;
  }

  private UserResponseDto TransformerUserToUserDto(User user)
  {
    return new UserResponseDto
    {
      Id = user.Id,
      Name = user.Name,
      Lastname = user.Lastname,
      Token = _jwtGenerator.CreateToken(user),
      UserName = user.UserName,
      Email = user.Email,
      PhoneNumber = user.PhoneNumber
    };
  }

  public async Task<UserResponseDto> GetUser()
  {
    var user = await _userManager.FindByNameAsync(_userSession.GetUserSession());
    if (user is null)
      throw new MiddlewareException(HttpStatusCode.Unauthorized, new { message = "User not exists on database" });

    return TransformerUserToUserDto(user!);
  }

  public async Task<UserResponseDto> RegisterUser(UserRegisterRequestDto userRegisterRequestDto)
  {
    var emailExists = await _context.Users.Where(u => u.Email == userRegisterRequestDto.Email).AnyAsync();
    if (emailExists)
      throw new MiddlewareException(HttpStatusCode.BadRequest,
        new { message = "The User Email already exists on database" });

    var userNameExists = await _context.Users.Where(u => u.UserName == userRegisterRequestDto.UserName).AnyAsync();
    if (userNameExists)
      throw new MiddlewareException(HttpStatusCode.BadRequest,
        new { message = "The User UserName already exists on database" });

    var user = new User
    {
      Name = userRegisterRequestDto.Name,
      Lastname = userRegisterRequestDto.Lastname,
      UserName = userRegisterRequestDto.UserName,
      Email = userRegisterRequestDto.Email,
      PhoneNumber = userRegisterRequestDto.PhoneNumber
    };

    var result = await _userManager.CreateAsync(user, userRegisterRequestDto.Password!);
    if (result.Succeeded) return TransformerUserToUserDto(user);

    throw new Exception("Error on register user");
  }

  public async Task<UserResponseDto> LoginUser(UserLoginRequestDto userLoginRequestDto)
  {
    var user = await _userManager.FindByEmailAsync(userLoginRequestDto.Email!);
    if (user is null)
      throw new MiddlewareException(HttpStatusCode.Unauthorized,
        new { message = "The User Email not exists on database" });

    var result = await _signInManager.CheckPasswordSignInAsync(user, userLoginRequestDto.Password!, false);
    if (result.Succeeded) return TransformerUserToUserDto(user);

    throw new MiddlewareException(HttpStatusCode.Unauthorized, new { message = "The User Password is incorrect" });
  }
}