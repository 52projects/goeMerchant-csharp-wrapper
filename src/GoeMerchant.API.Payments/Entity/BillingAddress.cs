using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoeMerchant.API.Payments.Entity {
    public class BillingAddress {
        /// <summary>
        /// Payment provider account name
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// Billing street address
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Billing street address 2
        /// </summary>
        public string Street2 { get; set; }

        /// <summary>
        /// Billing city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Billing state
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Billing zipcode
        /// </summary>
        public string Zipcode { get; set;  }

        /// <summary>
        /// Billing country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Billing email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Billing Phone number
        /// </summary>
        public string Phone { get; set; }

        internal List<Field> ToFields() {
            var fields = new List<Field>();

            fields.Add(new Field("owner_name", this.PersonName));
            fields.Add(new Field("owner_street", this.Street));
            fields.Add(new Field("owner_street2", this.Street2));
            fields.Add(new Field("owner_city", this.City));
            fields.Add(new Field("owner_state", this.State));
            fields.Add(new Field("owner_zip", this.Zipcode));
            fields.Add(new Field("owner_country", this.Country));
            fields.Add(new Field("owner_email", this.Email));
            fields.Add(new Field("owner_phone", this.Phone));

            return fields;
        }
    }
}
