using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDIConverterWeb.Data
{
    public class PurchaseOrderAcknowledgement
    { 
        [Key]
        public int InterchangeId { get; set; }
        //internally set, unique per 855 doc. starts at 100010001
        public string PurchaseOrderNumber { get; set; }
        //up to 22 characters
        public DateTime PurchaseOrderDate { get; set; }
        public string ReferenceNumber { get; set; }
        //autogenerate 12 digits
        public DateTime AcknowledgementDate { get; set; }
        public List<Item> ItemsOrdered { get; set; }
        public DateTime ScheduledShipDate { get; set; }
        //autogenerate one week later than today's date
        public string TransactionNumber { get; set; }
        //autogenerate 5 digits
        public char TestIndicator { get; set; }
        public string GroupNumber { get; set; }
        //autogenerate 5 digits
    }
}
