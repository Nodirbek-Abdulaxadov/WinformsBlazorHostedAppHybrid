namespace Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class TodoView
{
    [DataMember] public string Id { get; set; } = string.Empty;
    [DataMember] public string Name { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        var other = obj as TodoView;
        if (obj == null || other == null)
            return false;

        return other.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id is null ? 0 : Id.GetHashCode();
    }
}