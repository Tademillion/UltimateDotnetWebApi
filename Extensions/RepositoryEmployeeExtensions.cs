public static class RepositoryEmployeeExtensions {
 
   public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge)=>
   employees.Where(e => e.Age >= minAge && e.Age <= maxAge);
   // 
   
}