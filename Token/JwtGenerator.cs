using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NetKubernetes.Models;

namespace NetKubernetes.Token;

public class JwtGenerator : IJwtGenerator
{
  public string CreateToken(User user)
  {
    var claims = new List<Claim>
    {
      new Claim(JwtRegisteredClaimNames.NameId, user.UserName!),
      new Claim("userId", user.Id.ToString()),
      new Claim("email", user.Email!)
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret key"));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.Now.AddDays(7),
      SigningCredentials = creds
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(token);
  }
}