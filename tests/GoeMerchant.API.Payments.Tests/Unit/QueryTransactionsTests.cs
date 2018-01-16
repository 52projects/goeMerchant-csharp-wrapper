using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using System.Configuration;
using GoeMerchant.API.Payments;
using GoeMerchant.API.Payments.Entity;
using GoeMerchant.API.Payments.Enum;

namespace GoeMerchant.API.Payments.Tests.Unit {
    public class QueryTransactionsTests {
        private string _transactionCenterID = string.Empty;
        private string _gatewayID = string.Empty;
        private int _processorID = 0;

        [OneTimeSetUp]
        public void Setup() {
            _transactionCenterID = ConfigurationManager.AppSettings["Transaction.Center.ID"];
            _gatewayID = ConfigurationManager.AppSettings["Gateway.ID"];
            _processorID = int.Parse(ConfigurationManager.AppSettings["Processor.ID"]);
        }
        [Test]
        public void unit_query_transactions() {
            var qo = new GoeMerchant.API.Payments.Entity.TransactionQuery(_transactionCenterID, _gatewayID) {
                OrderID = "1515738059938",
                OperationType = OperationType.Query,
                BeginDate = DateTime.UtcNow.AddDays(-5),
                EndDate = DateTime.UtcNow.AddDays(-3),
                Transaction = "ACH",
                TransactionStatus = 1,
                TransactionType = OperationType.ALL,
                IncludeAdditionalFields = true,
                ProcessorID = _processorID
            };

            var response = GoeMerchant.API.Payments.TransactionManager.RequestData(qo);

            if (response != null) {
                
            }
        }
    }
}
            