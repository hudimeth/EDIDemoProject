using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EDIConverterWeb.Data
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public string FacilityName { get; set; }
        public string FacilityCode { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public List<Item> LineItems { get; set; }
        public POAcknowledgement? POAcknowledgement { get; set; }

    }
}
