using System.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public class EmployeeLinks
{
    private readonly LinkGenerator _linkGenerator;

    public EmployeeLinks(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    // Create links for a single employee
    public List<Link> CreateLinksForEmployee(HttpContext httpContext, Guid companyId, Guid employeeId)
    {
        return new List<Link>
        {
            new Link(
                href: _linkGenerator.GetUriByAction(httpContext,action: "GetEmployeeForCompany",
                    controller: null,
                    values: new { companyId, id = employeeId }),
                rel: "self",
                method: "GET"),

            new Link(
                href: _linkGenerator.GetUriByAction(
                    httpContext,
                    action: "UpdateEmployeeForCompany",
                    controller: null,
                    values: new { companyId, id = employeeId }),
                rel: "update_employee",
                method: "PUT"),

            new Link(
                href: _linkGenerator.GetUriByAction(
                    httpContext,
                    action: "DeleteEmployeeForCompany",
                    controller: null,
                    values: new { companyId, id = employeeId }),
                rel: "delete_employee",
                method: "DELETE")
        };
    }

    // Create links for each shaped employee in a collection
    public List<EntityWithLinks<ExpandoObject>> TryGenerateLinks(
        IEnumerable<ExpandoObject> shapedEmployees,
        IEnumerable<EmployeeDto> employeesDto,
        HttpContext httpContext,
        Guid companyId)
    {
        var shapedList = shapedEmployees.ToList();
        var dtoList = employeesDto.ToList();

        var linkedList = new List<EntityWithLinks<ExpandoObject>>();

        for (int i = 0; i < dtoList.Count; i++)
        {
            var dto = dtoList[i];
            var shaped = shapedList[i];

            var links = CreateLinksForEmployee(httpContext, companyId, dto.Id);

            linkedList.Add(new EntityWithLinks<ExpandoObject>
            {
                Value = shaped,
                Links = links
            });
        }

        return linkedList;
    }
}
