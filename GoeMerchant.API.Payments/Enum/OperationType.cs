using System.ComponentModel;

namespace GoeMerchant.API.Payments.Enum {
    public enum OperationType {
        [Description("AUTH")]
        Auth = 1,
        [Description("SALE")]
        Sale = 2,
        [Description("ACH_DEBIT")]
        ACHDebit = 3,
        [Description("ACH_CREDIT")]
        ACHCredit = 4,
        [Description("RETAIL_AUTH")]
        RetailAuth = 5,
        [Description("RETAIL_SALE")]
        RetailSale = 6,
        [Description("CIM_AUTH")]
        CIMAuth = 7,
        [Description("CIM_SALE")]
        CIMSale = 8,
        [Description("CIM_EDIT")]
        CIMEdit = 9,
        [Description("CIM_DELETE")]
        CIMDelete = 10,
        [Description("CIM_ACH_DEBIT")]
        CIMACHDebit = 11,
        [Description("CIM_ACH_CREDIT")]
        CIMACHCredit = 12,
        [Description("REAUTH")]
        ReAuth = 13,
        [Description("RESALE")]
        ReSale = 14,
        [Description("CREDIT")]
        Credit = 15,
        [Description("RETAIL_ALONE_CREDIT")]
        RetailAloneCredit = 16,
        [Description("VOID")]
        Void = 17,
        [Description("SETTLE")]
        Settle = 18,
        [Description("QUERY")]
        Query = 19,
        [Description("RECURRING_MODIFY")]
        RecurringModiy = 10
    }
}