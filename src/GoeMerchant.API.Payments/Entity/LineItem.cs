using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoeMerchant.API.Payments.Entity {
    public class LineItem {
        /// <summary>
        /// An item identifier or sku code 
        /// </summary>
        public string ItemSku { get; set; }

        /// <summary>
        /// Quantity of item purchased
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Text description of item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Amount in US dollars
        /// </summary>
        public double Price { get; set; }

        internal List<Field> ToFields(int index) {
            var fields = new List<Field>();

            fields.Add(new Field("item_quantity" + index, this.Quantity.ToString()));
            fields.Add(new Field("item_number" + index, this.ItemSku));
            fields.Add(new Field("item_description" + index, this.Description));
            fields.Add(new Field("item_price" + index, this.Price.ToString()));

            return fields;
        }
    }
}
