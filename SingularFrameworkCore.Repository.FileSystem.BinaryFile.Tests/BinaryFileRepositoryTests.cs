using System.IO;

namespace SingularFrameworkCore.Repository.FileSystem.BinaryFile.Tests
{
    public class BinaryFileRepositoryTests : IDisposable
    {
        private readonly string _tempFilePath;
        private readonly string _tempDirectory;

        public BinaryFileRepositoryTests()
        {
            // Create a unique temp directory for each test
            _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDirectory);
            _tempFilePath = Path.Combine(_tempDirectory, "test.bin");
        }

        public void Dispose()
        {
            // Clean up temp directory after each test
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, recursive: true);
            }
        }

        // --- Tests for Constructor ---
        [Fact]
        public void Constructor_SetsPathAndOverwrite()
        {
            var repository = new BinaryFileRepository("test.bin", overwrite: true);
            Assert.Equal("test.bin", repository.Path);
            Assert.True(repository.Overwrite);
        }

        // --- Tests for Create ---
        [Fact]
        public void Create_WhenFileDoesNotExist_CreatesFile()
        {
            // Arrange
            var repository = new BinaryFileRepository(_tempFilePath);
            byte[] data = [0x01, 0x02];

            // Act
            repository.Create(data);

            // Assert
            Assert.True(File.Exists(_tempFilePath));
            Assert.Equal(data, File.ReadAllBytes(_tempFilePath));
        }

        [Fact]
        public void Create_WhenFileExistsAndOverwriteFalse_ThrowsException()
        {
            // Arrange
            File.WriteAllBytes(_tempFilePath, [0x03]); // Create existing file
            var repository = new BinaryFileRepository(_tempFilePath, overwrite: false);
            byte[] data = [0x01, 0x02];

            // Act & Assert
            var ex = Assert.Throws<BinaryFileRepositoryFileAlreadyExistsException>(
                () => repository.Create(data)
            );
            Assert.Equal("File already exists", ex.Message);
            Assert.Equal([0x03], File.ReadAllBytes(_tempFilePath)); // File unchanged
        }

        [Fact]
        public void Create_WhenFileExistsAndOverwriteTrue_OverwritesFile()
        {
            // Arrange
            File.WriteAllBytes(_tempFilePath, [0x03]); // Existing file
            var repository = new BinaryFileRepository(_tempFilePath, overwrite: true);
            byte[] data = [0x01, 0x02];

            // Act
            repository.Create(data);

            // Assert
            Assert.Equal(data, File.ReadAllBytes(_tempFilePath));
        }

        // --- Tests for Read ---
        [Fact]
        public void Read_WhenFileExists_ReturnsData()
        {
            // Arrange
            byte[] expected = [0x01, 0x02];
            File.WriteAllBytes(_tempFilePath, expected);
            var repository = new BinaryFileRepository(_tempFilePath);

            // Act
            byte[] actual = repository.Read();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_WhenFileDoesNotExist_ThrowsFileNotFoundException()
        {
            // Arrange
            var repository = new BinaryFileRepository(_tempFilePath);

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => repository.Read());
        }

        // --- Tests for Update ---
        [Fact]
        public void Update_OverwritesFileContent()
        {
            // Arrange
            byte[] initialData = [0x01];
            byte[] newData = [0x02];
            File.WriteAllBytes(_tempFilePath, initialData);
            var repository = new BinaryFileRepository(_tempFilePath);

            // Act
            repository.Update(newData);

            // Assert
            Assert.Equal(newData, File.ReadAllBytes(_tempFilePath));
        }

        // --- Tests for Delete ---
        [Fact]
        public void Delete_WhenFileExists_RemovesFile()
        {
            // Arrange
            File.WriteAllBytes(_tempFilePath, [0x01]);
            var repository = new BinaryFileRepository(_tempFilePath);

            // Act
            repository.Delete();

            // Assert
            Assert.False(File.Exists(_tempFilePath));
        }

        [Fact]
        public void Delete_WhenFileDoesNotExist_DoesNothing()
        {
            // Arrange
            var repository = new BinaryFileRepository(_tempFilePath);

            // Act
            repository.Delete();

            // Assert
            Assert.False(File.Exists(_tempFilePath));
        }
    }
}
