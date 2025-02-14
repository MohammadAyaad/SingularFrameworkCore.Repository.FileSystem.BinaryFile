namespace SingularFrameworkCore.Repository.FileSystem.BinaryFile;

public class BinaryFileRepository : ISingularCrudRepository<byte[]>
{
    public string Path { get; }

    public BinaryFileRepository(string path)
    {
        this.Path = path;
    }

    public void Create(byte[] entity)
    {
        if (!File.Exists(this.Path))
        {
            FileStream fs = File.Create(this.Path);
            fs.Write(entity);
            fs.Close();
        }
        else
            throw new BinaryFileRepositoryFileAlreadyExistsException("File already exists");
    }

    public byte[] Read()
    {
        return File.ReadAllBytes(this.Path);
    }

    public void Update(byte[] newEntity)
    {
        File.WriteAllBytes(this.Path, newEntity);
    }

    public void Delete()
    {
        if (File.Exists(this.Path))
            File.Delete(this.Path);
    }
}

[Serializable]
public class BinaryFileRepositoryFileAlreadyExistsException : Exception
{
    public BinaryFileRepositoryFileAlreadyExistsException() { }

    public BinaryFileRepositoryFileAlreadyExistsException(string message)
        : base(message) { }

    public BinaryFileRepositoryFileAlreadyExistsException(string message, Exception inner)
        : base(message, inner) { }

    protected BinaryFileRepositoryFileAlreadyExistsException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context
    ) { }
}
