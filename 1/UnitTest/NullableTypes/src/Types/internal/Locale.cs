//
// NullableTypes.Locale
// 
// Author: Luca Minudel (dev@minudel.it)
//
// Date         Author  Changes    Reasons
// 24-Feb-2003  Luca    Create
// 12-May-2003  Luca    Bug Fix    Added check for missing string
// 07-Jul-2003  Luca    Upgrade    Applied FxCop guideline: class sealed, make default constructor private
// 06-Oct-2003  Luca    Upgrade    Code upgrade: Replaced tabs with spaces
//

namespace NullableTypes
{
    using sys = System;
    using sysRes = System.Resources;

#if DEBUG
    public sealed class Locale{
#else
    internal sealed class Locale{
#endif

        private Locale() {}

        private static sysRes.ResourceManager errorMessages;

        public static string GetText(string key) {
            if (errorMessages == null)
                errorMessages = new sysRes.ResourceManager("NullableTypes.internal.messages", 
                    typeof(Locale).Assembly);

            string text = errorMessages.GetString(key, System.Globalization.CultureInfo.InvariantCulture);
            if (text == null)
                throw new sys.IndexOutOfRangeException();

            return text;
        }
    }
}
