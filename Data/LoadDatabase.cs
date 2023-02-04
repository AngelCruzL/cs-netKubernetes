using Microsoft.AspNetCore.Identity;
using NetKubernetes.Models;

namespace NetKubernetes.Data;

public class LoadDatabase
{
  public static async Task InsertDataAsync(AppDbContext context, UserManager<User> userManager)
  {
    if (!userManager.Users.Any())
    {
      var user = new User
      {
        Name = "Angel",
        Lastname = "Cruz",
        Email = "me@angelcruzl.dev",
        UserName = "AngelCruzL",
        PhoneNumber = "1234567890"
      };

      await userManager.CreateAsync(user, "$ecretP@ssw0rd");
    }

    if (!context.Estates!.Any())
    {
      context.Estates!.AddRange(
        new Estate
        {
          Name = "Casa en la playa",
          Address = "Calle 1, Playa del Carmen, Quintana Roo",
          Price = 1000000,
          Picture = "https://picsum.photos/200/300",
          CreatedAt = DateTime.Now
        },
        new Estate
        {
          Name = "Casa en la montaña",
          Address = "Calle 2, San Cristobal de las Casas, Chiapas",
          Price = 2000000,
          Picture = "https://picsum.photos/200/300",
          CreatedAt = DateTime.Now
        },
        new Estate
        {
          Name = "Casa en el campo",
          Address = "Calle 3, Tlaxcala, Tlaxcala",
          Price = 3000000,
          Picture = "https://picsum.photos/200/300",
          CreatedAt = DateTime.Now
        }
      );
    }

    await context.SaveChangesAsync();
  }
}