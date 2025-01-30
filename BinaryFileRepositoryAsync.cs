namespace SingularFrameworkCore.Repository.FileSystem.BinaryFile;

public class BinaryFileRepositoryAsync : ISingularCrudAsyncRepository<byte[]>
{
    public string Path { get; }

    public BinaryFileRepositoryAsync(string path)
    {
        this.Path = path;
    }

    public async Task Create(byte[] entity)
    {
        if (!File.Exists(this.Path))
        {
            File.Create(this.Path);
            await File.WriteAllBytesAsync(this.Path, entity);
        }
        else
            throw new BinaryFileRepositoryFileAlreadyExistsException("File already exists");
    }

    public async Task<byte[]> Read()
    {
        return await File.ReadAllBytesAsync(this.Path);
    }

    public async Task Update(byte[] newEntity)
    {
        await File.WriteAllBytesAsync(this.Path, newEntity);
    }

    public Task Delete()
    {
        if (File.Exists(this.Path))
            File.Delete(this.Path);
        return Task.CompletedTask;
    }
}
