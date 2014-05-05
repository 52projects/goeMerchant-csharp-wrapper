using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoeMerchant.API.Payments.Extensions;

namespace GoeMerchant.API.Payments.Entity {
    public class RecurringBilling {
        /// <summary>
        /// Indicates the recurring interval for this transaction to repeat.
        /// </summary>
        public GoeMerchant.API.Payments.Enum.RecurringType RecurringType { get; set; }

        /// <summary>
        /// The date the recurring transaction is intended to start.
        /// </summary>
        public DateTime RecurringStartDate { get; set; }

        /// <summary>
        /// The date the recurring transaction is intended to end.
        /// </summary>
        public DateTime RecurringEndDate { get; set; }

        /// <summary>
        /// Indicates if the customer should receive an invoice each time a recurring billing transaction is processed.
        /// </summary>
        public bool ShouldSendInvoice { get; set; }

        internal List<Field> ToFields(bool isACH) {
            var fields = new List<Field>();

            fields.Add(new Field("recurring", "1"));
            fields.Add(new Field("recurrint_type", this.RecurringType.ToDescription()));
            fields.Add(new Field("recurring_start_date", this.RecurringStartDate.ToShortDateString()));
            fields.Add(new Field("recurring_end_date", this.RecurringEndDate.ToShortDateString()));
            fields.Add(new Field("send_invoice", this.ShouldSendInvoice ? "1" : "0"));
            fields.Add(new Field("is_ach", isACH ? "1" : "0"));

            return fields;
        }
    }
}
