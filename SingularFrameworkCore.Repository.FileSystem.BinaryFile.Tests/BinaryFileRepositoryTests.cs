using System.Reflection;
using System.Text;

namespace SingularFrameworkCore.Repository.FileSystem.BinaryFile.Tests;

public class BinaryFileRepositoryTests
{
    [Fact]
    [InlineData("hello.bin")]
    [InlineData("what.no")]
    public void Create_File_Exists(string fileName)
    {
        var repo = new BinaryFileRepository(fileName);
        repo.Create(new byte[] { });
        Assert.Equals(File.Exists(fileName), true);
    }

    [Fact]
    [InlineData("hello.bin", "Hello World!")]
    [InlineData("what.no", " why not World helo ?")]
    public void Create_File_Data_Reads_Correct(string fileName, string contents)
    {
        var repo = new BinaryFileRepository(fileName);
        repo.Create(new byte[] { });
    }
}
