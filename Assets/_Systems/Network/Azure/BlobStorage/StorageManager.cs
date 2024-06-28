using System;
using Azure.Storage.Blobs;
using Cysharp.Threading.Tasks;

namespace Network.Azure.BlobStorage
{
    public class StorageManager
    {
        private string connectionString = "TuCadenaDeConexion";
        private string containerName = "nombre-del-contenedor";

        public async UniTask UploadFileAsync(string filePath, string blobName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(filePath, true);
            Console.WriteLine($"Archivo {blobName} subido a Azure Blob Storage.");
        }

        public async UniTask DownloadFileAsync(string blobName, string downloadFilePath)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DownloadToAsync(downloadFilePath);
            Console.WriteLine($"Archivo {blobName} descargado desde Azure Blob Storage a {downloadFilePath}.");
        }
    }
}