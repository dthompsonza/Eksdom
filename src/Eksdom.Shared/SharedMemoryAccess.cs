using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
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
        using var mutex = new Mutex(initiallyOwned: false, MutexName);
        try
        {
            mutex.WaitOne();
            WriteFile(name, payload);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error on memory write - {ex.Message}");
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }

    private void WriteFile<T>(string name, T payload)
        where T : SerializableMemoryFile
    {
        var file = new MemoryFile<T>(name, payload);
        var json = EksdomJsonSerializer.ToJson(file);
        var bytes = Encoding.UTF8.GetBytes(json);
        if (bytes.Length > ushort.MaxValue)
        {
            throw new InvalidOperationException($"Cannot memory write '{name}', payload too large ({bytes.Length} b)");
        }
        var buffer = AddLengthHeader(bytes);

        var mmf = GetOrCreateMemoryMappedFile(BuildMapName(name), buffer.Length);
        
        
        using var stream = mmf.CreateViewStream();
        stream.Write(buffer, 0, buffer.Length);
        stream.Flush();
    }

    private static byte[] AddLengthHeader(byte[] buffer)
    {
        ushort length = Convert.ToUInt16(buffer.Length);
        byte[] result = new byte[length + 2];

        Array.Copy(length.ToBytesHeader(), 0, result, 0, 2);
        Array.Copy(buffer, 0, result, 2, length);

        return result;
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
        using var mutex = new Mutex(initiallyOwned: false, MutexName);

        try
        {
            mutex.WaitOne();

            var file = ReadFile<T>(name);
            return file;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error on memory read - {ex.Message}");
        }
        finally
        {
            mutex.ReleaseMutex();
        }
        return null;
    }

    private T? ReadFile<T>(string name)
        where T : SerializableMemoryFile
    {
        var mapName = BuildMapName(name);
        if (!FileExists(mapName))
        {
            return default;
        }
        using var mmf = MemoryMappedFile.OpenExisting(mapName, MemoryMappedFileRights.ReadWrite);
        using var stream = mmf.CreateViewStream();

        var streamLength = ushort.MaxValue;
        var dataLength = (ushort)0;
        byte[] data = Array.Empty<byte>();
        var dataIndex = 0;

        while (stream.Position < ushort.MaxValue)
        {
            if (stream.Position >= streamLength)
            {
                break;
            }
            if (stream.Position == 0)
            {
                var header = new byte[2];
                var headerBytes = stream.Read(header, 0, 2);
                dataLength = header.ToUshort();
                streamLength = Convert.ToUInt16(dataLength + headerBytes);
                data = new byte[dataLength];
            }
            var buffer = new byte[Math.Min(1024, (int)(streamLength - stream.Position))];
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            Array.Copy(buffer, 0, data, dataIndex, bytesRead);
            dataIndex += bytesRead;
        }

        if (dataIndex == 0)
        {
            return default;
        }

        var json = Encoding.UTF8.GetString(data, 0, data.Length);
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
