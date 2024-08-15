using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EDIConverterWeb.Data
{
    public class PurchaseOrderAcknowledgementRepo
    {
        private readonly string _connectionString;
        public PurchaseOrderAcknowledgementRepo(string connectionString)
        {
            _connectionString = connectionString;
        }
        public bool IsValid855Data(string purchaseOrderNum, string purchaseOrderDate, char testIndicator, string PO1)
        {
            bool isValidTestIndicator = testIndicator == 'T' || testIndicator == 'P';
            DateTime dateValue;
            bool isValidDate = DateTime.TryParseExact(purchaseOrderDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
            return purchaseOrderNum.Length <= 22 && isValidTestIndicator && isValidDate && IsValidPO1Lines(PO1);
        }
        public int AddDoc(string purchaseOrderNum, string purchaseOrderDate, char testIndicator, string itemsList)
        {
            using var ctx = new EDIDbContext(_connectionString);
            var doc = new PurchaseOrderAcknowledgement
            {
                PurchaseOrderNumber = purchaseOrderNum,
                PurchaseOrderDate = DateTime.ParseExact(purchaseOrderDate, "yyyyMMdd", CultureInfo.CurrentCulture),
                AcknowledgementDate = DateTime.Now,
                ItemsOrdered = ConvertPO1ListToItemList(itemsList),
                ScheduledShipDate = DateTime.Now.AddDays(7),
                TestIndicator = testIndicator
            };
            ctx.PurchaseOrderAcknowledgements.Add(doc);
            ctx.SaveChanges();


            doc.TransactionNumber = GenerateTransactionNumber(doc.ReferenceNumber);
            doc.GroupNumber = GenerateGroupNumber(doc.ReferenceNumber);
            doc.InterchangeNumber = GenerateInterchangeNumber(doc.ReferenceNumber);
            ctx.PurchaseOrderAcknowledgements.Update(doc);
            ctx.SaveChanges();
            return doc.ReferenceNumber;
        }
        private string GenerateTransactionNumber(int num)
        {
            string numToString = num.ToString();
            if (string.IsNullOrWhiteSpace(numToString) || numToString.Length <= 5)
            {
                return numToString;
            }
            else
            {
                var newNum = numToString.Substring(0, 5);
                return newNum;
            }
        }
        private string GenerateGroupNumber(int num)
        {
            string numToString = num.ToString();
            if (string.IsNullOrWhiteSpace(numToString))
            {
                return numToString;
            }
            else
            {
                var newNum = numToString.Substring(5);
                return newNum;
            }
        }
        private string GenerateInterchangeNumber(int num)
        {
            string numToString = num.ToString();
            if (string.IsNullOrWhiteSpace(numToString))
            {
                return numToString;
            }
            else
            {
                var newNum = numToString.Substring(numToString.Length - 9, 9);
                if(newNum.Length < 9)
                {   
                    //make it fill with zeros if it's not 9 digits... or maybe it should invalidate...
                }
                return newNum;
            }
        }
        private bool IsValidPO1Lines(string PO1)
        {
            var newList = ReplaceSpacesAndNewLines(PO1);
            if (newList[newList.Length - 1] != '~')
            {
                return false;
            }
            //working on the specific segments in separate project
            return true;
        }
        private string ReplaceSpacesAndNewLines(string text)
        {
            return text.Replace(" ", "").ReplaceLineEndings("");
        }
        private List<Item> ConvertPO1ListToItemList(string PO1)
        {
            var newList = ReplaceSpacesAndNewLines(PO1);
            var items = new List<Item>();
            var itemString = "";
            for (int i = 0; i < newList.Length; i++)
            {
                itemString += newList[i];
                if (newList[i] == '~')
                {
                    var item = ConvertPO1ToItem(itemString);
                    items.Add(item);
                    itemString = "";
                }
            }
            return items;
        }
        private Item ConvertPO1ToItem(string PO1)
        {
            Console.WriteLine(PO1);
            var item = new Item();
            var segment = "";
            var segmentCounter = 0;
            for (int i = 3; i < PO1.Length; i++)
            {
                //Console.WriteLine(PO1[i]);
                if (PO1[i] == '*')
                {
                    segmentCounter++;
                    //Console.WriteLine($"counter:{segmentCounter}");
                    if (segmentCounter == 2)
                    {
                        item.LineNumber = int.Parse(segment);
                    }
                    if (segmentCounter == 3)
                    {
                        item.QuantityOrdered = int.Parse(segment);
                    }
                    else if (segmentCounter == 4)
                    {
                        item.UnitOfMeasure = segment;
                    }
                    else if (segmentCounter == 5)
                    {
                        item.UnitPrice = decimal.Parse(segment);
                    }
                    segment = "";
                }
                else if (PO1[i] == '~')
                {
                    item.ItemNumber = segment;
                    break;
                }
                else
                {
                    segment += PO1[i];
                }
            }
            return item;
        }
    }
}
