using System.Dynamic;

public interface IEntityLinksService<T>
{
    EntityWithLinks<ExpandoObject> CreateLinksForEntity(
        ExpandoObject entity, 
        object id,
        string controllerName);

    IEnumerable<EntityWithLinks<ExpandoObject>> CreateLinksForEntities(
        IEnumerable<ExpandoObject> entities,
        string idField,
        string controllerName);
}
