using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

        var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

        var users = JsonSerializer.Deserialize<List<User>>(userData, options);

        if (users == null) return;

        var roles = new List<Role>
        {
            new() {Name = "Member"},
            new() {Name = "Admin"}
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        foreach (var user in users)
        {
            user.UserName = user.UserName!.ToLower();
            await userManager.CreateAsync(user, "Abc-123");
            await userManager.AddToRoleAsync(user, "Member");
        }

        var admin = new User
        {
            UserName = "admin",
            Firstname = "Admin",
            Lastname = "Admin"
        };

        await userManager.CreateAsync(admin, "Abc-123");
        await userManager.AddToRolesAsync(admin, ["Admin"]);
    }
}
