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
        public string WritePurchaseOrderAcknowledgement(int poId)
        {
            using var ctx = new EDIDbContext(_connectionString);
            var doc = ctx.PurchaseOrderAcknowledgements.Include(poa => poa.ItemsOrdered).FirstOrDefault(poa => poa.ReferenceNumber == poId);
            var ediText = $"ISA*00*          *00*          *ZZ*TRISTATE       *ZZ*80040537744CRT *{doc.AcknowledgementDate.ToString("yyMMdd")}*{doc.AcknowledgementDate.ToString("HHmm")}*U*00401*{doc.InterchangeNumber}*0*{doc.TestIndicator}*>~\n" +
                $"GS*PR*TRISTATE*80040537744CRT*{doc.AcknowledgementDate.ToString("yyyyMMdd")}*{doc.AcknowledgementDate.ToString("HHmm")}*{doc.GroupNumber}*X*004010~\n" +
                $"ST*855*{doc.TransactionNumber}~\n" +
                $"BAK*00*AC*{doc.PurchaseOrderNumber}*{doc.PurchaseOrderDate.ToString("yyyyMMdd")}****{doc.ReferenceNumber}*" +
                $"{doc.AcknowledgementDate.ToString("yyyyMMdd")}~\n";
            foreach (Item item in doc.ItemsOrdered.OrderBy(i => i.LineNumber))
            {
                ediText += $"PO1*{item.LineNumber}*{item.QuantityOrdered}*{item.UnitOfMeasure}*{item.UnitPrice}**VN*{item.ItemNumber}~\n" +
                    $"ACK*IA***068*{doc.ScheduledShipDate.ToString("yyyyMMdd")}~\n";
            }
            ediText += $"CTT*{doc.ItemsOrdered.Count}~\n" +
                $"AMT*TT*{GetTotalCost(doc.ItemsOrdered)}~\n" +
                $"SE*{(doc.ItemsOrdered.Count * 2) + 5}*{doc.TransactionNumber}~\n" +
                $"GE*1*{doc.GroupNumber}~\n" +
                $"IEA*1*{doc.InterchangeNumber}~\n";
            return ediText;
        }
        private decimal GetTotalCost(List<Item> itemsList)
        {
            return itemsList.Select(i => i.UnitPrice * i.QuantityOrdered).Sum();
        }
    }
}
