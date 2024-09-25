using EDIConverterWeb.Data;

namespace EDIConverterWeb.Web.ViewModels
{
    public class IdsViewModel
    {
        public int? PurchaseOrderId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public int? POAcknowledgementReferenceNumber { get; set; }
    }
}
