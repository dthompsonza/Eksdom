namespace Eksdom.Shared;

internal class MemoryFile<T>
    where T : SerializableMemoryFile
{
    public string Name { get; private set; }

    public T Payload { get; private set; }

    public DateTimeOffset CreatedWhen { get;private set; }

    public MemoryFile(string name, T payload, DateTimeOffset createdWhen = default)
    {
        Name = name;
        Payload = payload;
        CreatedWhen = createdWhen == default ? DateTimeOffset.Now : createdWhen;
    }
}