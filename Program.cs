var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(); // this is for minimal apis

var app = builder.Build();
//  configuring the cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("allowAllOrigin", policy =>
    {
        policy.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .AllowCredentials()
        .WithOrigins(
            "http://10.100.13.44:3004",
            "https://10.100.13.44:3004",
            "http://localhost:3004",
            "http://172.16.239.172:3000"
        );
    });
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


//  minimal Api
app.MapGet("/api", () => "this is test for hello");


app.Run();
