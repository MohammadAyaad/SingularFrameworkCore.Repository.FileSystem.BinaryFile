namespace SingularFrameworkCore.Repository.FileSystem.BinaryFile;

public class BinaryFileRepositoryAsync : ISingularCrudAsyncRepository<byte[]>
{
    public string Path { get; }
    public bool Overwrite { get; set; }

    public BinaryFileRepositoryAsync(string path, bool overwrite = false)
    {
        this.Path = path;
        this.Overwrite = overwrite;
    }

    public async Task Create(byte[] entity)
    {
        if (!File.Exists(this.Path))
        {
            FileStream fs = File.Create(this.Path);
            await fs.WriteAsync(entity);
            fs.Close();
        }
        else
        {
            if (this.Overwrite)
            {
                File.Delete(this.Path);
                FileStream fs = File.Create(this.Path);
                await fs.WriteAsync(entity);
                fs.Close();
            }
            else
                throw new BinaryFileRepositoryFileAlreadyExistsException("File already exists");
        }
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
