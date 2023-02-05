namespace NetKubernetes.Dtos.EstateDtos;

public class EstateRequestDto
{
  public string? Name { get; set; }
  public string? Address { get; set; }
  public decimal Price { get; set; }
  public string? Picture { get; set; }
}