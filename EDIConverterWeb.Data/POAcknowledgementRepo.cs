using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDIConverterWeb.Data
{
    public class POAcknowledgementRepo
    {
        private readonly string _connectionString;
        public POAcknowledgementRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int GeneratePOAcknowledgementControlNumbers(int purchaseOrderId)
        {
            using var ctx = new EDIDbContext(_connectionString);
            var poAcknowledgement = ctx.PurchaseOrderAcknowledgements.FirstOrDefault(poa => poa.PurchaseOrder.Id == purchaseOrderId);
            poAcknowledgement.InterchangeNumber = GenerateInterchangeNumber(poAcknowledgement.ReferenceNumber);
            poAcknowledgement.GroupNumber = GenerateGroupNumber(poAcknowledgement.ReferenceNumber);
            poAcknowledgement.TransactionNumber = GenerateTransactionNumber(poAcknowledgement.ReferenceNumber);
            ctx.Update(poAcknowledgement);
            ctx.SaveChanges();
            return poAcknowledgement.ReferenceNumber;
        }

        private string GenerateTransactionNumber(int referenceNum)
        {
            string numToString = referenceNum.ToString();
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
        private string GenerateGroupNumber(int referenceNum)
        {
            string numToString = referenceNum.ToString();
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
        private string GenerateInterchangeNumber(int referenceNum)
        {
            string numToString = referenceNum.ToString();
            if (string.IsNullOrWhiteSpace(numToString))
            {
                return numToString;
            }
            else
            {
                var newNum = numToString.Substring(numToString.Length - 9, 9);
                if (newNum.Length < 9)
                {
                    //make it fill with zeros if it's not 9 digits... or maybe it should invalidate...
                }
                return newNum;
            }
        }
    }
}
