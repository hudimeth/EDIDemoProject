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
        [Route("add850")]
        public ReferenceNumber855ViewModel Add850(Add850ViewModel vm)
        {
            var repo850 = new PurchaseOrderRepo(_connectionString);
            var referenceNum855 = repo850.AddDoc(vm.PurchaseOrder);
            if (referenceNum855.HasValue)
            {
                var repo855 = new POAcknowledgementRepo(_connectionString);
                repo855.Generate855ControlNumbers(referenceNum855.Value);
            }
            return new ReferenceNumber855ViewModel
            {
                Id = referenceNum855
            };
        }

        [HttpGet]
        [Route("view855")]
        public View855ViewModel View855(int referenceNumber)
        {
            var repo = new EDIFileWriterRepo(_connectionString);
            return new()
            {
                EdiText = repo.WritePurchaseOrderAcknowledgement(referenceNumber)
            };
        }

    }
}
