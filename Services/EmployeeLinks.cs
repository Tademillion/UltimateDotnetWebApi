public class EmployeeLinks
{
    private readonly LinkGenerator _linkGenerator;

    public EmployeeLinks(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    public List<Link> CreateLinksForEmployee(HttpContext httpContext, int employeeId)
    {
        var links = new List<Link>
        {
            new Link(
                href: _linkGenerator.GetPathByAction(httpContext, action: "GetEmployee", controller: "Employees", values: new { id = employeeId }),
                rel: "self",
                method: "GET"
            ),
            new Link(
                href: _linkGenerator.GetPathByAction(httpContext, action: "UpdateEmployee", controller: "Employees", values: new { id = employeeId }),
                rel: "update_employee",
                method: "PUT"
            ),
            new Link(
                href: _linkGenerator.GetPathByAction(httpContext, action: "DeleteEmployee", controller: "Employees", values: new { id = employeeId }),
                rel: "delete_employee",
                method: "DELETE"
            )
        };

        return links;
    }
}

