using Microsoft.AspNetCore.Identity;

namespace TalentoPlus.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public bool IsAdmin { get; set; } = false;
}