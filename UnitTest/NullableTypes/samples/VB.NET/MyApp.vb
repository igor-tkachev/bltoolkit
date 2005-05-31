Imports NullableTypes
Imports NullableTypes.HelperFunctions
Imports System.Globalization

Module MyApp

    Sub Main()
        Console.WriteLine()
        Console.WriteLine("NullableInt32 Sample")
        NullableInt32Sample()

        Console.WriteLine()
        Console.WriteLine("NullableString Sample")
        NullableStringSample()

        Console.WriteLine()
        Console.WriteLine("NullableTypesAndDataset Sample")
        NullableTypesAndDatasetSample()

        Console.WriteLine()
        Console.WriteLine("NullableTypesAndUIControls Sample")
        NullableTypesAndUIControlsSample()

        Console.Read()
    End Sub

    Private Sub NullableInt32Sample()
        '
        ' 
        ' Definition samples
        '
        ' declaration 
        Dim ni As NullableInt32

        ' declaration and initialization
        Dim nj As New NullableInt32(320)
        ' an alternative way is to call C# defined impicit conversion operator
        'Dim nj As NullableInt32 = NullableInt32.op_Implicit(320) 

        ' declaration and initialization to Null
        Dim nk As NullableInt32 = NullableInt32.Null

        ni = NullableInt32.Multiply(nj, nk)
        Console.WriteLine(" 1) {0}", ni) ' -> NullableInt32.Null

        nj = New NullableInt32(nj.Value * 11)
        Console.WriteLine(" 2) {0}", nj) ' -> 3520

        '
        ' 
        ' Assignments and conversions
        ' 
        ' 

        Dim i As Integer = 3200
        ni = New NullableInt32(i + 100) ' an implicit conversion happen before the assignment
        i = ni.Value  ' this conversion can raise an exception if 'ni' is Null
        Console.WriteLine(" 3) {0}", i) ' -> 3300

        '
        ' 
        ' Dealing with Null values
        ' 
        '

        nk = NullableInt32.Null 'set to null
        nj = New NullableInt32(2)
        ni = NullableInt32.Divide(nk, nj)

        ' Test for Null
        Console.WriteLine(" 4) {0}", ni.IsNull) ' -> Boolean.True
        If ni.IsNull Then
            Console.WriteLine("    If-True") ' <-
        Else
            Console.WriteLine("    If-NOT True")
        End If


        '' Optional advanced topics
        ''
        '' Equality operator:  If either instance of NullableInt32 is Null, the Value of the comparison 
        ''                     will be NullableBoolean.Null        
        'Console.WriteLine(" 5) {0}", NullableInt32.Equals(ni, NullableInt32.Null)) ' -> NullableBoolean.Null
        'If NullableInt32.Equals(ni, NullableInt32.Null).Equals(NullableBoolean.True) Then
        '    Console.WriteLine("    If-True ")
        'Else
        '    Console.WriteLine("    If-NOT True") ' <-
        'End If

        '' Equals:  Compares two NullableInt32 structures to determine if they are equivalent. 
        ''          Returns true if the parameter is a NullableInt32 and the two instances are equivalent; 
        ''          otherwise false. 
        'Console.WriteLine(" 5-bis) {0}", ni.Equals(NullableInt32.Null)) ' -> Boolean.True
        'If ni.Equals(NullableInt32.Null) Then
        '    Console.WriteLine("        If-True") ' <-
        'Else
        '    Console.WriteLine("        If-NOT True")
        'End If


        ' Comparison with a Null value return a NullableBoolean.Null,
        ' the 'if' will fail (as if false) with a NullableBoolean.Null
        Console.WriteLine(" 6) {0}", NullableInt32.GreaterThan(nj, nk)) ' -> NullableBoolean.Null
        If NullableInt32.GreaterThan(nj, nk).IsTrue Then
            Console.WriteLine("    If-True")
        Else
            Console.WriteLine("    If-NOT True") ' <-
        End If

        ' Comparison with a Null value return a NullableBoolean.Null,
        ' the 'if' will fail (as if false) with a NullableBoolean.Null
        Console.WriteLine(" 7) {0}", NullableInt32.LessThan(ni, nk)) ' -> NullableBoolean.Null
        If NullableInt32.LessThan(ni, nk).IsTrue Then
            Console.WriteLine("    If-True")
        Else
            Console.WriteLine("    If-NOT True") ' <-
        End If

        ' Getting the Value property raise an exception if the 
        ' NullableInt32 variable is Null
        Try
            ni = NullableInt32.Null
            Dim k As Integer = ni.Value
        Catch ne As NullableNullValueException
            Console.WriteLine(" 8) {0}", ne.Message) ' -> "Data is Null. This method or property cannot be called on Null values."
        End Try

        ' An explicit conversion to int raise an exception if the
        ' NullableInt32 variable is Null
        Try
            ni = NullableInt32.Null
            Dim k As Integer = NullableInt32.op_Explicit(ni)
        Catch ne As NullableNullValueException
            Console.WriteLine(" 9) {0}", ne.Message) ' -> "Data is Null. This method or property cannot be called on Null values."
        End Try

        '
        ' 
        ' Overflow
        ' 
        '

        ' NullableInt32 will always raise an exception when an overflow happen no matter if
        ' 'checked' or 'unchecked' is specified. This is due to the fact that NullableInt32 is not
        ' a real CLR built-in type (note that the same thing apply to System.Decimal).        

        'You will get the OverflowException also compiling with option /removeintchecks

        nj = NullableInt32.MaxValue
        Try
            nj = NullableInt32.Multiply(nj, nj)
        Catch e As OverflowException
            Console.WriteLine("10) {0}", e.Message) ' -> "Arithmetic operation resulted in an overflow."
        End Try

    End Sub

    Private Sub NullableStringSample()
        ' A NullableString type can sound very strange because System.String is a Reference-Type 
        ' and it can already be null (Nothing in Visual Basic).
        ' Anyway a NullableString parameter (or field or property) explicity states that a Null 
        ' value is welcome: you wont get an ArgumentNullException passing Null to a NullableString 
        ' method parameter (or field or property).
        ' Moreover NullableString permits to treat Null values for a String like for any other
        ' type of NullableTypes.

        '
        ' 
        ' Definition samples
        ' 
        '

        ' declaration 
        Dim ns As NullableString

        ' declaration and initialization 
        Dim nt As NullableString = New NullableString("Hello nullable string!")
        ' an alternative way is to call C# defined impicit conversion operator
        'Dim nj As NullableString = NullableString.op_Implicit("Hello nullable string!")

        ' declaration and initialization to Null
        Dim nu As NullableString = NullableString.Null

        ' declaration and initialization using a NullableString constructor
        Dim nv As NullableString = New NullableString("v"c, 100)

        ' initializing from null System.String variable do construct a NullableString.Null
        Dim nullString As String = Nothing
        Dim nw As NullableString = New NullableString(nullString)
        Console.WriteLine(" 0) {0}", nw.IsNull) ' -> Boolean.True

        '
        ' 
        ' Operators and expressions
        ' 
        '

        ns = NullableString.Concat(nt, nu)
        Console.WriteLine(" 1) '{0}'", ns) ' -> 'Null'

        nt = NullableString.Concat(nt, New NullableString(" Hello from System.String too"))
        Console.WriteLine(" 2) '{0}'", nt) ' -> 'Hello nullable string! Hello from System.String too'



        '
        ' 
        ' Assignments and conversions
        ' 
        '

        Dim s As String = "s string"
        ns = New NullableString(s + " and a const string")
        s = ns.Value  ' this conversion can raise an exception if 'ns' is Null
        Console.WriteLine(" 3) '{0}'", s) ' -> 's string and a const string'

        '
        ' 
        ' Dealing with Null values
        ' 
        '

        ns = NullableString.Null ' set to null
        nt = New NullableString("the nt string")
        nu = NullableString.Concat(ns, nt)
        ' Test for Null
        Console.WriteLine(" 4) {0}", nu.IsNull) ' -> Boolean.True
        If nu.IsNull Then
            Console.WriteLine("    if-True") ' <-
        Else
            Console.WriteLine("    if-NOT True")
        End If


        '' Optional advanced topics
        ''
        '' Equality operator:  If either instance of NullableString is Null, the Value of the comparison 
        ''                     will be NullableBoolean.Null
        'Console.WriteLine(" 5) {0}", NullableString.Equals(nu, NullableString.Null)) ' -> NullableBoolean.Null
        'If NullableString.Equals(nu, NullableString.Null).Equals(NullableBoolean.True) Then
        '    Console.WriteLine("    if-True")
        'Else
        '    Console.WriteLine("    if-NOT True") ' <- 
        'End If

        '' Equals:  Compares two NullableString structures to determine if they are equivalent. 
        ''          Returns true if the parameter is a NullableString and the two instances are equivalent; 
        ''          otherwise false. 
        'Console.WriteLine(" 5-bis) {0}", nu.Equals(NullableString.Null)) ' Boolean.True
        'If nu.Equals(NullableString.Null) Then
        '    Console.WriteLine("        if-True") ' <-
        'Else
        '    Console.WriteLine("        if-NOT True")
        'End If

        ' Equivalence comparison with a Null value return a NullableBoolean.Null,
        ' the 'if' will fail (as if false) with a NullableBoolean.Null
        nu = NullableString.Null
        Console.WriteLine(" 6) {0}", NullableString.Equals(nu, nv)) ' -> NullableBoolean.Null
        If NullableString.Equals(nu, nv).IsTrue Then
            Console.WriteLine("    if-True")
        Else
            Console.WriteLine("    if-NOT True") ' <-
        End If


        ' Equivalence comparison with a Null value return a NullableBoolean.Null,
        ' the 'if' will fail (as if false) with a NullableBoolean.Null
        Console.WriteLine(" 7) {0}", NullableString.NotEquals(nu, nv)) ' -> NullableBoolean.Null
        If NullableString.NotEquals(nu, nv).IsTrue Then
            Console.WriteLine("    if-True")
        Else
            Console.WriteLine("    if-NOT True") ' <-
        End If

        ' Getting the Value property raise an exception if the 
        ' NullableString variable is Null
        Try
            nu = NullableString.Null
            Dim u As String = nu.Value
        Catch ne As NullableNullValueException
            Console.WriteLine(" 8) {0}", ne.Message) ' -> "Data is Null. This method or property cannot be called on Null values."
        End Try

        ' An explicit conversion to string raise an exception if the 
        ' NullableString variable is Null
        Try
            nu = NullableString.Null
            Dim u As String = NullableString.op_Explicit(nu)
        Catch ne As NullableNullValueException
            Console.WriteLine(" 9) {0}", ne.Message) ' -> "Data is Null. This method or property cannot be called on Null values."
        End Try

        '
        ' 
        ' System.String members
        ' 
        '

        ' It's still possible to use all System.String members with NullableString getting
        ' the string Value, when this is not Null.
        nu = NullableString.op_Implicit("some value")
        Dim b As Boolean = False
        If Not nu.IsNull Then b = nu.Value.StartsWith("som")

    End Sub

    Private Sub NullableTypesAndDatasetSample()
        ' A DataTable built in-memory
        Dim dt As New System.Data.DataTable
        dt.Columns.Add("WHEN", GetType(System.DateTime))
        dt.Columns("WHEN").AllowDBNull = True
        ' Populate the DataTable with a new row
        Dim row As DataRow = dt.NewRow()

        '
        ' 
        ' Set the column value of a DataTable row from a NullableDateTime value
        ' 
        '

        ' without NullableTypes
        row("WHEN") = DBNull.Value
        Console.WriteLine(" 1) {0}", row("WHEN")) ' -> string.Empty

        ' with a non-null NullableDateTime
        Dim [when] As New NullableDateTime(DateTime.Now)
        row("WHEN") = DBNullConvert.From([when])
        Console.WriteLine(" 2) {0}", row("WHEN"))  ' -> the current time

        ' with a null NullableDateTime
        [when] = NullableDateTime.Null
        row("WHEN") = DBNullConvert.From([when])
        Console.WriteLine(" 3) {0}", row("WHEN")) ' -> string.Empty

        '
        ' 
        ' Assign the column value of a DataTable row to a NullableDateTime variable
        ' 
        '

        ' a null column value into a NullableDateTime
        row("WHEN") = DBNull.Value ' the column value now is NULL
        Dim nd1 As NullableDateTime = DBNullConvert.ToNullableDateTime(row("WHEN"))
        Console.WriteLine(" 4) {0}", nd1.IsNull) ' -> True

        ' a non-null column value into a NullableDateTime
        row("WHEN") = DateTime.Now ' the column value now is NULL
        Dim nd2 As NullableDateTime = DBNullConvert.ToNullableDateTime(row("WHEN"))
        Console.WriteLine(" 5) {0}", nd2.Value) ' -> the current time
    End Sub

    Private Sub NullableTypesAndUIControlsSample()
        ' A TextBox built in-memory
        Dim TextBox1 As New System.Web.UI.WebControls.TextBox

        '
        ' 
        ' Set the TextBox text from a NullableDateTime value
        ' 
        '

        ' a null NullableDateTime into a TextBox
        Dim dt1 As NullableDateTime = NullableDateTime.Null
        TextBox1.Text = NullConvert.From(dt1, String.Empty, "G")
        Console.WriteLine(" 1) {0}", TextBox1.Text) ' -> string.Empty

        ' a non-null NullableDateTime into a TextBox
        Dim dt2 As New NullableDateTime(DateTime.Now)
        TextBox1.Text = NullConvert.From(dt2, String.Empty, "G")
        Console.WriteLine(" 2) {0}", TextBox1.Text) ' -> the current time


        ' a non-null NullableDateTime into a TextBox
        Dim dt3 As New NullableDateTime(DateTime.Now)
        TextBox1.Text = NullConvert.From(dt2, String.Empty, CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern)
        Console.WriteLine(" 3) {0}", TextBox1.Text) ' -> the current time

        '
        ' 
        ' Set the TextBox text from a NullableDecimal value
        ' 
        '

        ' a null NullableDecimal into a TextBox 
        Dim dc1 As NullableDecimal = NullableDecimal.Null
        TextBox1.Text = NullConvert.From(dc1, String.Empty, "##,###,###.00000")
        Console.WriteLine(" 4) {0}", TextBox1.Text) ' -> string.Empty

        ' a non-null NullableDecimal into a TextBox 
        Dim dc2 As NullableDecimal = NullableDecimal.op_Implicit(12345678.091)
        TextBox1.Text = NullConvert.From(dc2, String.Empty, "##,###,###.00000") ' try format "N"
        Console.WriteLine(" 5) {0}", TextBox1.Text) ' -> 12.345.678,09100

        '
        ' 
        ' Assign the text value of a TextBox to a NullableDateTime variable
        ' Note: the conventional text for a null date is string.Empty in this sample
        ' 
        '


        ' an empty TextBox into a NullableDateTime
        TextBox1.Text = String.Empty
        Dim dt4 As NullableDateTime = NullConvert.ToNullableDateTime(TextBox1.Text, String.Empty)
        Console.WriteLine(" 6) {0}", dt4.IsNull) ' -> True


        '
        ' 
        ' Assign the text value of a TextBox to a NullableDecimal variable
        ' Note: the conventional text for a null date is string.Empty in this sample
        ' 
        '
        ' a non-empty TextBox into a NullableDateTime
        TextBox1.Text = DateTime.Now.ToString("G")
        Dim dt5 As NullableDateTime = NullConvert.ToNullableDateTime(TextBox1.Text, String.Empty)
        Console.WriteLine(" 7) {0}", dt5.Value.ToString("G")) ' -> the current time

        ' an empty TextBox into a NullableDecimal
        TextBox1.Text = String.Empty
        Dim dc3 As NullableDecimal = NullConvert.ToNullableDecimal(TextBox1.Text, String.Empty)
        Console.WriteLine(" 8) {0}", dc3.IsNull) ' -> True

        ' a non-empty TextBox into a NullableDecimal
        TextBox1.Text = (12345678.091).ToString("N")
        Dim dc4 As NullableDecimal = NullConvert.ToNullableDecimal(TextBox1.Text, String.Empty)
        Console.WriteLine(" 9) {0}", dc4.Value.ToString()) ' -> 12345678,09M formatted with current culture
    End Sub
End Module