using Microsoft.EntityFrameworkCore;

public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions options)
    : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
 
     }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Employee> Employees { get; set; }
    //  creating migration in dotnet core 
    //  install EF core for Dbcontext 
    //  creating models for  our tables 
    //  the craate dbcontext class for our models 
    //  then install the ef.design for acces  the migration providers
    //  and install the sqlServer for acces the sqlserver providers 
    //  then  add the dbcontext service in program.cs for sake of registry
    //  then run dotnet ef migrations add  migrationName
    //  dotnet database update 
}