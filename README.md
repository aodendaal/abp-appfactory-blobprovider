# abp-appfactory-blobprovider
ASP.NET Boilerplate module for connecting to Azure blob storage.

## Installation

## Usage
```csharp
public DemoAppService: AsyncCrudService<DemoEntity, DemoEntityDto>
{
    private readonly IBlobStorage blobStorage;

    public DemoAppService(
        IBlobStorage blobStorage

        IRepository<DemoEntity> repository,
    ) : base(repository)
    { 
        this.blobStorage = blobStorage;
    }

    public async Task Upload(string filename, byte[] file)
    {
        var container = "uploads";

        await blobStorage.UploadAsync(container, filename, file);
    }
}
```