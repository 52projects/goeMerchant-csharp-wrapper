using System.ComponentModel;

namespace GoeMerchant.API.Payments.Enum {
    public enum CardType {
        [Description("Visa")]
        Visa,
        [Description("Mastercard")]
        Mastercard,
        [Description("Amex")]
        Amex,
        [Description("Discover")]
        Discover
    }
}
