namespace NetKubernetes.Data.Users;

public class UserRegisterRequestDto
{
  public string? Name { get; set; }
  public string? Lastname { get; set; }
  public string? UserName { get; set; }
  public string? Email { get; set; }
  public string? Password { get; set; }
  public string? PhoneNumber { get; set; }
}