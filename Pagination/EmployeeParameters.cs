public class RequestParameters
{
    const int maxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
            //  this is only to restrict the maxPageNumber
        }
    }
}
    
public class EmployeeParameters : RequestParameters
{
    //  for examples if we want to add more parameters specific to employee
    public uint MinAge { get; set; }
    public uint MaxAge { get; set; } = 65;
    public bool ValidAgeRange => MaxAge > MinAge;
    public string searchTerm { get; set; }
}