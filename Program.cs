using FargoSpaWellness.Components;
using FargoSpaWellness.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

// 2. Identity + Roles + UI (now works after package install)
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultUI();

// 3. Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();  // ← Added for .NET 9 Identity routing
app.UseAuthentication();  // ← Required for login to work
app.UseAuthorization();   // ← Required for roles (Admin)

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map Razor Pages for Identity UI (Login/Register)
app.MapRazorPages();

// Seed data on every startup
// Seed data on every startup (no auto-migrate—handle manually via PMC)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // DON'T auto-migrate here—run via PMC instead to avoid startup crashes
    // var context = services.GetRequiredService<ApplicationDbContext>();
    // context.Database.Migrate();

    await SeedData.Initialize(services);
}

app.Run();
