using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace GoeMerchant.API.Payments.Entity {
    [XmlRoot("FIELD")]
    public class Field {
        #region Constructor
        public Field() {

        }

        public Field(string key, string value) {
            this.Key = key;
            this.Value = value;
        }
        #endregion Constructor

        [XmlAttribute("KEY")]
        public string Key { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
