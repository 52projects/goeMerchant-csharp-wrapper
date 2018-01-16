using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using System.Configuration;
using GoeMerchant.API.Payments;
using GoeMerchant.API.Payments.Entity;
using GoeMerchant.API.Payments.Enum;

namespace GoeMerchants.API.Payments.Tests.Unit {
    [TestFixture]
    public class SendPaymentTests {
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
        public async Task unit_send_payment_success() {
            Random rgen = new Random();
            var transaction = new TransactionRequest(_transactionCenterID, _gatewayID);
            transaction.OperationType = OperationType.Sale;
            transaction.OrderID = string.Format("{0}", rgen.Next(0, 1000) + rgen.Next(1000, 5000));
            transaction.OrderTotal = 2.00;
            transaction.ProcessorID = _processorID;

            transaction.CreditCard = new CreditCard {
                CardType = CardType.Visa,
                CardNumber = "4716389275666851",
                CardExpiration = "1214",
                CloseDate = DateTime.Now
            };

            transaction.BillingAddress = new BillingAddress {
                PersonName = "Joe Test",
                Street = "123 Test St",
                State = "TX",
                Country = "USA",
                Zipcode = "12345-6789"
            };

            transaction.LineItems.Add(new LineItem {
                ItemSku = "1111",
                Description = "Test sku",
                Quantity = 1,
                Price = 2.00
            });

            transaction.CustomFields.Add(new Field("test", @" this is a test"));
            var results = await TransactionManager.RequestPayment(transaction);
            results.Status.ShouldBe(1);
        }

        [Test]
        public async Task unit_send_void_success() {
            Random rgen = new Random();
            var transaction = new TransactionRequest(_transactionCenterID, _gatewayID);
            transaction.OperationType = OperationType.Sale;
            transaction.OrderID = string.Format("{0}", rgen.Next(0, 1000) + rgen.Next(1000, 5000));
            transaction.OrderTotal = 2.00;
            transaction.ProcessorID = _processorID;

            transaction.CreditCard = new CreditCard {
                CardType = CardType.Visa,
                CardNumber = "4716389275666851",
                CardExpiration = "0618",
                CloseDate = DateTime.Now
            };

            transaction.BillingAddress = new BillingAddress {
                PersonName = "Joe Test",
                Street = "123 Test St",
                State = "TX",
                Country = "USA",
                Zipcode = "12345-6789"
            };

            transaction.LineItems.Add(new LineItem {
                ItemSku = "1111",
                Description = "Test sku",
                Quantity = 1,
                Price = 2.00
            });

            transaction.CustomFields.Add(new Field("test", @" this is a test"));
            var results = await TransactionManager.RequestPayment(transaction);
            

            if (results.Status != 1) {
                return;
            }

            var voidTransaction = new TransactionRequest(_transactionCenterID, _gatewayID);
            voidTransaction.CreditTransactions.Add(new Field(results.ReferenceNumber, transaction.OrderTotal.ToString()));
            voidTransaction.OperationType = OperationType.Void;
            var voidResults = await TransactionManager.RequestPayment(voidTransaction);
            voidResults.Status.ShouldBe(1);
        }

        [Test]
        public async Task unit_create_custom_field() {
            var transaction = new TransactionRequest();
            transaction.CustomFields.Add(new Field("color", "red"));
            transaction.CustomFields.Add(new Field("fund", "missions"));
            transaction.BillingAddress = new BillingAddress();

            var xml = transaction.ToXml();

            xml.ShouldContain("field_name1");
        }
    }
}
