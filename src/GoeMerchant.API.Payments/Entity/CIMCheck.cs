using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using GoeMerchant.API.Payments.Extensions;

namespace GoeMerchant.API.Payments.Entity {
    public class CIMCheck {
        public string Sequence { get; set; }
        /// <summary>
        /// The category setup in the virtual that corresponds to this transaction
        /// </summary>
        public string CategoryText { get; set; }

        internal List<Field> ToFields() {
            var fields = new List<Field>();

            fields.Add(new Field("ach_sequence", this.Sequence));
            fields.Add(new Field("ach_category_text", this.CategoryText));

            return fields;
        }
    }
}
