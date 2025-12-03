using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
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
        builder.Services.AddScoped<IDataShaper<EmployeeDto>,DataShaper<EmployeeDto>>();
        builder.Services.AddScoped<ValidationFilterAttribute>();
        // HATEOAS support
        // builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<EmployeeLinks>();
        // builder.Services.AddScoped(typeof(IEntityLinksService<>), typeof(EntityLinksService<>));
          builder.Services.AddApiVersioning(opt=>{// we can add versioning to the extensions as well in service extensions
            opt.ReportApiVersions=true;
            opt.AssumeDefaultVersionWhenUnspecified=true;
            opt.DefaultApiVersion=new ApiVersion(1,0);
            opt.ApiVersionReader= new HeaderApiVersionReader("api-version");
          });
        //   add rate linmiitng
        builder.Services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                return RateLimitPartition.GetFixedWindowLimiter(partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown", factory: partition => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 30,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0
                });
            });
            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = 429;
                await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken);
            };});
            // 
        //  builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
         builder.Services.AddAutoMapper(typeof(MappingProfile));
        //  add identity services
        //  let add identity 
        builder.Services.AddIdentity<User, IdentityRole>(option =>
        {
            //  here is place where restric the password options
            option.Password.RequireDigit = true;
            option.Password.RequireLowercase = true;
            option.Password.RequireUppercase = true;
            option.Password.RequireNonAlphanumeric = false;
            option.Password.RequiredLength = 6;
            option.User.RequireUniqueEmail = true; 
        }).AddEntityFrameworkStores<RepositoryContext>()
        .AddDefaultTokenProviders();
        //
        builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();
        builder.Services.ConfigureJWT(builder.Configuration);
         builder.Services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
            // config.Filters.Add(typeof(ValidationFilterAttribute)); this apply thr model state filteration over  all the projects 
            //  its cool
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
        // builder.Services.AddAuthentication();
        // builder.Logging.ClearProviders(); 
        // builder.Logging.AddConsole();
        // builder.Services.AddEndpointsApiExplorer();
        builder.Services.ConfigureSwagger();       
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
        app.UseSwagger();
        app.UseSwaggerUI();
       app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseRateLimiter();

        app.MapControllers();
        // app.UseHttpsRedirection();
        var summaries = new[]
        {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
        //  minimal Api
        app.MapGet("/", () => "this is test for hello"); 

        app.Run();
    }
}