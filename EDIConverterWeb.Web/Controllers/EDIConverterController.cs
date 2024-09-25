using EDIConverterWeb.Data;
using EDIConverterWeb.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace EDIConverterWeb.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EDIConverterController : ControllerBase
    {
        private readonly string _connectionString;
        public EDIConverterController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpPost]
        [Route("addpurchaseorder")]
        public IdsViewModel AddPurchaseOrder(Add850ViewModel vm)
        {
            var repo850 = new PurchaseOrderRepo(_connectionString);
            var purchaseOrderId = repo850.AddDoc(vm.PurchaseOrder);
            int? referenceNumber = null;
            string purchaseOrderNumber = "";
            if (purchaseOrderId.HasValue)
            {
                var repo855 = new POAcknowledgementRepo(_connectionString);
                referenceNumber = repo855.GeneratePOAcknowledgementControlNumbers(purchaseOrderId.Value);
                purchaseOrderNumber = repo850.GetPurchaseOrderNumber(purchaseOrderId.Value);
            }
            return new IdsViewModel
            {
                PurchaseOrderId = purchaseOrderId,
                PurchaseOrderNumber = purchaseOrderNumber,
                POAcknowledgementReferenceNumber = referenceNumber
            };
        }

        [HttpGet]
        [Route("get855file")]
        public IActionResult Get855File(int referenceNumber)
        {
            var repo = new EDIFileWriterRepo(_connectionString);
            var poAcknowledgementText = repo.WritePOAcknowledgementText(referenceNumber);
            return File(Encoding.UTF8.GetBytes(poAcknowledgementText), "application/octet-stream");
        }

        [HttpGet]
        [Route("getpurchaseorderfile")]
        public IActionResult GetPurchaseOrderFile(int purchaseOrderId)
        {
            var repo = new EDIFileWriterRepo(_connectionString);
            var poText = repo.WritePurchaseOrderText(purchaseOrderId);
            return File(Encoding.UTF8.GetBytes(poText), "application/octet-stream");
        }
    }
}
