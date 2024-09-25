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
        public string WritePOAcknowledgementText(int referenceNum)
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
        public string WritePurchaseOrderText(int purchaseOrderId)
        {
            using var ctx = new EDIDbContext(_connectionString);
            var po = ctx.PurchaseOrders.Include(po => po.LineItems).FirstOrDefault(po => po.Id == purchaseOrderId);
            var totalPrice = GetTotalCost(po.LineItems);
            var purchaseOrderText = $"Purchase Order #: {po.PurchaseOrderNumber}\n" +
                $"Purchase Order Date: {po.PurchaseOrderDate.ToString("MM/dd/yyyy")}\n" +
                $"Customer Details:\n" +
                $"\t\t\t{po.FacilityName}\n" +
                $"\t\t\t{po.StreetAddress}\n" +
                $"\t\t\t{po.City}, {po.State} {po.PostalCode}\n\n" +
                $"\t\t\tFacility Code: {po.FacilityCode}\n\n" +
                $"Contact Name: {po.ContactName}\n" +
                $"Contact Number: {po.ContactNumber}\n\n" +
                $"Line Items:\n";
            foreach(Item i in po.LineItems.OrderBy(i => i.LineNumber))
            {
                purchaseOrderText += $"\tItem #{i.LineNumber}:\n" +
                    $"\tQuantity: {i.QuantityOrdered}\n" +
                    $"\tUnit of Measure: {i.UnitOfMeasure}\n" +
                    $"\tUnit Price: {i.UnitPrice}\n" +
                    $"\tItem Number: {i.ItemNumber}\n\n";
            }
            purchaseOrderText += $"Total Price: {totalPrice}";
            return purchaseOrderText;
        }
    }
}
