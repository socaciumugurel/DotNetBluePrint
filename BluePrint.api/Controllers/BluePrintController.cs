using BluePrint.services.Module1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BluePrint.api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BluePrintController : ControllerBase
    {

        private readonly IBluePrintService _bluePrintService;

        public BluePrintController(IBluePrintService bluePrintService)
        {
            _bluePrintService = bluePrintService;
        }

        [HttpGet("GetBluePrintData")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ObjectResult GetData()
        {
            return Ok(_bluePrintService.GetBluePrintData());
        }
    }
}