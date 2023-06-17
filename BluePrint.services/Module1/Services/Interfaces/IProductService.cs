using BluePrint.shared.services.Responses;

namespace BluePrint.services.Module1.Services.Interfaces
{
    public interface IProductService
    {
        Task<GenericResult<Stream>> GetProductFileAsync(string fileName);
    }
}
