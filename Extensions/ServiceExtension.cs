using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

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
//  
public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MyApp API",
                
                Description = "API for MyApp Application"
            });
            
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        }
        );
    
    }
    
}