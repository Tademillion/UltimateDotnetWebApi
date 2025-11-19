using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Linq.Dynamic.Core;

public static class RepositoryEmployeeExtensions {
 
   public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge)=>
   employees.Where(e => e.Age >= minAge && e.Age <= maxAge);
   // this  is for calling the extension method direclty like  a built in orderBy and others
 public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string searchTerm)
{
   if(string.IsNullOrWhiteSpace(searchTerm))
   return employees;
   var lowerCaseTerm = searchTerm.Trim().ToLower();
  return employees.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));

} 
 public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQueryString)
 {
    var orderQueryBuilder= new StringBuilder();
  if(string.IsNullOrWhiteSpace(orderByQueryString))
  return employees.OrderBy(e => e.Name);
    var orderParams = orderByQueryString.Trim().Split(','); 
    var propertyInfos = typeof(Employee).GetProperties(BindingFlags.Public |BindingFlags.Instance);
    // read all public properties of the employees
    Console.WriteLine($"the param is {orderParams.ToList()}");
foreach (var param in orderParams) 
    { 
        if (string.IsNullOrWhiteSpace(param)) 
            continue; 
 
        var propertyFromQueryName = param.Split(" ")[0]; 
        var objectProperty = propertyInfos.FirstOrDefault(pi => 
              pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase)); 
        if (objectProperty == null) 
            continue; 
            Console.WriteLine($"the ending of the param is {param}");
            var direction = param.EndsWith(" desc") ? "descending" : "ascending";  
            orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction}, "); 
    }   
        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' '); 
      if (string.IsNullOrWhiteSpace(orderQuery)) 
        return employees.OrderBy(e => e.Name); 
   Console.WriteLine($"the order by is {orderQuery}");
    return employees.OrderBy(orderQuery); 
    }
 

   }