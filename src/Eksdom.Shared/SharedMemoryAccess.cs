using System.IO.MemoryMappedFiles;
using System.Text;
using Eksdom.Shared;

namespace Eksdom.SharedMemory;

public sealed class SharedMemoryAccess : IDisposable
{
    private const string MutexName = "Eksdom.SharedMemory.Mutex";
    private string _memoryName;
    private Dictionary<string, MemoryMappedFile> _mmfWriters;

    public SharedMemoryAccess(string memoryName)
    {
        _memoryName = memoryName;
        _mmfWriters = new Dictionary<string, MemoryMappedFile>();
    }

    public void Write<T>(string name, T payload)
        where T : SerializableMemoryFile
    {
        var file = new MemoryFile<T>(name, payload);
        //var options = new JsonSerializerOptions()
        //{
        //    Converters =
        //    {
        //        new DateOnlyJsonConverter(),
        //        new TimePeriodJsonConverter()
        //    }
        //};
        //var json = JsonSerializer.Serialize(file,options);
        var json = EksdomJsonSerializer.ToJson(file);
        var buffer = Encoding.UTF8.GetBytes(json);

        var mmf = GetOrCreateMemoryMappedFile(BuildMapName(name), buffer.Length);
        using var mutex = new Mutex(initiallyOwned: false, MutexName);
        mutex.WaitOne();
        using var stream = mmf.CreateViewStream();
        stream.Write(buffer, 0, buffer.Length);
        stream.Flush();
    }

    private MemoryMappedFile GetOrCreateMemoryMappedFile(string mapName, int bufferLength)
    {
        if (!_mmfWriters.ContainsKey(mapName))
        {
            _mmfWriters.Add(mapName, MemoryMappedFile.CreateOrOpen(mapName, bufferLength, MemoryMappedFileAccess.ReadWrite));
        }
        
        return _mmfWriters[mapName];
    }

    private bool FileExists(string mapName)
    {
        try
        {
            using var mmf = MemoryMappedFile.OpenExisting(mapName, MemoryMappedFileRights.ReadWrite);
            return true;
        }
        catch (FileNotFoundException)
        {
            return false;
        }
    }

    public T? Read<T>(string name)
        where T : SerializableMemoryFile
    {
        var mapName = BuildMapName(name);
        if (!FileExists(mapName))
        {
            return default;
        }
        using var mmf = MemoryMappedFile.OpenExisting(mapName, MemoryMappedFileRights.ReadWrite);
        using var mutex = new Mutex(initiallyOwned: false, MutexName);
        mutex.WaitOne();
        using var stream = mmf.CreateViewStream();

        var jsonBytes = new List<byte>();

        while (stream.Position < stream.Length)
        {
            var buffer = new byte[Math.Min(1024, (int)(stream.Length - stream.Position))];
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            jsonBytes.AddRange(buffer.Take(bytesRead));
        }

        mutex.ReleaseMutex();

        if (jsonBytes.Count == 0)
        {
            return default;
        }

        var json = Encoding.UTF8.GetString(jsonBytes.ToArray(), 0, jsonBytes.IndexOf((byte)0));
        //var options = new JsonSerializerOptions()
        //{
        //    Converters = 
        //    { 
        //        new DateOnlyJsonConverter(), 
        //        new TimePeriodJsonConverter() 
        //    }
        //};
        //var file = JsonSerializer.Deserialize<MemoryFile<T>>(json, options);
        var file = EksdomJsonSerializer.FromJson<MemoryFile<T>>(json);
        return file!.Payload;
    }

    private string BuildMapName(string name)
    {
        var mapName = SanitizeName($"{_memoryName}-{name}");
        return mapName;
    }

    private static string SanitizeName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitizedFileName = new StringBuilder(fileName.Length);

        foreach (var c in fileName)
        {
            if (!invalidChars.Contains(c))
            {
                sanitizedFileName.Append(c);
            }
            else
            {
                sanitizedFileName.Append("_");
            }
        }

        return sanitizedFileName.ToString();
    }

    public void Dispose()
    {
        foreach (var kvp in _mmfWriters)
        {
            kvp.Value?.Dispose();
        }
    }
}
