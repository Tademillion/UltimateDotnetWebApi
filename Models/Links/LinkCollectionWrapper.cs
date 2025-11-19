// Models/LinkCollectionWrapper.cs
public class LinkCollectionWrapper<T>
{
    public IEnumerable<T> Value { get; set; }
    public List<Link> Links { get; set; } = new List<Link>();

    public LinkCollectionWrapper(IEnumerable<T> value)
    {
        Value = value;
    }
}
