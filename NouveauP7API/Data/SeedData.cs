using Microsoft.AspNetCore.Identity;
using NouveauP7API.Data;
using NouveauP7API.Models;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var dbContext = serviceProvider.GetRequiredService<LocalDbContext>();

        await SeedRoles(roleManager, dbContext);
        await SeedUsers(userManager, dbContext);
        // Ajoutez d'autres méthodes de séquençage si nécessaire
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager, LocalDbContext dbContext)
    {
        string[] roles = { "Admin", "User", "RH" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }


    private static async Task SeedUsers(UserManager<User> userManager, LocalDbContext dbContext)
    {
        // Vérifiez si l'utilisateur admin existe, sinon créez-le
        var adminUser = await userManager.FindByNameAsync("admin");
        if (adminUser == null)
        {
            var newAdmin = new User();
            newAdmin.Email = "admin@admin.fr";
            newAdmin.EmailConfirmed = true;

            // Ajoutez l'utilisateur admin à la base de données
            await userManager.CreateAsync(newAdmin, "MotDePasseAdmin123"); // Remplacez "MotDePasseAdmin123" par le mot de passe souhaité
            var result = await userManager.CreateAsync(newAdmin, "YourSecurePassword");

            if (result.Succeeded)
            {
                // Ajoutez l'utilisateur au rôle 'Admin'
                await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
            // Gérez les erreurs si la création de l'utilisateur échoue
        }
    }
}
