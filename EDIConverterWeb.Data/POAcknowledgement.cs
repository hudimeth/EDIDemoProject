using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EDIConverterWeb.Data
{
    public class POAcknowledgement
    { 
        [Key]
        public int ReferenceNumber { get; set; }
        //1000000001
        public string InterchangeNumber { get; set; }
        //000000001
        public string GroupNumber { get; set; }
        //00001
        public string TransactionNumber { get; set; }
        //10000
        public DateTime AcknowledgementDate { get; set; }
        public DateTime ScheduledShipDate { get; set; }
        public int PurchaseOrderId { get; set; }

        [JsonIgnore]
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
        
    }
}
