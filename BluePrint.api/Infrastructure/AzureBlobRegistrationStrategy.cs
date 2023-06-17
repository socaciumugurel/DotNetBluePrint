using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BluePrint.core.Infrastructure;
using BluePrint.shared.services;
using Microsoft.Extensions.Azure;

namespace BluePrint.api.Infrastructure
{
    /// <summary>
    /// Strategy how to register AzureBlob in ServiceCollection
    /// </summary>
    public class AzureBlobRegistrationStrategy : IStrategy<IServiceCollection>
    {
        private readonly string _connectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public AzureBlobRegistrationStrategy(string? connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Here initialize connection for blob client and generates container if not exist
        /// </summary>
        /// <param name="input"></param>
        public void Execute(IServiceCollection input)
        {
            input.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(_connectionString)
                    .ConfigureOptions(options =>
                    {
                        options.Retry.Mode = Azure.Core.RetryMode.Exponential;
                        options.Retry.MaxRetries = 3;
                        options.Retry.MaxDelay = TimeSpan.FromSeconds(120);
                    });
            });

            var blobClient = new BlobServiceClient(_connectionString);
            var container = blobClient.GetBlobContainerClient(Constants.AzureBlobContainers.PRODUCT);
            container.CreateIfNotExists(PublicAccessType.Blob);
        }

        /// <summary>
        /// Static Factory
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static AzureBlobRegistrationStrategy Create(string? connectionString)
        {
            return new AzureBlobRegistrationStrategy(connectionString);
        }
    }
}
