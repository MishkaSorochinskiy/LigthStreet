using Domain.AzureConnections.Interfaces;
using Infrastructure.Helpers;
using Infrastructure.Services.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly IAzureStorageConnection _azureStorageConnection;
        private CloudBlobContainer cloudBlobContainer;

        public ImageService(IAzureStorageConnection azureStorageConnection)
        {
            _azureStorageConnection = azureStorageConnection;
            string accessKey = azureStorageConnection.GetConfiguration();
            SetStorageSettings(accessKey);
        }

        private async Task SetStorageSettings(string accessKey)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            string containerName = SignatureConstants.CONTAINER_NAME;

            cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
        }

        public async Task DeleteImageFromStorageAsync(string pointId)
        {
            try
            {
                StringBuilder fileName = new StringBuilder(pointId);
                fileName.Append(SignatureConstants.JPG);
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName.ToString());

                await cloudBlockBlob.DeleteAsync();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<string> DownloadIMageFromStorageAsync(string pointId)
        {
            try
            {
                StringBuilder fileName = new StringBuilder(pointId);
                fileName.Append(SignatureConstants.JPG);
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName.ToString());

                byte[] array = new byte[SignatureConstants.IMAGE_BYTE_LENGTH];
                await cloudBlockBlob.DownloadToByteArrayAsync(array, 0);
                string image = Convert.ToBase64String(array);
                return image;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task UploadImageToStorageAsync(string pointId, string image)
        {
            try
            {
                StringBuilder fileName = new StringBuilder(pointId);
                fileName.Append(SignatureConstants.JPG);

                byte[] fileData = Convert.FromBase64String(image);
                if (fileName != null && fileData != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName.ToString());
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
