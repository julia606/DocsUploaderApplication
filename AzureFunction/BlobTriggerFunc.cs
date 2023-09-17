using System;
using System.IO;
using System.Net.Mail;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AzureFunction
{
    public class BlobTriggerFunc
    {
        [FunctionName("BlobTriggerFunc")]
        public void Run([BlobTrigger("blobcontainer/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
        private static void SendEmailToUser(string blobName, string recipientEmail, ILogger log)
        {
            string apiKey = Environment.GetEnvironmentVariable("SendGridApiKey");
            var client = new SendGridClient(apiKey);

            var senderEmail = "julietmntg@gmail.com";
            var subject = "File Uploaded Notification";

            var plainTextContent = $"The file {blobName} has been uploaded to the Azure Blob Storage.";
            var htmlContent = $"<p>The file <strong>{blobName}</strong> has been uploaded to the Azure Blob Storage.</p>";
            var msg = MailHelper.CreateSingleEmail(new EmailAddress(senderEmail), new EmailAddress(recipientEmail), subject, plainTextContent, htmlContent);

            try
            {
                var response = client.SendEmailAsync(msg).Result;
                if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
                {
                    log.LogError($"Failed to send email: {response.StatusCode}");
                }
                else
                {
                    log.LogInformation($"Email sent successfully: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                log.LogError($"Error sending email: {ex.Message}");
            }
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
