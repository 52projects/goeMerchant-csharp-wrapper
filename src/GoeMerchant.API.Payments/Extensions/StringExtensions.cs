using System;
using System.ComponentModel;
using System.Reflection;

namespace GoeMerchant.API.Payments.Extensions {
    public static class StringExtensions {
        public static string ToDescription(this System.Enum en) {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0) {
                object[] attrs = memInfo[0].GetCustomAttributes(
                   typeof(DescriptionAttribute),
                   false);

                if (attrs != null && attrs.Length > 0) {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }
    }
}
