using NetKubernetes.Dtos.UserDtos;

namespace NetKubernetes.Data.Users;

public interface IUserRepository
{
  Task<UserResponseDto> GetUser();
  Task<UserResponseDto> RegisterUser(UserRegisterRequestDto userRegisterRequestDto);
  Task<UserResponseDto> LoginUser(UserLoginRequestDto userLoginRequestDto);
}