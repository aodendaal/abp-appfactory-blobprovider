# abp-appfactory-blobprovider
ASP.NET Boilerplate module for connecting to Azure blob storage.

## Setup Dependency

Add the **Abp.AppFactory.Interfaces** and **Abp.AppFactory.Utilities** NuGet packages to your **.Application** project.
```powershell
dotnet new package Abp.AppFactory.Interfaces
dotnet new package Abp.AppFactory.Utilities
```

Add **Abp.AppFactory.BlobStorage** NuGet package to your **.Web.Core** project.
```powershell
dotnet new package Abp.AppFactory.BlobStorage
```
Add **BlobProviderModule** as a dependency in the attributes of the **CoreModule**.
```csharp
[DependsOn(
    ...
    typeof(BlobProviderModule))]
public class DemoCoreModule : AbpModule
{
    ...
}
```

## Create Endpoint
Create a DTO to transfer the details of the file.

```csharp
public class FileDto
{
    public string Filename { get; set; }
    public string Data { get; set; }
}
```

Inject an interface of BlobStorage into your AppService and use the Utilities extension _ToByteArray()_ to convert the DTO string to a byte[].
```csharp
using Abp.AppFactory.Interfaces;
using Abp.AppFactory.Utilities;

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

    public async Task Upload(FileDto input)
    {
        var container = "uploads";

        var bytes = input.Data.ToByteArray();

        await blobStorage.UploadAsync(container, input.Filename, bytes);
    }
}
```

## Uploading
Add a file input in your HTML and bind the _change_ event.

```html
<input (change)="upload($event)">
```

Create a function in the controller to upload the file
```javascript
file: File = null;

upload(event: any): void {
    if (event.target.files[0]) {
        this.file = event.target.files[0];

        let reader = new FileReader();
        
        reader.onload = () => {
            let dto = new FileDto();
            dto.init({
                filename: this.file.name,
                data: reader.result.toString(),
                fileSize: data.length
            });
            
            this.profileService.upload(dto)
                               .subscribe(() => {
                                   abp.notify.success("File uploaded successfully");
                               });
        };
        
        reader.readAsBinaryString(this.file);
    }        
}
```
