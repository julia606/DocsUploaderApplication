using Azure.Storage.Blobs;
using DocsUploaderApplication.Controllers;
using DocsUploaderApplication.Models;
using DocsUploaderApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using static System.Net.WebRequestMethods;

namespace DocsUploaderAppTest
{

    [TestClass]
    public class FileControllerTests
    {
        private static readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=docxuploaderstorage;AccountKey=msYl+hbqOZVH4JNHidWFcfGA8v3JtiW3aenMOEIVufcP2BM2y8as8QwUhfuu9asXgK3ErFnkv3Ky+AStPE5OSg==;EndpointSuffix=core.windows.net";
        private static readonly BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

        [TestMethod]
        public async Task Upload_ValidFileAndEmail()
        {
            var controller = GetController();
            var model = GetModel("test.docx", "test@test.com");

            var result = await controller.Upload(model) as OkObjectResult;

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal("File uploaded successfully.", result.Value);
        }

        [TestMethod]
        public async Task Upload_InvalidFileExtention()
        {
            var controller = GetController();
            var model = GetModel("test.txt", "test@test.com");

            var result = await controller.Upload(model) as BadRequestObjectResult;

            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal("Invalid data", result.Value);
        }

        private static FileController GetController()
        {
            return new FileController(blobServiceClient);
        }

        private static FileUploadModel GetModel(string fileName, string email)
        {
            return new FileUploadModel
            {
                File = new FormFile(new MemoryStream(new byte[] { 1, 2, 3 }), 0, 3, "testFile", fileName),
                Email = email
            };
        }
        
    }
}
