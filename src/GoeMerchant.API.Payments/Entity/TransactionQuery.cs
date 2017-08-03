using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using GoeMerchant.API.Payments.Extensions;
using System.IO;
using GoeMerchant.API.Payments.Enum;

namespace GoeMerchant.API.Payments.Entity {
    [XmlRoot("TRANSACTION")]
    public class TransactionQuery {
        /// <summary>
        /// Instantiates a Transaction object to send to GoeMerchant. Remember to specify the transaction center ID and the gateway ID
        /// </summary>
        public TransactionQuery() {

        }

        /// <summary>
        /// Instantiates a Transaction object to send to GoeMerchant
        /// </summary>
        /// <param name="transactionCenterID">Unique identifier assigned by gateway. This is your unique Transaction Center number</param>
        /// <param name="gatewayID">Unique identifier assigned by gateway. Can be found and or reset via the Options Tab in the Transaction Center.</param>
        public TransactionQuery(string transactionCenterID, string gatewayID) {
            this.Fields = new FieldCollection();

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

        [XmlIgnore]
        public string Transaction { get; set; }

        [XmlIgnore]
        public  CardType? CardType { get; set; }

        [XmlIgnore]
        public OperationType? TransactionType { get; set; }

        [XmlIgnore]
        public int? TransactionStatus { get; set; }

        [XmlIgnore]
        public DateTime BeginDate { get; set; }

        [XmlIgnore]
        public DateTime EndDate { get; set; }

        [XmlIgnore]
        public decimal? LowAmount { get; set; }

        [XmlIgnore]
        public decimal? HighAmount { get; set; }

        [XmlIgnore]
        public string OrderID { get; set; }

        [XmlIgnore]
        public string CardNumber { get; set; }

        [XmlIgnore]
        public bool IncludeAdditionalFields { get; set; }

        public string ToXml() {
            this.Fields = new FieldCollection();
            this.Fields.Add(new Field("transaction_center_id", this.TransactionCenterID));
            this.Fields.Add(new Field("gateway_id", this.GatewayID));
            this.Fields.Add(new Field("operation_type", this.OperationType.ToDescription().ToLower()));

            // If the processor id is available, there is no need to check the mid / tid/ processor
            if (this.ProcessorID.HasValue) {
                this.Fields.Add(new Field("processor_id", this.ProcessorID.ToString()));
            }
            else {
                if (!string.IsNullOrEmpty(MID)) {
                    this.Fields.Add(new Field("mid", this.MID));
                    this.Fields.Add(new Field("processor", this.Processor.ToDescription()));
                }
            }

            if (this.OperationType == Enum.OperationType.Query) {
                this.Fields.Add(new Field("trans_action", this.Transaction));
                this.Fields.Add(new Field("trans_type", this.TransactionType.HasValue ? this.TransactionType.ToDescription() : ""));
                this.Fields.Add(new Field("trans_status", this.TransactionStatus.HasValue ? this.TransactionStatus.Value.ToString() : ""));
                this.Fields.Add(new Field("begin_date", this.BeginDate.ToString("MMddyy")));
                this.Fields.Add(new Field("end_date", this.EndDate.ToString("MMddyy")));
                this.Fields.Add(new Field("low_amount", this.LowAmount.HasValue ? this.LowAmount.Value.ToString() : ""));
                this.Fields.Add(new Field("high_amount", this.HighAmount.HasValue ? this.HighAmount.Value.ToString() : ""));

                if (!string.IsNullOrEmpty(this.OrderID)) {
                    this.Fields.Add(new Field("order_id", this.OrderID));
                }

                if (this.Transaction == "CC") {
                    this.Fields.Add(new Field("card_number", this.CardNumber));
                    this.Fields.Add(new Field("card_type", this.CardType.HasValue ? this.CardType.ToDescription() : ""));
                }

                this.Fields.Add(new Field("include_additional_fields", this.IncludeAdditionalFields ? "1" : ""));
            }

            if (this.OperationType == Enum.OperationType.QueryRejects) {
                this.Fields.Add(new Field("begin_date", this.BeginDate.ToString("MMddyy")));
                this.Fields.Add(new Field("end_date", this.EndDate.ToString("MMddyy")));
                this.Fields.Add(new Field("low_amount", this.LowAmount.HasValue ? this.LowAmount.Value.ToString() : ""));
                this.Fields.Add(new Field("high_amount", this.HighAmount.HasValue ? this.HighAmount.Value.ToString() : ""));
                this.Fields.Add(new Field("order_id", this.OrderID));
                this.Fields.Add(new Field("include_additional_fields", this.IncludeAdditionalFields ? "1" : ""));
            }

            XmlSerializer xsSubmit = new XmlSerializer(typeof(TransactionQuery));
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
