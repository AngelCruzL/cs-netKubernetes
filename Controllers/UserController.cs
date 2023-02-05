using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes.Data.Users;
using NetKubernetes.Dtos.UserDtos;

namespace NetKubernetes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
  private readonly IUserRepository _repository;

  public UserController(IUserRepository repository)
  {
    _repository = repository;
  }

  [AllowAnonymous]
  [HttpPost("login")]
  public async Task<ActionResult<UserResponseDto>> Login([FromBody] UserLoginRequestDto request)
  {
    return await _repository.LoginUser(request);
  }

  [AllowAnonymous]
  [HttpPost("register")]
  public async Task<ActionResult<UserResponseDto>> Register([FromBody] UserRegisterRequestDto request)
  {
    return await _repository.RegisterUser(request);
  }

  [HttpGet]
  public async Task<ActionResult<UserResponseDto>> GetUser()
  {
    return await _repository.GetUser();
  }
}