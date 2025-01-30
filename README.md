# SingularFrameworkCore.Repository.FileSystem.BinaryFile

A C# library that provides binary file storage implementation for the SingularFrameworkCore repository interface. This library offers both synchronous and asynchronous implementations for storing string data in binary files.

## Features

- **Binary File Storage**: Simple implementation for storing string data in binary files
- **Dual Implementation**: Both synchronous (`BinaryFileRepository`) and asynchronous (`BinaryFileRepositoryAsync`) versions
- **CRUD Operations**: Full support for Create, Read, Update, and Delete operations
- **Path Management**: Secure file path handling
- **Exception Handling**: Custom exceptions for common scenarios

## Installation

The package is available on NuGet. To install it, use the following command:

```bash
Install-Package SingularFrameworkCore.Repository.FileSystem.BinaryFile
```

Or using the .NET CLI:

```bash
dotnet add package SingularFrameworkCore.Repository.FileSystem.BinaryFile
```

## Usage

### Synchronous Implementation

```csharp
using SingularFrameworkCore.Repository.FileSystem.BinaryFile;

// Create an instance with a file path
var repository = new BinaryFileRepository("path/to/your/file.txt");

// Create
repository.Create("Hello, World!");

// Read
string content = repository.Read();

// Update
repository.Update("Updated content");

// Delete
repository.Delete();
```

### Asynchronous Implementation

```csharp
using SingularFrameworkCore.Repository.FileSystem.BinaryFile;

// Create an instance with a file path
var repository = new BinaryFileRepositoryAsync("path/to/your/file.txt");

// Create
await repository.Create("Hello, World!");

// Read
string content = await repository.Read();

// Update
await repository.Update("Updated content");

// Delete
await repository.Delete();
```

## Integration with SingularFrameworkCore

This library implements the `ISingularCrudRepository<string>` and `ISingularCrudAsyncRepository<string>` interfaces from SingularFrameworkCore, making it perfect for use with the Singular pipeline:

```csharp
var singular = new Singular<MyClass, string>(
    new BinaryFileRepository("data.txt"), // or BinaryFileRepositoryAsync
    serializer,
    preProcessors,
    postProcessors
);
```

## Exception Handling

The library includes a custom exception:

- `BinaryFileRepositoryFileAlreadyExistsException`: Thrown when attempting to create a file that already exists

```csharp
try 
{
    repository.Create("content");
}
catch (BinaryFileRepositoryFileAlreadyExistsException ex)
{
    // Handle the case where the file already exists
}
```

## Requirements

- .NET Standard 8.0+ 
- SingularFrameworkCore

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.

## Author
###  Made by [Mohammad Ayaad](https://github.com/MohammadAyaad) ![MohammadAyaad](https://img.shields.io/static/v1?label=|&message=MohammadAyaad&color=grey&logo=github&logoColor=white)

## Original Project

- [SingularFrameworkCore](https://github.com/MohammadAyaad/SingularFrameworkCore) - The core framework this implementation is built for