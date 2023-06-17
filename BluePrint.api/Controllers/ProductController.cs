using BluePrint.services.Module1.Services.Interfaces;
using BluePrint.shared.services.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BluePrint.api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetProductFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> GetProductFileAsync(string fileName)
        {
            var productFile = await _productService.GetProductFileAsync(fileName);

            if (!productFile.Succeeded)
            {
                return NotFound(productFile.Errors);
            }

            var fileContentType = MimeHelper.GetMimeTypeFromFileName(fileName);

            return File(productFile.Result, fileContentType.Result);
        }
    }
}