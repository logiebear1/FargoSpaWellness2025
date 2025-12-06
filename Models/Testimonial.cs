namespace FargoSpaWellness.Models;

public class Testimonial
{
    public int Id { get; set; }
    public string AuthorName { get; set; } = "Anonymous";
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; } = 5; // 1–5
    public DateTime DatePosted { get; set; } = DateTime.Now;
    public bool IsApproved { get; set; } = false; // Admin must approve
}
