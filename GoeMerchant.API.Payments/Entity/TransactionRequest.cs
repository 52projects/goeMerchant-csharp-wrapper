using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using GoeMerchant.API.Payments.Extensions;
using System.IO;

namespace GoeMerchant.API.Payments.Entity {
    [XmlRoot("TRANSACTION")]
    public class TransactionRequest {
        /// <summary>
        /// Instantiates a Transaction object to send to GoeMerchant. Remember to specify the transaction center ID and the gateway ID
        /// </summary>
        public TransactionRequest() {
            this.CustomFields = new List<Field>();
            this.LineItems = new List<LineItem>();
        }

        /// <summary>
        /// Instantiates a Transaction object to send to GoeMerchant
        /// </summary>
        /// <param name="transactionCenterID">Unique identifier assigned by gateway. This is your unique Transaction Center number</param>
        /// <param name="gatewayID">Unique identifier assigned by gateway. Can be found and or reset via the Options Tab in the Transaction Center.</param>
        public TransactionRequest(string transactionCenterID, string gatewayID) {
            this.Fields = new FieldCollection();
            this.LineItems = new List<LineItem>();
            this.CustomFields = new List<Field>();

            _transactionCenterID = transactionCenterID;
            _gatewayID = gatewayID;
        }

        [XmlElement("FIELDS")]
        public FieldCollection Fields { get; set; }

        private string _transactionCenterID;
        [XmlIgnore]
        public string TransactionCenterID { get { return _transactionCenterID; } set { _transactionCenterID = value; } }

        private string _gatewayID;
        [XmlIgnore]
        public string GatewayID { get { return _gatewayID; } set { _gatewayID = value; } }

        /// <summary>
        /// String specifying operation attempting to be run
        /// </summary>
        [XmlIgnore]
        public GoeMerchant.API.Payments.Enum.OperationType OperationType { get; set; }

        /// <summary>
        /// Specific merchant number or firstfund username for the transaction to be processed under. This is only applicable if you have multiple accounts associated with your transaction center IDd.
        /// </summary>
        [XmlIgnore]
        public string MID { get; set; }

        /// <summary>
        /// Specific terminal number this transaction. This is required if the mid is supplied and it is not an ACH transaction
        /// </summary>
        [XmlIgnore]
        public string TID { get; set; }

        /// <summary>
        /// Processor for the mid/tid combination suppled. This is required if the mid is supplied.
        /// </summary>
        [XmlIgnore]
        public GoeMerchant.API.Payments.Enum.Processor? Processor { get; set; }

        /// <summary>
        /// Used in place of mid/tid/processor this.Fields to identify the processor to run the transaction under. This is a vailable in the transaction center.
        /// </summary>
        [XmlIgnore]
        public int? ProcessorID { get; set; }

        /// <summary>
        /// IP address of the customer contacting the merchant's site or application
        /// </summary>
        [XmlIgnore]
        public string RemoteIPAddress { get; set; }

        /// <summary>
        /// Unique order id or invoice number. Cannot contain "insert", "update" or "delete".
        /// </summary>
        [XmlIgnore]
        public string OrderID { get; set; }

        /// <summary>
        /// Amount in US dollars.
        /// </summary>
        [XmlIgnore]
        public double OrderTotal { get; set; }

        [XmlIgnore]
        public BillingAddress BillingAddress { get; set; }

        [XmlIgnore]
        public CreditCard CreditCard { get; set; }

        [XmlIgnore]
        public Check Check { get; set; }

        [XmlIgnore]
        public RecurringBilling RecurringBilling { get; set; }

        [XmlIgnore]
        public List<LineItem> LineItems { get; set; }
        
        /// <summary>
        /// Used to add any custom field to send to Goe. The field key will be the field_nameN and the field value will be the field_valueN
        /// </summary>
        [XmlIgnore]
        public List<Field> CustomFields { get; set; }

        public string ToXml() {
            this.Fields = new FieldCollection();
            this.Fields.Add(new Field("transaction_center_id", this.TransactionCenterID));
            this.Fields.Add(new Field("gateway_id", this.GatewayID));
            this.Fields.Add(new Field("operation_type", this.OperationType.ToDescription()));

            // If the processor id is available, there is no need to check the mid / tid/ processor
            if (this.ProcessorID.HasValue) {
                this.Fields.Add(new Field("processor_id", this.ProcessorID.ToString()));
            }
            else {
                if (!string.IsNullOrEmpty(MID)) {
                    this.Fields.Add(new Field("mid", this.MID));

                    if (CreditCard != null) {
                        this.Fields.Add(new Field("tid", this.TID));
                    }

                    this.Fields.Add(new Field("processor", this.Processor.ToDescription()));
                }
            }

            this.Fields.Add(new Field("order_id", this.OrderID));
            this.Fields.Add(new Field("total", string.Format("{0:N2}", this.OrderTotal)));

            if (CreditCard != null) {
                this.Fields.AddRange(this.CreditCard.ToFields());
            }

            if (Check != null) {
                this.Fields.AddRange(this.Check.ToFields());
            }

            if (this.BillingAddress == null) {
                throw new Exception("Billing address is required.");
            }
            this.Fields.AddRange(this.BillingAddress.ToFields());

            if (this.RecurringBilling != null) {
                this.Fields.AddRange(this.RecurringBilling.ToFields(this.Check != null));
            }

            this.Fields.Add(new Field("remote_ip_address", this.RemoteIPAddress));

            for (int i = 1; i <= this.LineItems.Count(); i++) {
                this.Fields.AddRange(this.LineItems[i - 1].ToFields(i));
            }

            if (this.CustomFields.Count > 0) {
                this.Fields.Add(new Field("total_additioanl_fields", this.CustomFields.Count().ToString()));
                for (int i = 1; i <= this.CustomFields.Count(); i++) {
                    this.Fields.Add(new Field("field_name" + i.ToString(), this.CustomFields[i-1].Key));
                    this.Fields.Add(new Field("field_value" + i.ToString(), this.CustomFields[i-1].Value));
               }
            }

            XmlSerializer xsSubmit = new XmlSerializer(typeof(TransactionRequest));
            StringWriter sww = new StringWriter();
            XmlWriter writer = XmlWriter.Create(sww);
            xsSubmit.Serialize(writer, this);
            var xml = sww.ToString(); // Your xml

            writer.Dispose();
            sww.Dispose();

            return xml;
        }
    }
}