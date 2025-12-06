using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FargoSpaWellness.Models;

namespace FargoSpaWellness.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<SpaService> Services => Set<SpaService>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Testimonial> Testimonials => Set<Testimonial>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Fix decimal precision for Price (and any future money fields)
        builder.Entity<SpaService>()
            .Property(s => s.Price)
            .HasColumnType("decimal(18,2)");
    }
}
