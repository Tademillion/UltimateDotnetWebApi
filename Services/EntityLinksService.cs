// using System.Dynamic;
// using Microsoft.AspNetCore.Http;

// public class EntityLinksService<T> : IEntityLinksService<T>
// {
//     private readonly LinkGenerator _linkGenerator;
//     private readonly IHttpContextAccessor _httpContextAccessor;

//     public EntityLinksService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
//     {
//         _linkGenerator = linkGenerator;
//         _httpContextAccessor = httpContextAccessor;
//     }

//     public EntityWithLinks<ExpandoObject> CreateLinksForEntity(
//         ExpandoObject entity,
//         object id,
//         string controllerName)
//     {
//         var wrapper = new EntityWithLinks<ExpandoObject>
//         {
//             Value = entity
//         };

//         var httpContext = _httpContextAccessor.HttpContext;
//         string? href = _linkGenerator.GetUriByAction(httpContext, action: "GetById", controller: controllerName, values: new { id });
//         if (!string.IsNullOrWhiteSpace(href)) wrapper.Links.Add(new Link(href, "self", "GET"));

//         href = _linkGenerator.GetUriByAction(httpContext, action: "Update", controller: controllerName, values: new { id });
//         if (!string.IsNullOrWhiteSpace(href)) wrapper.Links.Add(new Link(href, "update", "PUT"));

//         href = _linkGenerator.GetUriByAction(httpContext, action: "Delete", controller: controllerName, values: new { id });
//         if (!string.IsNullOrWhiteSpace(href)) wrapper.Links.Add(new Link(href, "delete", "DELETE"));

//         return wrapper;
//     }

//     public IEnumerable<EntityWithLinks<ExpandoObject>> CreateLinksForEntities(
//         IEnumerable<ExpandoObject> entities,
//         string idField,
//         string controllerName)
//     {
//         var result = new List<EntityWithLinks<ExpandoObject>>();

//         foreach (IDictionary<string, object> shaped in entities)
//         {
//             if (!shaped.TryGetValue(idField, out var id)) continue;
//             result.Add(CreateLinksForEntity((ExpandoObject)shaped, id, controllerName));
//         }

//         return result;
//     }
// }
