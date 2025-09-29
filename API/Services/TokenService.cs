using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config, UserManager<AppUser> userManager) : ITokenService
{
    public async Task<string> CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"] ?? throw new Exception("Nessun Token Key");
        if (tokenKey.Length < 64)
            throw new Exception("La Token Key deve essere >= di 64 caratteri");

        // chiave per firmare il token. Simmetrica perchè questa chiave viene usata sia per crittare che per decrittare 
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        // informazioni sull'utente che l'utente dichiara su se stesso. 
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        var roles = await userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // Credenziali che si basano sulla chiave generata prima per firmare il token
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        // Queste sono le proprietà e caratteristiche che il token dovrà avere
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(7), // solitamente molto corte, si parla di minuti
            SigningCredentials = creds
        };

        // Questa classe crea il token sulla base del descrittore qui sopra. 
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }
}
