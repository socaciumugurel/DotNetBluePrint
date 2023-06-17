using BluePrint.core.Infrastructure;
using BluePrint.services.Module1.Services.Interfaces;
using BluePrint.shared.services;
using BluePrint.shared.services.AzureBlob;
using BluePrint.shared.services.Responses;

namespace BluePrint.services.Module1.Services
{
    [Service(typeof(IProductService))]
    public class ProductService : IProductService
    {
        private readonly IAzureBlobService _azure;

        public ProductService(IAzureBlobService azure)
        {
            _azure = azure;
        }

        public async Task<GenericResult<Stream>> GetProductFileAsync(string fileName)
        {
            var file = await _azure.GetFileAsync(Constants.AzureBlobContainers.PRODUCT, "", fileName);

            if (!file.Succeeded)
            {
                return GenericResult<Stream>.Error(ErrorCodes.NotFound);
            }

            return GenericResult<Stream>.Success(file.Result);

        }
    }
}
