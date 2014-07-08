using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using GoeMerchant.API.Payments.Extensions;

namespace GoeMerchant.API.Payments.Entity {
    public class Check {
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
        public GoeMerchant.API.Payments.Enum.ACHAccountType AccountType { get; set; }

        /// <summary>
        /// The category setup in the virtual that corresponds to this transaction
        /// </summary>
        public string CategoryText { get; set; }

        /// <summary>
        /// The date the transaction is intended to post if not immediate
        /// </summary>
        [XmlIgnore]
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// Account nickname Ex: CitiBank Checking
        /// </summary>
        [XmlIgnore]
        public string AccountName { get; set; }

        internal List<Field> ToFields() {
            var fields = new List<Field>();

            fields.Add(new Field("aba", this.RoutingNumber));
            fields.Add(new Field("dda", this.AccountNumber));
            fields.Add(new Field("ach_account_type", this.AccountType.ToDescription()));
            fields.Add(new Field("ach_category_text", this.CategoryText));
            fields.Add(new Field("close_date", this.CloseDate.HasValue ? this.CloseDate.Value.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/tttt")));
            fields.Add(new Field("ach_name", this.AccountName));

            return fields;
        }
    }
}
