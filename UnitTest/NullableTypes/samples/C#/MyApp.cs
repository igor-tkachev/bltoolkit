namespace samples {        

    using System;
    using System.Globalization;
    using NullableTypes;
    using NullableTypes.HelperFunctions;

    sealed class MyApp {

        static void Main() {

            Console.WriteLine();
            Console.WriteLine("NullableInt32 Sample");
            NullableInt32Sample();

            Console.WriteLine();
            Console.WriteLine("NullableString Sample");
            NullableStringSample();

            Console.WriteLine();
            Console.WriteLine("NullableTypesAndDataset Sample");
            NullableTypesAndDatasetSample();

            Console.WriteLine();
            Console.WriteLine("NullableTypesAndUIControls Sample");
            NullableTypesAndUIControlsSample();

            return;
        }
	

        private static void NullableInt32Sample() {
            /*
             * 
             * Definition samples
             * 
             */

            // declaration 
            NullableInt32 ni; 
            
            // declaration and initialization 
            NullableInt32 nj = 320; 

            // declaration and initialization to Null
            NullableInt32 nk = NullableInt32.Null; 



            /*
             * 
             * Operators and expressions
             * 
             */
             
            ni = nj * nk;
            Console.WriteLine(" 1) {0}", ni); // -> NullableInt32.Null

            nj *= 11;
            Console.WriteLine(" 2) {0}", nj); // -> 3520



            /*
             * 
             * Assignments and conversions
             * 
             */
            
            int i = 3200;
            ni = i + 100; // an implicit conversion happen before the assignment
            i = (int)ni;  // this conversion can raise an exception if 'ni' is Null
            Console.WriteLine(" 3) {0}", i); // -> 3300


            /*
             * 
             * Dealing with Null values
             * 
             */

            nk = NullableInt32.Null; // set to null
            nj = 2;
            ni = nk / nj; 
            // Test for Null
            Console.WriteLine(" 4) {0}", ni.IsNull); // -> bool.true
            if (ni.IsNull) 
                Console.WriteLine("    if-true" );  // <-
            else 
                Console.WriteLine("    if-NOT true");
			
//            // Optional advanced topics
//
//            // Equality operator:  If either instance of NullableInt32 is Null, the Value of the comparison 
//            //                     will be NullableBoolean.Null
//            Console.WriteLine(" 5) {0}", (ni == NullableInt32.Null)); // -> NullableBoolean.Null
//            if (ni == NullableInt32.Null) 
//                Console.WriteLine("    if-true");
//            else 
//                Console.WriteLine("    if-NOT true"); // <-
//
//            // Equals:  Compares two NullableInt32 structures to determine if they are equivalent. 
//            //          Returns true if the parameter is a NullableInt32 and the two instances are equivalent; 
//            //          otherwise false. 
//            Console.WriteLine(" 5-bis) {0}", ni.Equals(NullableInt32.Null)); // -> bool.true
//            if (ni.Equals(NullableInt32.Null))
//                Console.WriteLine("        if-true"); // <-
//            else 
//                Console.WriteLine("        if-NOT true");


            // Comparison with a Null value return a NullableBoolean.Null,
            // the 'if' will fail (as if false) with a NullableBoolean.Null
            Console.WriteLine(" 6) {0}", (ni > nk));  // -> NullableBoolean.Null
            if (ni > nk)
                Console.WriteLine("    if-true");
            else
                Console.WriteLine("    if-NOT true"); // <-


            // Comparison with a Null value return a NullableBoolean.Null,
            // the 'if' will fail (as if false) with a NullableBoolean.Null
            Console.WriteLine(" 7) {0}", (ni < nk));  // -> NullableBoolean.Null
            if (ni < nk)
                Console.WriteLine("    if-true");
            else
                Console.WriteLine("    if-NOT true"); // <-

            // Getting the Value property raise an exception if the 
            // NullableInt32 variable is Null
            try {
                ni = NullableInt32.Null;
                int k = ni.Value;
            }
            catch (NullableNullValueException ne) {
                Console.WriteLine(" 8) {0}", ne.Message); // -> " Data is Null. This method or property cannot be called on Null values."
            }
			
            // An explicit conversion to int raise an exception if the 
            // NullableInt32 variable is Null
            try {
                ni = NullableInt32.Null;
                int k = (int)ni;
            }
            catch (NullableNullValueException ne) {
                Console.WriteLine(" 9) {0}", ne.Message); // -> " Data is Null. This method or property cannot be called on Null values."
            }
    


            /*
             * 
             * Overflow
             * 
             */

            // NullableInt32 will always raise an exception when an overflow happen no matter if
            // 'checked' or 'unchecked' is specified. This is due to the fact that NullableInt32 is not
            // a real CLR built-in type (note that the same thing apply to System.Decimal).
            nj = NullableInt32.MaxValue;
            try {
                nj = checked( nj * nj );
            }
            catch(OverflowException e) {
                Console.WriteLine("10) {0}", e.Message); // -> "Arithmetic operation resulted in an overflow."
            }

            // NullableInt32 will always raise an exception when an overflow happen no matter if
            // 'checked' or 'unchecked' is specified. This is due to the fact that NullableInt32 is not
            // a real CLR built-in type (note that the same thing apply to System.Decimal).
            nj = NullableInt32.MaxValue;
            try {
                nj = unchecked( nj * nj );
            }
            catch(OverflowException e) {
                Console.WriteLine("11) {0}", e.Message); // -> "Arithmetic operation resulted in an overflow."
            }		
        }


        private static void NullableStringSample() {

            // A NullableString type can sound very strange because System.String is a Reference-Type 
            // and it can already be null (Nothing in Visual Basic).
            // Anyway a NullableString parameter (or field or property) explicity states that a Null 
            // value is welcome: you wont get an ArgumentNullException passing Null to a NullableString 
            // method parameter (or field or property).
            // Moreover NullableString permits to treat Null values for a String like for any other
            // type of NullableTypes.

            /*
             * 
             * Definition samples
             * 
             */

            // declaration 
            NullableString ns; 
            
            // declaration and initialization 
            NullableString nt = "Hello nullable string!"; 

            // declaration and initialization to Null
            NullableString nu = NullableString.Null; 

            // declaration and initialization using a NullableString constructor
            NullableString nv = new NullableString('v', 100); 

            // initializing from null System.String variable do construct a NullableString.Null
            string nullString = null;
            NullableString nw = new NullableString(nullString); 
            Console.WriteLine(" 0) {0}", nw.IsNull); // -> bool.true


            /*
             * 
             * Operators and expressions
             * 
             */
             
            ns = nt + nu;
            Console.WriteLine(" 1) '{0}'", ns); // -> 'Null'

            nt += " Hello from System.String too";
            Console.WriteLine(" 2) '{0}'", nt); // -> 'Hello nullable string! Hello from System.String too'



            /*
             * 
             * Assignments and conversions
             * 
             */
            
            string s = "s string";
            ns = s + " and a const string"; // an implicity conversion happen before the assignment
            s = (string)ns;  // this conversion can raise an exception if 'ns' is Null
            Console.WriteLine(" 3) '{0}'", s); // -> 'Hello nullable string! Hello from System.String too'


            /*
             * 
             * Dealing with Null values
             * 
             */

            ns = NullableString.Null; // set to null
            nt = "the nt string";
            nu = ns + nt; 
            // Test for Null
            Console.WriteLine(" 4) {0}", nu.IsNull); // -> bool.true
            if (nu.IsNull) 
                Console.WriteLine("    if-true"); // <-
            else 
                Console.WriteLine("    if-NOT true");
			
//            // Optional advanced topics
//            //
//            // Equality operator:  If either instance of NullableString is Null, the Value of the comparison 
//            //                     will be NullableBoolean.Null
//            Console.WriteLine(" 5) {0}", (nu == NullableString.Null)); // -> NullableBoolean.Null
//            if (nu == NullableString.Null) 
//                Console.WriteLine("    if-true");
//            else 
//                Console.WriteLine("    if-NOT true"); // <-
//            
//
//            // Equals:  Compares two NullableString structures to determine if they are equivalent. 
//            //          Returns true if the parameter is a NullableString and the two instances are equivalent; 
//            //          otherwise false. 
//            Console.WriteLine(" 5-bis) {0}", (nu.Equals(NullableString.Null))); // -> bool.true
//            if (nu.Equals(NullableString.Null)) 
//                Console.WriteLine("        if-true"); // <-
//            else 
//                Console.WriteLine("        if-NOT true");


            // Equivalence comparison with a Null value return a NullableBoolean.Null,
            // the 'if' will fail (as if false) with a NullableBoolean.Null
            nu = NullableString.Null;
            Console.WriteLine(" 6) {0}", (NullableString.Equals(nu, nv))); // -> NullableBoolean.Null
            if (NullableString.Equals(nu, nv))
                Console.WriteLine("    if-true");
            else 
                Console.WriteLine("    if-NOT true"); // <-

            // Equivalence comparison with a Null value return a NullableBoolean.Null,
            // the 'if' will fail (as if false) with a NullableBoolean.Null
            Console.WriteLine(" 7) {0}", ((NullableString.NotEquals(nu, nv)))); // -> NullableBoolean.Null
            if ((NullableString.NotEquals(nu, nv)))
                Console.WriteLine("    if-true");
            else 
                Console.WriteLine("    if-NOT true"); // <-

            // Getting the Value property raise an exception if the 
            // NullableString variable is Null
            try {
                nu = NullableString.Null;
                string u = nu.Value;
            }
            catch (NullableNullValueException ne) {
                Console.WriteLine(" 8) {0}", ne.Message); // -> "Data is Null. This method or property cannot be called on Null values."
            }
			
            // An explicit conversion to string raise an exception if the 
            // NullableString variable is Null
            try {
                nu = NullableString.Null;
                string u = (string)nu;
            }
            catch (NullableNullValueException ne) {
                Console.WriteLine(" 9) {0}", ne.Message); // -> "Data is Null. This method or property cannot be called on Null values."
            }
    


            /*
             * 
             * System.String members
             * 
             */

            // It's still possible to use all System.String members with NullableString getting
            // the string Value, when this is not Null.
            nu = "some value";
            bool b = false;
            if (!nu.IsNull)
                b = nu.Value.StartsWith("som");
        }


        private static void NullableTypesAndDatasetSample() {
            // A DataTable built in-memory
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("WHEN", typeof(System.DateTime));
            dt.Columns["WHEN"].AllowDBNull = true;

            // Populate the DataTable with a new row
            System.Data.DataRow row = dt.NewRow();

            /*
             * 
             * Set the column value of a DataTable row from a NullableDateTime value
             * 
             */

            // without NullableTypes
            row["WHEN"] = DBNull.Value; 
            Console.WriteLine(" 1) {0}", row["WHEN"]); // -> string.Empty

            // with a non-null NullableDateTime
            NullableDateTime when = new NullableDateTime(DateTime.Now);
            row["WHEN"] = DBNullConvert.From(when); 
            Console.WriteLine(" 2) {0}", row["WHEN"]); // -> the current time

            // with a null NullableDateTime
            when = NullableDateTime.Null;
            row["WHEN"] = DBNullConvert.From(when); 
            Console.WriteLine(" 3) {0}", row["WHEN"]); // -> string.Empty

            /*
             * 
             * Assign the column value of a DataTable row to a NullableDateTime variable
             * 
             */

            // a null column value into a NullableDateTime
            row["WHEN"] = DBNull.Value; // the column value now is NULL
            NullableDateTime nd1 = DBNullConvert.ToNullableDateTime(row["WHEN"]);
            Console.WriteLine(" 4) {0}", nd1.IsNull); // -> True


            // a non-null column value into a NullableDateTime
            row["WHEN"] = DateTime.Now; // the column value now is NULL
            NullableDateTime nd2 = DBNullConvert.ToNullableDateTime(row["WHEN"]);
            Console.WriteLine(" 5) {0}", nd2.Value); // -> the current time

        }


        private static void NullableTypesAndUIControlsSample() {
            
            // A TextBox built in-memory
            System.Web.UI.WebControls.TextBox TextBox1 = new System.Web.UI.WebControls.TextBox();

            /*
             * 
             * Set the TextBox text from a NullableDateTime value
             * 
             */

            // a null NullableDateTime into a TextBox
            NullableDateTime dt1 = NullableDateTime.Null;
            TextBox1.Text = NullConvert.From(dt1, string.Empty, "G");
            Console.WriteLine(" 1) {0}", TextBox1.Text); // -> string.Empty

            // a non-null NullableDateTime into a TextBox
            NullableDateTime dt2 = new NullableDateTime(DateTime.Now);
            TextBox1.Text = NullConvert.From(dt2, string.Empty, "G");
            Console.WriteLine(" 2) {0}", TextBox1.Text); // -> the current time

            // a non-null NullableDateTime into a TextBox
            NullableDateTime dt3 = new NullableDateTime(DateTime.Now);
            TextBox1.Text = NullConvert.From(dt2, string.Empty, 
                CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern);
            Console.WriteLine(" 3) {0}", TextBox1.Text); // -> the current time

            /*
             * 
             * Set the TextBox text from a NullableDecimal value
             * 
             */

            // a null NullableDecimal into a TextBox 
            NullableDecimal dc1 = NullableDecimal.Null;
            TextBox1.Text = NullConvert.From(dc1, string.Empty, "##,###,###.00000");
            Console.WriteLine(" 4) {0}", TextBox1.Text); // -> string.Empty

            // a non-null NullableDecimal into a TextBox 
            NullableDecimal dc2 = 12345678.091M;
            TextBox1.Text = NullConvert.From(dc2, string.Empty, "##,###,###.00000"); // try format "N"
            Console.WriteLine(" 5) {0}", TextBox1.Text); // -> 12.345.678,09100 

            /*
             * 
             * Assign the text value of a TextBox to a NullableDateTime variable
             * Note: the conventional text for a null date is string.Empty in this sample
             * 
             */

            // an empty TextBox into a NullableDateTime
            TextBox1.Text = string.Empty; 
            NullableDateTime dt4 = NullConvert.ToNullableDateTime(TextBox1.Text, string.Empty);
            Console.WriteLine(" 6) {0}", dt4.IsNull); // -> True

            // a non-empty TextBox into a NullableDateTime
            TextBox1.Text = DateTime.Now.ToString("G"); 
            NullableDateTime dt5 = NullConvert.ToNullableDateTime(TextBox1.Text, string.Empty);
            Console.WriteLine(" 7) {0}", dt5.Value.ToString("G")); // -> the current time

            /*
             * 
             * Assign the text value of a TextBox to a NullableDecimal variable
             * Note: the conventional text for a null date is string.Empty in this sample
             * 
             */

            // an empty TextBox into a NullableDecimal
            TextBox1.Text = string.Empty; 
            NullableDecimal dc3 = NullConvert.ToNullableDecimal(TextBox1.Text, string.Empty);
            Console.WriteLine(" 8) {0}", dc3.IsNull); // -> True

            // a non-empty TextBox into a NullableDecimal
            TextBox1.Text = (12345678.091M).ToString("N");
            NullableDecimal dc4 = NullConvert.ToNullableDecimal(TextBox1.Text, string.Empty);
            Console.WriteLine(" 9) {0}", dc4.Value.ToString()); // -> 12345678,09M formatted with current culture

        }

    }
}
