using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class ServiceExtensions
{
    public static void ConfigureJWT(this IServiceCollection services, IConfiguration 
configuration) 
{ 
var jwtSettings = configuration.GetSection("jwt"); 
var secretKey = jwtSettings["secretKey"]; 
services.AddAuthentication(opt => { 
opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 
}) 
.AddJwtBearer(options => 
{ 
options.TokenValidationParameters = new TokenValidationParameters 
{ 
    
ValidateIssuer = true, // server created the token
ValidateAudience = false, // the valid allowed client get the token
ValidateLifetime = true, // token is not expired
ValidateIssuerSigningKey = true, // key is valid
ValidIssuer = jwtSettings["validIssuer"], 
ValidAudience = jwtSettings["validAudience"], 
IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) 
}; 
options.Events = new JwtBearerEvents 
{
    OnAuthenticationFailed = context =>
    {
        Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
        return Task.CompletedTask;
    },
    OnChallenge = context =>
    {
       context.HandleResponse(); // Stops the default challenge/redirect behavior
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"message\": \"Authentication failed: Token missing or invalid.\"}");
    },
    OnTokenValidated = context =>
    {
        Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
        return Task.CompletedTask;
    }   
};
}
  
) ;
} 

}