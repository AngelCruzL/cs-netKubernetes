using Microsoft.AspNetCore.Identity;
using NetKubernetes.Dtos.UserDtos;
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
    return TransformerUserToUserDto(user!);
  }

  public async Task<UserResponseDto> RegisterUser(UserRegisterRequestDto userRegisterRequestDto)
  {
    var user = new User
    {
      Name = userRegisterRequestDto.Name,
      Lastname = userRegisterRequestDto.Lastname,
      UserName = userRegisterRequestDto.UserName,
      Email = userRegisterRequestDto.Email,
      PhoneNumber = userRegisterRequestDto.PhoneNumber
    };

    await _userManager.CreateAsync(user, userRegisterRequestDto.Password!);

    return TransformerUserToUserDto(user);
  }

  public async Task<UserResponseDto> LoginUser(UserLoginRequestDto userLoginRequestDto)
  {
    var user = await _userManager.FindByEmailAsync(userLoginRequestDto.Email!);
    await _signInManager.CheckPasswordSignInAsync(user!, userLoginRequestDto.Password!, false);
    return TransformerUserToUserDto(user!);
  }
}