using Microsoft.EntityFrameworkCore;
using MyApp;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi(); // this is for minimal apis
                                       //  configuring the cors policy
                                       //  configure serilog
                                       // Log.Logger = new LoggerConfiguration()
                                       //             .MinimumLevel.Debug() // Set the minimum logging level
                                       //             .WriteTo.Console()    // Log to the console
                                       //             .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // Log to a file, rolling daily
                                       //             .CreateLogger();

        //  register the services
        builder.Services.AddScoped<ISendMessage, SendEmailMessage>();
        builder.Services.AddScoped<ISendMessage, SendsmsMessage>();
        // builder.Host.UseSerilog();
        builder.Services.AddScoped<IEmployeeRepo, EmployeeRepos>();
        builder.Services.AddScoped<IEmployeeCleanRepo, EmployeeCleanRepo>();
        //  configure the Dbcontext Classs
        builder.Services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();// register the services
                                                                            //  adda ctionfilter controller levels

        builder.Services.AddScoped<ValidationFilterAttribute>();

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
        }).AddNewtonsoftJson()// for patch requests
        .AddXmlDataContractSerializerFormatters();// used to chnage only json to different response types
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
        // builder.Services.AddControllers();
        // Configure the HTTP request pipeline.
        //  wnat to configure for iis server to deploy on it
        builder.Services.Configure<IISOptions>(options =>
        {
            // configure for IIS Deployments and have many properties

        });
        // builder.Logging.ClearProviders(); 
        // builder.Logging.AddConsole();
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        //  middlewares order
        app.UseCors("allowAllOrigin");
        // app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseGlobalExceptionHandler();//
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
    }
}