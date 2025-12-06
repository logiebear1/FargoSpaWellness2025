using Microsoft.AspNetCore.Identity;

namespace FargoSpaWellness.Models;

public class Booking
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public IdentityUser? User { get; set; }

    public int ServiceId { get; set; }
    public SpaService? Service { get; set; }

    public DateTime AppointmentDateTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string Status { get; set; } = "Confirmed"; // Confirmed, Cancelled, Completed
    public string Notes { get; set; } = string.Empty;
}
