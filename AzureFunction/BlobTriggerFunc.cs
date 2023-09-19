using System;
using System.Collections.Generic;
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
        public void Run([BlobTrigger("blobcontainer/{name}", Connection = "AzureWebJobsStorage")]
            byte[] myBlob, string name, ILogger log, IDictionary<string,string> metadata)
        {
            if (metadata.TryGetValue("Email", out var blob))
            {
                SendEmailToUser(name, blob, log);
                var sasToken = GenerateSasToken(name);
                log.LogInformation($"SAS Token: {sasToken}");
            }
            else
            {
                log.LogError("Metadata is empty");
            }
            
        }
        private static void SendEmailToUser(string blobName, string recipientEmail, ILogger log)
        {
            string apiKey = "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";
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
                    log.LogError($"Failed to send email: {response.StatusCode} Response: {response.Body.ReadAsStringAsync()}");
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
