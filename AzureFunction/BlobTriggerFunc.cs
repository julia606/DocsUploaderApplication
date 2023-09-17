using System;
using System.IO;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunction
{
    public class BlobTriggerFunc
    {
        [FunctionName("BlobTriggerFunc")]
        public void Run([BlobTrigger("blobcontainer/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
        private static string GenerateSasToken(string blobName)
        {
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = "blobcontainer",
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            var blobContainerClient = blobServiceClient.GetBlobContainerClient("blobcontainer");
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            return blobClient.GenerateSasUri(sasBuilder).ToString();
        }
    }
}
