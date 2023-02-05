namespace NetKubernetes.Dtos.EstateDtos;

public class EstateResponseDto
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public string? Address { get; set; }
  public decimal Price { get; set; }
  public string? Picture { get; set; }
  public DateTime? CreatedAt { get; set; }
}