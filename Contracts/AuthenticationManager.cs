using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

public class AuthenticationManager : IAuthenticationManager 
{ 
    private readonly UserManager<User> _userManager; 
    private readonly IConfiguration _configuration; 
     private User _user; 
 
    public AuthenticationManager(UserManager<User> userManager, IConfiguration 
configuration) 
    { 
        _userManager = userManager; 
        _configuration = configuration; 
    } 
 
    public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth) 
    { 
       _user = await _userManager.FindByNameAsync(userForAuth.UserName);  

        return (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password)); 
    } 
 
    public async Task<string> CreateToken() 
    { 
        var signingCredentials = GetSigningCredentials(); 
        var claims = await GetClaims(); 
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims); 
 
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions); 
    } 
 
    private SigningCredentials GetSigningCredentials() 
    { 
        var JwtSettings = _configuration.GetSection("jwt"); 
        var configurat = JwtSettings["secretKey"];
        var key =Encoding.UTF8.GetBytes(configurat); 
        var secret = new SymmetricSecurityKey(key); 
 
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256); 
    } 
 
    private async Task<List<Claim>> GetClaims() 
    { 
        var claims = new List<Claim> 
        { 
            new Claim(ClaimTypes.Name, _user.UserName,_user.LastName) 
        }; 
 
        var roles = await _userManager.GetRolesAsync(_user); 
        foreach (var role in roles) 
        { 
            claims.Add(new Claim(ClaimTypes.Role, role)); 
        } 
 
        return claims; 
    } 
 
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials 
signingCredentials, List<Claim> claims) 
    { 
        var jwtSettings = _configuration.GetSection("jwt"); 
 
        var tokenOptions = new JwtSecurityToken 
        ( 
            issuer: jwtSettings["validIssuer"], 
            audience: jwtSettings["validAudience"], 
            claims: claims, 
            expires: 
DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])), 
            signingCredentials: signingCredentials 
        ); 
        return tokenOptions; 
    } 
} 
