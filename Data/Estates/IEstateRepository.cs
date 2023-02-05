using NetKubernetes.Models;

namespace NetKubernetes.Data.Estates;

public interface IEstateRepository
{
  Task<bool> SaveChanges();
  Task<IEnumerable<Estate>> GetAllEstates();
  Task<Estate> GetEstateById(int id);
  Task CreateEstate(Estate estate);
  Task DeleteEstate(int id);
}