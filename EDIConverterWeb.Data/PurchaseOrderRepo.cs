using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EDIConverterWeb.Data
{
    public class PurchaseOrderRepo
    {
        private readonly string _connectionString;
        public PurchaseOrderRepo(string connectionString)
        {
            _connectionString = connectionString;
        }
        public int? AddDoc(string text)
        {
            var poData = Parse850Text(text);
            if (poData == null)
            {
                return null;
            }
            using var ctx = new EDIDbContext(_connectionString);
            var po = new PurchaseOrder
            {
                PurchaseOrderNumber = poData.PurchaseOrderNumber,
                PurchaseOrderDate = poData.PurchaseOrderDate,
                FacilityName = poData.FacilityName,
                FacilityCode = poData.FacilityCode,
                StreetAddress = poData.StreetAddress,
                City = poData.City,
                State = poData.State,
                PostalCode = poData.PostalCode,
                ContactName = poData.ContactName,
                ContactNumber = poData.ContactNumber,
                LineItems = poData.LineItems,
                POAcknowledgement = new()
                {
                    AcknowledgementDate = DateTime.Now,
                    ScheduledShipDate = poData.PurchaseOrderDate.AddDays(7)
                }
            };
            ctx.PurchaseOrders.Add(po);
            ctx.SaveChanges();
            return po.POAcknowledgement.ReferenceNumber;
        }
        public PurchaseOrder Parse850Text(string text)
        {
            var oneLineText = text.ReplaceLineEndings("").Trim();
            var segment = "";
            var purchaseOrder = new PurchaseOrder();
            string lineItemsText = "";

            //now it only validates the info based on the lines and specific segments that we take info from
            bool validBEGSegment = false;
            bool validN1Segment = false;
            bool validN3Segment = false;
            bool validN4Segment = false;
            bool validPERSegment = false;
            bool validPO1Segment = false;

            for (int i = 0; i < oneLineText.Length; i++)
            {
                segment += oneLineText[i];
                if (oneLineText[i] == '~')
                {
                    if (segment.StartsWith("BEG"))
                    {
                        var newSegment = segment.Remove(segment.Length - 1);
                        var eachBEGSegmentSeparated = newSegment.Split('*');
                        if (eachBEGSegmentSeparated.Count() == 6)
                        {
                            if (eachBEGSegmentSeparated[0] != "" && eachBEGSegmentSeparated[3] != "" && eachBEGSegmentSeparated[5] != "")
                            {
                                validBEGSegment = true;
                                purchaseOrder.PurchaseOrderNumber = eachBEGSegmentSeparated[3];
                                purchaseOrder.PurchaseOrderDate = DateTime.ParseExact(eachBEGSegmentSeparated[5], "yyyyMMdd", CultureInfo.CurrentCulture);
                            }
                        }
                    }
                    else if (segment.StartsWith("N1"))
                    {
                        var newSegment = segment.Remove(segment.Length - 1);
                        var eachN1SegmentSeparated = newSegment.Split('*');
                        var noEmptySegments = eachN1SegmentSeparated[0] != "" && eachN1SegmentSeparated[2] != "" && eachN1SegmentSeparated[4] != "";
                        if (eachN1SegmentSeparated.Count() == 5 && noEmptySegments)
                        {
                            validN1Segment = true;
                            purchaseOrder.FacilityName = eachN1SegmentSeparated[2];
                            purchaseOrder.FacilityCode = eachN1SegmentSeparated[4];
                        }
                    }
                    else if (segment.StartsWith("N3"))
                    {
                        var newSegment = segment.Remove(segment.Length - 1);
                        var eachN3SegmentSeparated = newSegment.Split('*');
                        if (eachN3SegmentSeparated.Count() == 2)
                        {
                            if (eachN3SegmentSeparated[0] != "" && eachN3SegmentSeparated[1] != "")
                            {
                                validN3Segment = true;
                                purchaseOrder.StreetAddress = eachN3SegmentSeparated[1];
                            }
                        }
                    }
                    else if (segment.StartsWith("N4"))
                    {
                        var newSegment = segment.Remove(segment.Length - 1);
                        var eachN4SegmentSeparated = newSegment.Split('*');
                        if (eachN4SegmentSeparated.Count() == 4)
                        {
                            if (eachN4SegmentSeparated[0] != "" && eachN4SegmentSeparated[1] != "" && eachN4SegmentSeparated[2] != "" && eachN4SegmentSeparated[3] != "")
                            {
                                validN4Segment = true;
                                purchaseOrder.City = eachN4SegmentSeparated[1];
                                purchaseOrder.State = eachN4SegmentSeparated[2];
                                purchaseOrder.PostalCode = eachN4SegmentSeparated[3];
                            }
                        }
                    }
                    else if (segment.StartsWith("PER"))
                    {
                        var newSegment = segment.Remove(segment.Length - 1);
                        var eachPERSegmentSeparated = newSegment.Split('*');
                        if (eachPERSegmentSeparated.Count() == 5)
                        {
                            if (eachPERSegmentSeparated[0] != "" && eachPERSegmentSeparated[2] != "" && eachPERSegmentSeparated[4] != "")
                            {
                                validPERSegment = true;
                                purchaseOrder.ContactName = eachPERSegmentSeparated[2];
                                purchaseOrder.ContactNumber = eachPERSegmentSeparated[4];
                            }
                        }
                    }
                    if (segment.StartsWith("PO1"))
                    {
                        lineItemsText += segment;
                    }
                    segment = "";
                }
            }
            if (IsValidPO1Lines(lineItemsText))
            {
                validPO1Segment = true;
                var lineItems = ConvertPO1TextToItemList(lineItemsText);
                purchaseOrder.LineItems = lineItems;
            }

            var validData = validBEGSegment && validN1Segment && validN3Segment && validN4Segment && validPERSegment && validPO1Segment;
            if (!validData)
            {
                return null;
            }
            return purchaseOrder;
        }
        public bool IsValidPO1Lines(string PO1)
        {
            var withoutSpaces = ReplaceSpacesAndNewLines(PO1);
            if (withoutSpaces[withoutSpaces.Length - 1] != '~')
            {
                return false;
            }
            List<string[]> splitItemsList = withoutSpaces
                .Split('~')
                .Select(s => s.Split('*'))
                .ToList();
            if (splitItemsList[splitItemsList.Count() - 1][0] == "")
            {
                splitItemsList.RemoveAt(splitItemsList.Count() - 1);
            };
            foreach (string[] itemStringArr in splitItemsList)
            {
                if (itemStringArr.Length != 8)
                {
                    return false;
                }
                if (itemStringArr[0] != "PO1")
                {
                    return false;
                }
                int lineNumber;
                if (!int.TryParse(itemStringArr[1], out lineNumber) || int.Parse(itemStringArr[1]) > splitItemsList.Count())
                {
                    return false;
                }
                int quantity;
                if (!int.TryParse(itemStringArr[2], out quantity))
                {
                    return false;
                }
                if (!Enum.IsDefined(typeof(UnitOfMeasure), itemStringArr[3]))
                {
                    return false;
                }
                decimal unitPrice;
                if (!decimal.TryParse(itemStringArr[4], out unitPrice))
                {
                    return false;
                }
                if (itemStringArr[5] != "")
                {
                    return false;
                }
                if (itemStringArr[6] != "VN")
                {
                    return false;
                }
                if (itemStringArr[7] == "")
                {
                    return false;
                }
            }
            return true;
        }
        private string ReplaceSpacesAndNewLines(string text)
        {
            return text.Replace(" ", "").ReplaceLineEndings("");
        }
        private List<Item> ConvertPO1TextToItemList(string PO1)
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
