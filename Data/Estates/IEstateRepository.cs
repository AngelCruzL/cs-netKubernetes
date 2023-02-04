using NetKubernetes.Models;

namespace NetKubernetes.Data.Estates;

public interface IEstateRepository
{
  bool SaveChanges();
  IEnumerable<Estate> GetAllEstates();
  Estate GetEstateById(int id);
  Task CreateEstate(Estate estate);
  void DeleteEstate(int id);
}