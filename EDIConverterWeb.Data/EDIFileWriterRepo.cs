using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDIConverterWeb.Data
{
    public class EDIFileWriterRepo
    {
        private readonly string _connectionString;
        public EDIFileWriterRepo(string connectionString)
        {
            _connectionString = connectionString;
        }
        public string WritePurchaseOrderAcknowledgement(int referenceNum)
        {
            using var ctx = new EDIDbContext(_connectionString);
            var po = ctx.PurchaseOrders.Include(po => po.POAcknowledgement).Include(po => po.LineItems).FirstOrDefault(po => po.POAcknowledgement.ReferenceNumber == referenceNum);

            var ediText = $"ISA*00*          *00*          *ZZ*TRISTATE       *ZZ*80040537744CRT *{po.POAcknowledgement.AcknowledgementDate.ToString("yyMMdd")}*{po.POAcknowledgement.AcknowledgementDate.ToString("HHmm")}*U*00401*{po.POAcknowledgement.InterchangeNumber}*0*P*>~\n" +
                $"GS*PR*TRISTATE*80040537744CRT*{po.POAcknowledgement.AcknowledgementDate.ToString("yyyyMMdd")}*{po.POAcknowledgement.AcknowledgementDate.ToString("HHmm")}*{po.POAcknowledgement.GroupNumber}*X*004010~\n" +
                $"ST*855*{po.POAcknowledgement.TransactionNumber}~\n" +
                $"BAK*00*AC*{po.PurchaseOrderNumber}*{po.PurchaseOrderDate.ToString("yyyyMMdd")}****{po.POAcknowledgement.ReferenceNumber}*" +
                $"{po.POAcknowledgement.AcknowledgementDate.ToString("yyyyMMdd")}~\n";
            foreach (Item item in po.LineItems.OrderBy(i => i.LineNumber))
            {
                ediText += $"PO1*{item.LineNumber}*{item.QuantityOrdered}*{item.UnitOfMeasure}*{item.UnitPrice}**VN*{item.ItemNumber}~\n" +
                    $"ACK*IA***068*{po.POAcknowledgement.ScheduledShipDate.ToString("yyyyMMdd")}~\n";
            }
            ediText += $"CTT*{po.LineItems.Count}~\n" +
                //make sure to check that this total matches the total in the PO
                $"AMT*TT*{GetTotalCost(po.LineItems)}~\n" +
                $"SE*{(po.LineItems.Count * 2) + 5}*{po.POAcknowledgement.TransactionNumber}~\n" +
                $"GE*1*{po.POAcknowledgement.GroupNumber}~\n" +
                $"IEA*1*{po.POAcknowledgement.InterchangeNumber}~\n";
            return ediText;
        }
        private decimal GetTotalCost(List<Item> itemsList)
        {
            return itemsList.Select(i => i.UnitPrice * i.QuantityOrdered).Sum();
        }
    }
}
