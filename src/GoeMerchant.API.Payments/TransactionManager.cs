﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoeMerchant.API.Payments.Entity;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Linq;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GoeMerchant.API.Payments {
    public class TransactionManager {
        public static async Task<TransactionResponse> RequestPayment(TransactionRequest transaction) {
            using (var client = new HttpClient()) {
                var httpContent = new StringContent(transaction.ToXml(), Encoding.UTF8, "application/xml");

                var result = await client.PostAsync("https://secure.goemerchant.com/secure/gateway/xmlgateway.aspx", httpContent);
                string resultContent = await result.Content.ReadAsStringAsync();

                XmlSerializer serializer = new XmlSerializer(typeof(TransactionResponse));
                TransactionResponse transactionResponse = (TransactionResponse)serializer.Deserialize(new StringReader(resultContent));

                // A return currently only one transaction is returned at a time
                if (transactionResponse.Fields.Fields.FirstOrDefault(x => x.Key == "total_transactions_credited") != null) {
                    if (!transactionResponse.HasField("status")) {
                        transactionResponse.Fields.Add(new Field("status", transactionResponse.GetFieldValue("status1")));
                    }

                    if (!transactionResponse.HasField("response")) {
                        transactionResponse.Fields.Add(new Field("response", transactionResponse.GetFieldValue("response1")));
                    }

                    if (!transactionResponse.HasField("reference_number")) {
                        transactionResponse.Fields.Add(new Field("reference_number", transactionResponse.GetFieldValue("reference_number1")));
                    }

                    if (!transactionResponse.HasField("credit_amount")) {
                        transactionResponse.Fields.Add(new Field("credit_amount", transactionResponse.GetFieldValue("credit_amount1")));
                    }

                    if (!transactionResponse.HasField("error")) {
                        transactionResponse.Fields.Add(new Field("error", transactionResponse.GetFieldValue("error1")));
                    }
                }

                return transactionResponse;
            }
        }

        public static async Task<List<QueryResponse>> RequestData(TransactionQuery transaction) {
            var returnResponse = new List<QueryResponse>();
            using (var client = new HttpClient()) {
                var httpContent = new StringContent(transaction.ToXml(), Encoding.UTF8, "application/xml");

                var result = await client.PostAsync("https://secure.goemerchant.com/secure/gateway/xmlgateway.aspx", httpContent);
                string resultContent = await result.Content.ReadAsStringAsync();

                XmlSerializer serializer = new XmlSerializer(typeof(TransactionResponse));
                TransactionResponse transactionResponse = (TransactionResponse)serializer.Deserialize(new StringReader(resultContent));

                var totalRecords = transactionResponse.Fields.Fields.Where(x => x.Key == "records_found").SingleOrDefault();

                if (totalRecords != null) {
                    int records = 0;

                    if (int.TryParse(totalRecords.Value, out records)) {
                        for (int i = 1; i <= records; i++) {
                            var queryResponse = new QueryResponse();
                            queryResponse.OrderID = transactionResponse.GetFieldValue("order_id" + i);
                            queryResponse.Amount = transactionResponse.GetDecimalFieldValue("amount" + i);
                            queryResponse.AmountCredited = transactionResponse.GetDecimalFieldValue("amount_credited" + i);
                            queryResponse.AmountSetted = transactionResponse.GetDecimalFieldValue("amount_settled" + i);
                            queryResponse.Settled = transactionResponse.GetFieldValue("auth_response" + i) == "Settled" ? 1 : 0;
                            queryResponse.TransactionTime = transactionResponse.GetDateTimeFieldValue("trans_time" + i);
                            queryResponse.CardType = transactionResponse.GetFieldValue("card_type" + i);
                            queryResponse.authResponse = transactionResponse.GetFieldValue("auth_response" + i);
                            queryResponse.CreditVoid = transactionResponse.GetFieldValue("credit_void" + i);
                            queryResponse.TransactionStatus = transactionResponse.GetIntegerFieldValue("trans_status" + i);
                            queryResponse.ReferenceNumber = transactionResponse.GetFieldValue("reference_number" + i);
                            queryResponse.RejectDate = transactionResponse.GetDateTimeFieldValue("reject_date" + i);
                            queryResponse.Description = transactionResponse.GetFieldValue("description" + i);
                            queryResponse.ReturnCode = transactionResponse.GetFieldValue("return_code" + i);

                            var hashes = parseXML(resultContent, i);

                            foreach (string k in hashes.Keys) {
                                if (k.Contains("field_name")) {
                                    var number = k.Substring(10);

                                    queryResponse.AdditionalFields.Add(new Field {
                                        Key = hashes[k].ToString(),
                                        Value = hashes["field_value" + number].ToString()
                                    });
                                }
                            }

                            returnResponse.Add(queryResponse);
                        }
                    }
                }
            }
            return returnResponse;
        }
       

        /// <summary>
        /// takes in raw xml string and attempts to parse it into a workable hash.
        /// all valid xml for the gateway contains
        /// <transaction><fields><field key="attribute name">value</field></fields></transaction>
        /// there will be 1 or more (should always be more than 1 to be valid) field tags
        /// this method will take the attribute name and make that the hash key and then the value is the value
        /// if an error occurs then the error key will be added to the hash.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private static Hashtable parseXML(string xml, int number) {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            Hashtable ret_hash = new Hashtable(); //stores key values to return
            XmlTextReader txtreader = null;
            XmlValidatingReader reader = null;
            if (xml != null && xml.Length > 0) {
                try {
                    //Implement the readers.
                    txtreader = new XmlTextReader(new System.IO.StringReader(xml));
                    reader = new XmlValidatingReader(txtreader);
                    //Parse the XML and display the text content of each of the elements.
                    while (reader.Read()) {
                        Console.WriteLine(reader.Value);
                        if (reader.IsStartElement() && reader.Name.ToLower() == "field") {
                            if (reader.HasAttributes) {
                                if (reader.GetAttribute(0).ToLower().Contains("additional_fields" + number)) {
                                    XmlNode node = doc.DocumentElement.SelectSingleNode("//FIELD[@KEY='" + reader.GetAttribute(0).ToLower() + "']");

                                    if (node.HasChildNodes) {
                                        foreach (XmlNode current in node.ChildNodes) {
                                            if (current.Attributes != null && current.Attributes.Count > 0) {
                                                foreach (XmlAttribute attribute in current.Attributes) {
                                                    if (attribute.Value.Contains("field_name") || attribute.Value.Contains("field_value")) {
                                                        ret_hash[attribute.Value] = current.InnerText;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else {
                                    //we want the key attribute value
                                    //ret_hash[reader.GetAttribute(0).ToLower()] = reader.ReadString();
                                }
                            }
                            else {
                                ret_hash["error"] = "All FIELD tags must contains a KEY attribute.";
                            }
                        }
                    } //ends while
                }
                catch (Exception e) {
                    //handle exceptions
                    ret_hash["error"] = e.Message;
                }
                finally {
                    if (reader != null)
                        reader.Close();
                }
            }

            return ret_hash;
        }
    }
}
