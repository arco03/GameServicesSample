using Azure.Storage.Blobs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Network.Azure.BlobStorage
{
    public class StorageManager
    {
        private readonly AzureConfigurations _configurations;
        
        public StorageManager(AzureConfigurations configurations)
        {
            _configurations = configurations;
        }
        
        public async UniTask UploadFileAsync(string filePath, string blobName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_configurations.connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_configurations.containerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(filePath, true);
            Debug.Log($"File {blobName} uploaded to Azure Blob Storage.");
        }

        public async UniTask DownloadFileAsync(string blobName, string downloadFilePath)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_configurations.connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_configurations.containerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DownloadToAsync(downloadFilePath);
            Debug.Log($"File {blobName} downloaded from Azure Blob Storage to {downloadFilePath}.");
        }
    }
}