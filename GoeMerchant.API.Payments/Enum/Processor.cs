using System.ComponentModel;

namespace GoeMerchant.API.Payments.Enum {
    public enum Processor {
        [Description("echeck")]
        eCheck,
        [Description("paymentech")]
        PaymentTech,
        [Description("nova")]
        Nova,
        [Description("fdc")]
        FDC,
        [Description("vital")]
        Vital,
        [Description("fifththird")]
        FifthThird,
        [Description("fnms")]
        FNMS,
        [Description("global")]
        Global
    }
}
