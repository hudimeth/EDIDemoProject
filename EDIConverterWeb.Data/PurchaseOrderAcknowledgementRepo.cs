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
        private string _connectionString;
        public PurchaseOrderAcknowledgementRepo(string connectionString)
        {
            _connectionString = connectionString;
        }
        public bool IsValid855Data(string purchaseOrderNum, string purchaseOrderDate, char testIndicator)
        {
            bool isValidTestIndicator = testIndicator == 't' || testIndicator == 'p';
            DateTime dateValue;
            bool isValidDate = DateTime.TryParseExact(purchaseOrderDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
            return purchaseOrderNum.Length <= 22 && isValidTestIndicator && isValidDate;
        }
        public int AddDoc(string purchaseOrderNum, string purchaseOrderDate, char testIndicator, string itemsList)
        {
            var ctx = new EDIDbContext(_connectionString);
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


            //these aren't getting set- figure it out, i think it's cuz i erased some code
            doc.TransactionNumber = GenerateTransactionNumber(doc.InterchangeId);
            doc.GroupNumber = GenerateGroupNumber(doc.InterchangeId);
            doc.ReferenceNumber = GenerateReferenceNumber(doc.InterchangeId);
            ctx.PurchaseOrderAcknowledgements.Update(doc);
            ctx.SaveChanges();
            return doc.InterchangeId;
        }
        private string GenerateTransactionNumber(int num)
        {
            string numToString = num.ToString();
            if (string.IsNullOrWhiteSpace(numToString) || numToString.Length <= 5)
            {
                return num.ToString();
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
                return num.ToString();
            }
            else
            {
                var newNum = numToString.Substring(4);
                return newNum;
            }
        }
        private string GenerateReferenceNumber(int num)
        {
            string numToString = num.ToString();
            if (string.IsNullOrWhiteSpace(numToString))
            {
                return num.ToString();
            }
            else
            {
                return "123" + numToString;
            }
        }
        private List<Item> ConvertPO1ListToItemList(string list)
        {
            var newList = list.Replace(" ", "");
            var items = new List<Item>();
            var itemString = "";
            for (int i = 0; i < newList.Length; i++)
            {
                itemString += newList[i];
                if (list[i] == '~')
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
                        item.Index = int.Parse(segment);
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
