using System.IO;
using System.Threading.Tasks;

namespace SingularFrameworkCore.Repository.FileSystem.BinaryFile.Tests
{
    public class BinaryFileRepositoryAsyncTests : IDisposable
    {
        private readonly string _tempFilePath;
        private readonly string _tempDirectory;

        public BinaryFileRepositoryAsyncTests()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDirectory);
            _tempFilePath = Path.Combine(_tempDirectory, "test.bin");
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, recursive: true);
            }
        }

        // --- Create Tests ---
        [Fact]
        public async Task CreateAsync_WhenFileDoesNotExist_CreatesFile()
        {
            // Arrange
            var repository = new BinaryFileRepositoryAsync(_tempFilePath);
            byte[] data = [0x01, 0x02];

            // Act
            await repository.Create(data);

            // Assert
            Assert.True(File.Exists(_tempFilePath));
            Assert.Equal(data, await File.ReadAllBytesAsync(_tempFilePath));
        }

        [Fact]
        public async Task CreateAsync_WhenFileExistsAndOverwriteFalse_ThrowsException()
        {
            // Arrange
            await File.WriteAllBytesAsync(_tempFilePath, [0x03]);
            var repository = new BinaryFileRepositoryAsync(_tempFilePath, overwrite: false);
            byte[] data = [0x01, 0x02];

            // Act & Assert
            await Assert.ThrowsAsync<BinaryFileRepositoryFileAlreadyExistsException>(
                () => repository.Create(data)
            );
            Assert.Equal([0x03], await File.ReadAllBytesAsync(_tempFilePath));
        }

        [Fact]
        public async Task CreateAsync_WhenFileExistsAndOverwriteTrue_OverwritesFile()
        {
            // Arrange
            await File.WriteAllBytesAsync(_tempFilePath, [0x03]);
            var repository = new BinaryFileRepositoryAsync(_tempFilePath, overwrite: true);
            byte[] data = [0x01, 0x02];

            // Act
            await repository.Create(data);

            // Assert
            Assert.Equal(data, await File.ReadAllBytesAsync(_tempFilePath));
        }

        // --- Read Tests ---
        [Fact]
        public async Task ReadAsync_WhenFileExists_ReturnsData()
        {
            // Arrange
            byte[] expected = [0x01, 0x02];
            await File.WriteAllBytesAsync(_tempFilePath, expected);
            var repository = new BinaryFileRepositoryAsync(_tempFilePath);

            // Act
            byte[] actual = await repository.Read();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ReadAsync_WhenFileDoesNotExist_ThrowsFileNotFoundException()
        {
            // Arrange
            var repository = new BinaryFileRepositoryAsync(_tempFilePath);

            // Act & Assert
            await Assert.ThrowsAsync<FileNotFoundException>(() => repository.Read());
        }

        // --- Update Tests ---
        [Fact]
        public async Task UpdateAsync_OverwritesFileContent()
        {
            // Arrange
            byte[] initialData = [0x01];
            byte[] newData = [0x02];
            await File.WriteAllBytesAsync(_tempFilePath, initialData);
            var repository = new BinaryFileRepositoryAsync(_tempFilePath);

            // Act
            await repository.Update(newData);

            // Assert
            Assert.Equal(newData, await File.ReadAllBytesAsync(_tempFilePath));
        }

        // --- Delete Tests ---
        [Fact]
        public async Task DeleteAsync_WhenFileExists_RemovesFile()
        {
            // Arrange
            await File.WriteAllBytesAsync(_tempFilePath, [0x01]);
            var repository = new BinaryFileRepositoryAsync(_tempFilePath);

            // Act
            await repository.Delete();

            // Assert
            Assert.False(File.Exists(_tempFilePath));
        }

        [Fact]
        public async Task DeleteAsync_WhenFileDoesNotExist_DoesNothing()
        {
            // Arrange
            var repository = new BinaryFileRepositoryAsync(_tempFilePath);

            // Act
            await repository.Delete();

            // Assert
            Assert.False(File.Exists(_tempFilePath));
        }
    }
}
