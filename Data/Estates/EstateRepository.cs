using System.Net;
using Microsoft.AspNetCore.Identity;
using NetKubernetes.Middleware;
using NetKubernetes.Models;
using NetKubernetes.Token;

namespace NetKubernetes.Data.Estates;

public class EstateRepository : IEstateRepository
{
  private readonly AppDbContext _context;
  private readonly IUserSession _userSession;
  private readonly UserManager<User> _userManager;

  public EstateRepository(AppDbContext context, IUserSession userSession, UserManager<User> userManager)
  {
    _context = context;
    _userSession = userSession;
    _userManager = userManager;
  }

  public bool SaveChanges()
  {
    return _context.SaveChanges() >= 0;
  }

  public IEnumerable<Estate> GetAllEstates()
  {
    return _context.Estates!.ToList();
  }

  public Estate GetEstateById(int id)
  {
    return _context.Estates!.FirstOrDefault(e => e.Id == id)!;
  }

  public async Task CreateEstate(Estate estate)
  {
    var user = await _userManager.FindByNameAsync(_userSession.GetUserSession());

    if (user is null)
      throw new MiddlewareException(HttpStatusCode.Unauthorized,
        new { message = "This user doesn't have permission to perform this action" });

    if (estate is null)
      throw new MiddlewareException(HttpStatusCode.BadRequest, new { message = "The estate data is not valid" });

    estate.CreatedAt = DateTime.Now;
    estate.UserId = Guid.Parse(user.Id);

    _context.Estates!.Add(estate);
  }

  public void DeleteEstate(int id)
  {
    var estate = _context.Estates!.FirstOrDefault(e => e.Id == id);

    if (estate == null) return;

    _context.Estates!.Remove(estate);
  }
}