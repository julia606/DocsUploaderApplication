using Azure.Storage.Blobs;
using AzureFunction;
using Microsoft.Extensions.Logging;
using DocsUploaderApplication.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace DocsUploaderAppTest
{
    [TestClass]
    public class BlobTriggerTest
    {
        private static readonly Mock<ILogger> log = new Mock<ILogger>();

        [TestMethod]
        public async Task BlobTrigger_EmailSend()
        {
            var blobTriggerData = new BlobTriggerDataModel
            {
                BlobStream = new byte[] { 1, 2, 3 },
                BlobName = "example.docx",
                Metadata = new Dictionary<string, string>
                {
                    { "Email", "example@example.com" },
                },
            };

            BlobTriggerFunc.Run(blobTriggerData.BlobStream, blobTriggerData.BlobName, log.Object, blobTriggerData.Metadata);

            log.Verify(x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.IsAny<It.IsAnyType>(),
                        null,
                        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                    ), Times.AtLeastOnce);
        }

        [TestMethod]
        public async Task BlobTrigger_EmailNotSend()
        {
            var blobTriggerData = new BlobTriggerDataModel
            {
                BlobStream = new byte[] { 1, 2, 3 },
                BlobName = "example.docx",
                Metadata = new Dictionary<string, string>
                {
                    { "Test", "test" },
                },
            };

            BlobTriggerFunc.Run(blobTriggerData.BlobStream, blobTriggerData.BlobName, log.Object, blobTriggerData.Metadata);

            log.Verify(x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.IsAny<It.IsAnyType>(),
                        null,
                        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                    ), Times.AtLeastOnce);
        }

    }
}
