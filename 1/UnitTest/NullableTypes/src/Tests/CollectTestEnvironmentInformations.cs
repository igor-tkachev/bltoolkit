//
// NullableTypes.Tests.CollectTestEnvironmentInformations
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes   Reasons
// 21-Sep-2003  Luca    Create
// 06-Oct-2003  Luca    Upgrade   Code upgrade: Replaced tabs with spaces
//


namespace NullableTypes.Tests { 

    using nu = NUnit.Framework;
    using nua = NUnit.Framework.Assertion;
    using sys = System;
    using sysGlb = System.Globalization;
    using sysRfl = System.Reflection;

    [nu.TestFixture]
    public class CollectTestEnvironmentInformations    {

        [nu.Test]
        public void EnvironmentInformationsToStandardOutput() {

            // Collect infos to reproduce in-house the submitted bug 

            sys.Console.WriteLine();
            sys.Console.WriteLine("Assembly Versions");
            sys.Console.WriteLine("--------------------");
            sys.Console.WriteLine(sysRfl.Assembly.GetExecutingAssembly().FullName);
            sys.Console.WriteLine(" Product Version: " + ((sysRfl.AssemblyInformationalVersionAttribute)sysRfl.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(sysRfl.AssemblyInformationalVersionAttribute), false).GetValue(0)).InformationalVersion);
            sys.Console.WriteLine(typeof(NullableString).Assembly.FullName);
            sys.Console.WriteLine(" Product Version: " + ((sysRfl.AssemblyInformationalVersionAttribute)typeof(NullableString).Assembly.GetCustomAttributes(typeof(sysRfl.AssemblyInformationalVersionAttribute), false).GetValue(0)).InformationalVersion);

            sys.Console.WriteLine();
            sys.Console.WriteLine("Machine Versions");
            sys.Console.WriteLine("--------------------");
            sys.Console.WriteLine(" OS: " + sys.Environment.OSVersion.ToString());
            sys.Console.WriteLine(" CLR: " + sys.Environment.Version.ToString());

            sys.Console.WriteLine();
            sys.Console.WriteLine("Regional Settings");
            sys.Console.WriteLine("--------------------");
            sys.Console.WriteLine("{0} {1} {2}",
                sysGlb.CultureInfo.CurrentCulture.EnglishName,
                sysGlb.CultureInfo.CurrentCulture.Name,
                sysGlb.CultureInfo.CurrentCulture.LCID);
            sys.Console.WriteLine("{0} {1}  -  {2} {3}", 
                sysGlb.DateTimeFormatInfo.CurrentInfo.ShortDatePattern,
                sysGlb.DateTimeFormatInfo.CurrentInfo.ShortTimePattern,
                sysGlb.DateTimeFormatInfo.CurrentInfo.LongDatePattern,
                sysGlb.DateTimeFormatInfo.CurrentInfo.LongTimePattern);
            sys.Console.WriteLine("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}",
                System.Environment.NewLine,
                "CurrencyDecimalDigits " + sysGlb.NumberFormatInfo.CurrentInfo.CurrencyDecimalDigits,
                "CurrencyDecimalSeparator " + sysGlb.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator,
                "CurrencyGroupSeparator " + sysGlb.NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator,
                "CurrencyNegativePattern " + sysGlb.NumberFormatInfo.CurrentInfo.CurrencyNegativePattern,
                "CurrencyPositivePattern " + sysGlb.NumberFormatInfo.CurrentInfo.CurrencyPositivePattern,
                "CurrencySymbol " + sysGlb.NumberFormatInfo.CurrentInfo.CurrencySymbol,
                "NegativeSign " + sysGlb.NumberFormatInfo.CurrentInfo.NegativeSign,
                "NumberDecimalDigits " + sysGlb.NumberFormatInfo.CurrentInfo.NumberDecimalDigits,
                "NumberDecimalSeparator " + sysGlb.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator,
                "NumberGroupSeparator " + sysGlb.NumberFormatInfo.CurrentInfo.NumberGroupSeparator,
                "NumberNegativePattern " + sysGlb.NumberFormatInfo.CurrentInfo.NumberNegativePattern,
                "PositiveSign " + sysGlb.NumberFormatInfo.CurrentInfo.PositiveSign);
        }            
    }
}
