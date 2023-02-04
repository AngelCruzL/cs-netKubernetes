using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetKubernetes.Models;

public class Estate
{
  [Key]
  [Required]
  public int Id { get; set; }
  public string? Name { get; set; }
  public string? Address { get; set; }
  [Required]
  [Column(TypeName = "decimal(18,2)")]
  public int Price { get; set; }
  public string? Picture { get; set; }
  public DateTime? CreatedAt { get; set; }
  public Guid? UserId { get; set; }
}