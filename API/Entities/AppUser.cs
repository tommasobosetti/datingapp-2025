using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppUser : IdentityUser
{
    public required string DisplayName { get; set; }
    //Duplicazione del dato per evitare una join ogni volta tra due tabelle. 
    public string? ImageUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }

    //Navigation property
    public Member Member { get; set; } = null!;
}
