using BluePrint.core.Infrastructure;
using BluePrint.services.Module1.Services.Interfaces;

namespace BluePrint.services.Module1.Services
{
    [Service(typeof(IBluePrintService))]
    public class BluPrintService : IBluePrintService
    {
        public string GetBluePrintData()
        {
            return "BluePrintService";
        }
    }
}
