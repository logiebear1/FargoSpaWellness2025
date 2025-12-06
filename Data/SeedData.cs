using Microsoft.AspNetCore.Identity;
using FargoSpaWellness.Models;
using Microsoft.EntityFrameworkCore;

namespace FargoSpaWellness.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // ← THIS LINE FIXES THE TRANSIENT FAILURE
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {
                // 1. Create Admin role
                if (!await roleManager.RoleExistsAsync("Admin"))
                    await roleManager.CreateAsync(new IdentityRole("Admin"));

                // 2. Seed Services
                if (!context.Services.Any())
                {
                    context.Services.AddRange(
                        new SpaService { Name = "Swedish Massage", Description = "Classic relaxing massage", Price = 95m, DurationMinutes = 60 },
                        new SpaService { Name = "Deep Tissue Massage", Description = "Targets muscle knots", Price = 120m, DurationMinutes = 75 },
                        new SpaService { Name = "Hot Stone Therapy", Description = "Heated stones for deep relaxation", Price = 140m, DurationMinutes = 90 },
                        new SpaService { Name = "Aromatherapy Facial", Description = "Rejuvenating skin treatment", Price = 110m, DurationMinutes = 60 }
                    );
                    await context.SaveChangesAsync();
                }

                // 3. Create Admin user
                var adminEmail = "admin@fargospa.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                    await userManager.CreateAsync(adminUser, "Admin123!");
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        });
    }
}
