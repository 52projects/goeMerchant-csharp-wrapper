using System.ComponentModel;

namespace GoeMerchant.API.Payments.Enum {
    public enum ACHAccountType {
        [Description("C")]
        Checking,
        [Description("S")]
        Savings
    }
}
