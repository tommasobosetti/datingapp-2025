namespace API.Entities;

public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    //Duplicazione del dato per evitare una join ogni volta tra due tabelle. 
    public string? ImageUrl { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }

    //Navigation property
    public Member Member { get; set; } = null!;
}
