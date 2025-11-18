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

} // this is for searching the employees by name
//  public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQueryString)
//  {
//   if (string.IsNullOrWhiteSpace(orderByQueryString))
//   return employees.OrderBy(e => e.Name);
//   var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);
//   return employees.OrderBy(orderQuery);
//  }
//  // this is for sorting the employees by name
 
   }