using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class ServiceExtensions
{
    public static void ConfigureJWT(this IServiceCollection services, IConfiguration 
configuration) 
{ 
var jwtSettings = configuration.GetSection("JwtSettings"); 
var secretKey = Environment.GetEnvironmentVariable("SECRET"); 
services.AddAuthentication(opt => { 
opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 
}) 
.AddJwtBearer(options => 
{ 
options.TokenValidationParameters = new TokenValidationParameters 
{ 
ValidateIssuer = true, // server created the token
ValidateAudience = true, // the valid allowed client get the token
ValidateLifetime = true, // token is not expired
ValidateIssuerSigningKey = true, // key is valid
ValidIssuer = jwtSettings.GetSection("validIssuer").Value, 
ValidAudience = jwtSettings.GetSection("validAudience").Value, 
IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) 
}; 
}); 
} 

}