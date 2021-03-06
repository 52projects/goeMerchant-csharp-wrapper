﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using GoeMerchant.API.Payments.Extensions;
using System.IO;

namespace GoeMerchant.API.Payments.Entity {
    [XmlRoot("RESPONSE")]
    public class TransactionResponse {

        public TransactionResponse() {
            this.Fields = new FieldCollection();
        }

        [XmlElement("FIELDS")]
        public FieldCollection Fields { get; set; }

        [XmlIgnore]
        public int Status {
            get {
                int status = 0;

                var field = Fields.GetFieldValue("status");

                if (!string.IsNullOrEmpty(field)) {
                    int.TryParse(field, out status);
                }

                return status;
            }
        }

        [XmlIgnore]
        public string Response {
            get {
                return Fields.GetFieldValue("response");
            }
        }

        [XmlIgnore]
        public string AuthCode {
            get {
                return Fields.GetFieldValue("auth_code");
            }
        }

        [XmlIgnore]
        public string AuthResponse {
            get {
                return Fields.GetFieldValue("auth_response");
            }
        }


        [XmlIgnore]
        public string AVSCode {
            get {
                return Fields.GetFieldValue("avs_code");
            }
        }

        [XmlIgnore]
        public string CVVCode {
            get {
                return Fields.GetFieldValue("cvv2_code");
            }
        }

        [XmlIgnore]
        public string OrderID {
            get {
                return Fields.GetFieldValue("order_id");
            }
        }

        [XmlIgnore]
        public string ReferenceNumber {
            get {
                return Fields.GetFieldValue("reference_number");
            }
        }

        [XmlIgnore]
        public string AvailableBalance {
            get {
                return Fields.GetFieldValue("available_balance");
            }
        }

        [XmlIgnore]
       public decimal? CreditAmount {
            get {
                decimal amount = 0;
                var value = Fields.GetFieldValue("credit_amount");

                if (!string.IsNullOrEmpty(value)) {
                    decimal.TryParse(value, out amount);
                }

                return amount;
            }
        }

        [XmlIgnore]
        public string ErrorMessage {
            get {
                return Fields.GetFieldValue("error");
            }
        }

        public string GetFieldValue(string key) {
            var result = Fields.Fields.SingleOrDefault(x => x.Key == key);

            if (result != null) {
                return result.Value;
            }

            return string.Empty;
        }

        public DateTime GetDateTimeFieldValue(string key) {
            var date = DateTime.MinValue;
            var results = GetFieldValue(key);

            DateTime.TryParse(results, out date);
            return date;
        }

        public int GetIntegerFieldValue(string key) {
            var value = int.MinValue;
            var results = GetFieldValue(key);

            int.TryParse(results, out value);
            return value;
        }

        public decimal GetDecimalFieldValue(string key) {
            var value = decimal.MinValue;
            var results = GetFieldValue(key);

            decimal.TryParse(results, out value);
            return value;
        }

        public bool HasField(string key) {
            return Fields.Fields.FirstOrDefault(x => x.Key == key) != null;
        }
    }
}
