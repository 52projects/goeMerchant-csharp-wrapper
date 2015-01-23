using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoeMerchant.API.Payments.Entity {
    public class QueryResponse {
        public QueryResponse() {
            this.AdditionalFields = new List<Field>();
        }
        public string OrderID { get; set; }

        public decimal Amount { get; set; }

        public decimal AmountSetted { get; set; }

        public decimal AmountCredited { get; set; }

        public int Settled { get; set; }

        public DateTime TransactionTime { get; set; }

        public string CardType { get; set; }

        public string authResponse { get; set; }

        public string CreditVoid { get; set; }

        public string AuthCode { get; set; }

        public string NameOnCard { get; set; }

        public string CardExpiration { get; set; }

        public string TransactionType { get; set; }

        public int TransactionStatus { get; set; }

        public string ReferenceNumber { get; set; }

        public List<Field> AdditionalFields { get; set; }

        public string ReturnCode { get; set; }

        public DateTime RejectDate { get; set; }

        public string Description { get; set; }

    }
}
