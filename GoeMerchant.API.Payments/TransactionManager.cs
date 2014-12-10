using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoeMerchant.API.Payments.Entity;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace GoeMerchant.API.Payments {
    public class TransactionManager {
        public static TransactionResponse RequestPayment(TransactionRequest transaction) {
            string url = @"https://secure.goemerchant.com/secure/gateway/xmlgateway.aspx";

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";

            byte[] transactionBytes = System.Text.Encoding.GetEncoding(1252).GetBytes(transaction.ToXml());
            webRequest.ContentLength = transactionBytes.Length;

            using (Stream writer = webRequest.GetRequestStream())
                writer.Write(transactionBytes, 0, transactionBytes.Length);

            using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse()) {
                //Only for debug
                using (var stream = new StreamReader(webResponse.GetResponseStream())) {
                    var response = stream.ReadToEnd();
                    XmlSerializer serializer = new XmlSerializer(typeof(TransactionResponse));
                    return (TransactionResponse)serializer.Deserialize(new StringReader(response));
                }
            }
        }

        public static TransactionResponse RequestData(TransactionQuery transaction) {
            string url = @"https://secure.goemerchant.com/secure/gateway/xmlgateway.aspx";

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";
            var xml = transaction.ToXml();

            byte[] transactionBytes = System.Text.Encoding.GetEncoding(1252).GetBytes(transaction.ToXml());
            webRequest.ContentLength = transactionBytes.Length;

            using (Stream writer = webRequest.GetRequestStream())
                writer.Write(transactionBytes, 0, transactionBytes.Length);

            using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse()) {
                //Only for debug
                using (var stream = new StreamReader(webResponse.GetResponseStream())) {
                    var response = stream.ReadToEnd();
                    XmlSerializer serializer = new XmlSerializer(typeof(TransactionResponse));
                    return (TransactionResponse)serializer.Deserialize(new StringReader(response));
                }
            }
        }
    }
}
