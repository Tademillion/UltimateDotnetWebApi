var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(); // this is for minimal apis
//  configuring the cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                "http://10.100.13.44:3004",
                "https://10.100.13.44:3004",
                "http://localhost:3004",
                "http://172.16.239.172:3000"
                )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});
builder.Services.AddControllers();
// Configure the HTTP request pipeline.
//  wnat to configure for iis server to deploy on it
builder.Services.Configure<IISOptions>(options =>
{
    // configure for IIS Deployments and have many properties

});
builder.Logging.ClearProviders();// clear the default providers
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
var logger = app.Logger;
logger.LogInformation("the application is running");
//  middlewares order
app.UseCors("allowAllOrigin");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


//  minimal Api
app.MapGet("/", () => "this is test for hello");


app.Run();
