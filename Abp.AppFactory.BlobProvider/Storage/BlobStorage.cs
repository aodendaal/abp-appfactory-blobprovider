using Abp.AppFactory.BlobProvider.Configuration;
using Abp.Dependency;
using HashidsNet;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Abp.AppFactory.BlobProvider.Storage
{
    public class BlobStorage : ITransientDependency
    {
        private readonly BlobConfiguration config;

        public CloudBlobClient BlobClient { get; set; }

        public BlobStorage(BlobConfiguration config)
        {
            this.config = config;

            var blobAccount = new CloudStorageAccount(
                new StorageCredentials(config.AccountName, config.AccountKey),
                config.DefaultEndpointsProtocol == "https");

            BlobClient = blobAccount.CreateCloudBlobClient();
        }

        public async Task UploadAsync(string containerName, string directory, string fileName, byte[] file)
        {
            var blob = await GetBlobAsync(containerName, directory, fileName);

            await blob.UploadFromByteArrayAsync(file, 0, file.Length);
        }

        public async Task DeleteAsync(string containerName, string directory, string fileName)
        {
            var blob = await GetBlobAsync(containerName, directory, fileName);

            await blob.DeleteIfExistsAsync();
        }

        public async Task<CloudBlockBlob> GetBlobAsync(string containerName, string directory, string fileName)
        {
            var container = await GetOrCreateContainer(containerName);

            return container.GetBlockBlobReference($"{directory}/{fileName}");
        }

        public async Task<CloudBlobContainer> GetOrCreateContainer(string containerName, BlobContainerPublicAccessType accessType = BlobContainerPublicAccessType.Blob)
        {
            if (containerName.Contains("[A-Z]"))
            {
                throw new FormatException("No uppercase characters");
            }
            if (containerName.Length < 3 || containerName.Length >= 63)
            {
                throw new FormatException("Less than 3 or greater than 63 characters");
            }
            if (containerName.Contains(@"\b\-{2,}\b"))
            {
                throw new FormatException("More than one consecutive '-'");
            }

            var container = BlobClient.GetContainerReference(containerName);

            if (await container.CreateIfNotExistsAsync())
            {
                await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = accessType });
            }

            return container;
        }

        public string GenerateHashId(string name, string salt)
        {
            var ids = name.Select(cha => Convert.ToInt32(cha)).ToArray();
            var hashIdGenerator = new Hashids(salt);
            return hashIdGenerator.Encode(ids);
        }
    }
}