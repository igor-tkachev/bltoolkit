<%@ Register TagPrefix="uc1" TagName="Menu" Src="Menu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Header.ascx" %>
<%@ Page language="c#" Codebehind="Default.aspx.cs" AutoEventWireup="false" Inherits="WebRFD._Default" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>The Rsdn.Framework.Data namespace</title>
<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema>
<meta http-equiv=Content-Type content="text/html; charset=windows-1251">
<meta content="ORM, .NET, C#, object-relational mapping" name=keywords><LINK href="rfd.css" type=text/css rel=stylesheet >
  </HEAD>
<body marginheight="0" marginwidth="0">
<TABLE height="100%" cellSpacing=0 cellPadding=0 width="100%" border=0>
  <TR>
    <TD class=hdr colSpan=2><uc1:header id=Header1 runat="server"></uc1:Header></TD></TR>
  <TR>
    <TD class=menu vAlign=top><uc1:menu id=Menu1 runat="server"></uc1:Menu></TD>
    <TD class=art vAlign=top width="100%" height="100%"><font size=2>
<!------ Article Starts ----->

<H2>Examples</H2>

<P>I won’t make you bored by providing you with the complete list of the library functions and features here. You can find them in the documentation. Instead let’s take a look at a few examples that give you some idea of what the library is and how it can be used.</P>
<OL>
<LI>Basic example, just an introduction 

<p><TABLE class="code" width="97%"><TR><TD><PRE><font size=2><SPAN class="KEYWORD">using</SPAN> System;

<SPAN class="KEYWORD">using</SPAN> Rsdn.Framework.Data;

<SPAN class="KEYWORD">namespace</SPAN> Example
{
<SPAN class="KEYWORD">    class</SPAN> Test
    {
<SPAN class="KEYWORD">        static</SPAN> <SPAN class="KEYWORD">void</SPAN> Main()
        {
<SPAN class="KEYWORD">            using</SPAN> (DbManager db = <SPAN class="KEYWORD">new</SPAN> DbManager())
            {
<SPAN class="KEYWORD">                string</SPAN> name = (<SPAN class="KEYWORD">string</SPAN>)db.ExecuteScalar(<SPAN class="STRING">@"
                    SELECT
                        CategoryName
                    FROM
                        Categories 
                    WHERE
                        CategoryID = @id"</SPAN>,
                    db.Parameter(<SPAN class="STRING">"@id"</SPAN>, 1));

                Console.WriteLine(name);
            }
        }
    }
}
</font></PRE></TD></TR></TABLE></p>

<LI>Default configuration.

<P>App.config</P>

<p><TABLE class="code" width="98%"><TR><TD><PRE><font size=2>&lt;configuration&gt;
    &lt;appSettings&gt;
        &lt;add 
            key   = <SPAN class="STRING">"ConnectionString"</SPAN> 
            value = <SPAN class="STRING">"Server=(local);Database=NorthwindDev;Integrated Security=SSPI"</SPAN> /&gt;
    &lt;/appSettings&gt;
&lt;configuration&gt;</font></PRE></TD></TR></TABLE></p>

<P>Test.cs</P>

<p><TABLE class="code" width="98%"><TR><TD><PRE><font size=2><SPAN class="KEYWORD">using</SPAN> System;

<SPAN class="KEYWORD">using</SPAN> Rsdn.Framework.Data;

<SPAN class="KEYWORD">namespace</SPAN> Example
{
<SPAN class="KEYWORD">    class</SPAN> Test
    {
<SPAN class="KEYWORD">        static</SPAN> <SPAN class="KEYWORD">void</SPAN> Main()
        {
<SPAN class="KEYWORD">            using</SPAN> (DbManager db = <SPAN class="KEYWORD">new</SPAN> DbManager())
            {
<SPAN class="COMMENT">                // ...</SPAN>
            }
        }
    }
}</font></PRE></TD></TR></TABLE></p>

<LI>Advanced configuration

<P>App.config</P>

<p><TABLE class="code" width="98%"><TR><TD><PRE><font size=2>&lt;configuration&gt;
    &lt;appSettings&gt;
        &lt;add 
            key   = <SPAN class="STRING">"ConnectionString.Development"</SPAN> 
            value = <SPAN class="STRING">"Server=(local);Database=NorthwindDev;Integrated Security=SSPI"</SPAN> /&gt;
        &lt;add 
            key   = <SPAN class="STRING">"ConnectionString.Production"</SPAN> 
            value = <SPAN class="STRING">"Server=(local);Database=Northwind;Integrated Security=SSPI"</SPAN> /&gt;
    &lt;/appSettings&gt;
&lt;configuration&gt;</font></PRE></TD></TR></TABLE></p>

<P>Test.cs</P>

<p><TABLE class="code" width="98%"><TR><TD><PRE><font size=2><SPAN class="KEYWORD">using</SPAN> System;

<SPAN class="KEYWORD">using</SPAN> Rsdn.Framework.Data;

<SPAN class="KEYWORD">namespace</SPAN> Example
{
    <SPAN class="KEYWORD">class</SPAN> Test
    {
        <SPAN class="KEYWORD">static</SPAN> <SPAN class="KEYWORD">void</SPAN> Main()
        {
            <SPAN class="KEYWORD">using</SPAN> (DbManager db = <SPAN class="KEYWORD">new</SPAN> DbManager(<SPAN class="STRING">"Development"</SPAN>))
            {
                <SPAN class="COMMENT">// ...</SPAN>
            }
        }
    }
}</font></PRE></TD></TR></TABLE></p>

<LI>Adding a data provider - Borland Data Providers for .NET (BDP.NET)

<p><TABLE class="code" width="98%"><TR><TD><PRE><font size=2><SPAN class="KEYWORD">using</SPAN> System;
<SPAN class="KEYWORD">using</SPAN> System.Data;
<SPAN class="KEYWORD">using</SPAN> System.Data.Common;

<SPAN class="KEYWORD">using</SPAN> Borland.Data.Provider;

<SPAN class="KEYWORD">using</SPAN> Rsdn.Framework.Data;
<SPAN class="KEYWORD">using</SPAN> Rsdn.Framework.Data.DataProvider;

<SPAN class="KEYWORD">namespace</SPAN> Example
{
    <SPAN class="KEYWORD">public</SPAN> <SPAN class="KEYWORD">class</SPAN> BdpDataProvider: IDataProvider
    {
        IDbConnection IDataProvider.CreateConnectionObject()
        {
            <SPAN class="KEYWORD">return</SPAN> <SPAN class="KEYWORD">new</SPAN> BdpConnection();
        }

        DbDataAdapter IDataProvider.CreateDataAdapterObject()
        {
            <SPAN class="KEYWORD">return</SPAN> <SPAN class="KEYWORD">new</SPAN> BdpDataAdapter();
        }

        <SPAN class="KEYWORD">void</SPAN> IDataProvider.DeriveParameters(IDbCommand command)
        {
            BdpCommandBuilder.DeriveParameters((BdpCommand)command);
        }

        Type IDataProvider.ConnectionType
        {
            get { <SPAN class="KEYWORD">return</SPAN> <SPAN class="KEYWORD">typeof</SPAN>(BdpConnection); }
        }

        <SPAN class="KEYWORD">string</SPAN> IDataProvider.Name
        {
            get { <SPAN class="KEYWORD">return</SPAN> <SPAN class="STRING">"Bdp"</SPAN>; }
        }
    }

    <SPAN class="KEYWORD">class</SPAN> Test
    {
        <SPAN class="KEYWORD">static</SPAN> <SPAN class="KEYWORD">void</SPAN> Main()
        {
            DbManager.AddDataProvider(<SPAN class="KEYWORD">new</SPAN> BdpDataProvider());
            DbManager.AddConnectionString(<SPAN class="STRING">".bdp"</SPAN>,
                <SPAN class="STRING">"assembly=Borland.Data.Mssql,"</SPAN> + 
                <SPAN class="STRING">"Version=1.1.0.0,Culture=neutral, "</SPAN> +                 
                <SPAN class="STRING">"PublicKeyToken=91d62ebb5b0d1b1b;"</SPAN> +
                <SPAN class="STRING">"vendorclient=sqloledb.dll;osauthentication=True;"</SPAN> +
                <SPAN class="STRING">"database=Northwind;hostname=localhost;provider=MSSQL"</SPAN>);

            <SPAN class="KEYWORD">using</SPAN> (DbManager db = <SPAN class="KEYWORD">new</SPAN> DbManager())
            {
                <SPAN class="KEYWORD">int</SPAN> count = (<SPAN class="KEYWORD">int</SPAN>)db.ExecuteScalar(<SPAN class="STRING">"SELECT Count(*) FROM Categories"</SPAN>);

                Console.WriteLine(count);
            }
        }
    }
}</font></PRE></TD></TR></TABLE></p>

<LI>Business object reading

<p><TABLE class="code" width="98%"><TR><TD><PRE><font size=2><SPAN class="KEYWORD">using</SPAN> System;

<SPAN class="KEYWORD">using</SPAN> Rsdn.Framework.Data;
<SPAN class="KEYWORD">using</SPAN> Rsdn.Framework.Data.Mapping;

<SPAN class="KEYWORD">namespace</SPAN> Example
{
    <SPAN class="KEYWORD">public</SPAN> <SPAN class="KEYWORD">class</SPAN> Category
    {
        [MapField(Name=<SPAN class="STRING">"CategoryID"</SPAN>)]
        <SPAN class="KEYWORD">public</SPAN> <SPAN class="KEYWORD">int</SPAN> ID;

        [MapField(Name=<SPAN class="STRING">"CategoryName"</SPAN>)]
        <SPAN class="KEYWORD">public</SPAN> <SPAN class="KEYWORD">string</SPAN> Name;

        [MapField(IsNullable=<SPAN class="KEYWORD">true</SPAN>)]
        <SPAN class="KEYWORD">public</SPAN> <SPAN class="KEYWORD">string</SPAN> Description;
    }

    <SPAN class="KEYWORD">class</SPAN> Test
    {
        <SPAN class="KEYWORD">static</SPAN> <SPAN class="KEYWORD">void</SPAN> Main()
        {
            <SPAN class="KEYWORD">using</SPAN> (DbManager db = <SPAN class="KEYWORD">new</SPAN> DbManager())
            {
                Category category = db.ExecuteBizEntity(
                    <SPAN class="KEYWORD">typeof</SPAN>(Category), <SPAN class="STRING">@"
</SPAN>                    <SPAN class="STRING">SELECT TOP 1
</SPAN>                        <SPAN class="STRING">CategoryID,
</SPAN>                        <SPAN class="STRING">CategoryName,
</SPAN>                        <SPAN class="STRING">Description
</SPAN>                    <SPAN class="STRING">FROM Categories"</SPAN>);

                <SPAN class="KEYWORD">if</SPAN> (category != <SPAN class="KEYWORD">null</SPAN>)
                {
                    Console.WriteLine(<SPAN class="STRING">"ID  : {0}\nName: {1}\nDesc: {2}"</SPAN>,
                        category.ID, category.Name, category.Description);
                }
            }
        }
    }
}</font></PRE></TD></TR></TABLE></p>

<LI>Stored procedure use

<p><TABLE class="code" width="98%"><TR><TD><PRE><font size=2><SPAN class="KEYWORD">using</SPAN> System;
<SPAN class="KEYWORD">using</SPAN> System.Data;

<SPAN class="KEYWORD">using</SPAN> Rsdn.Framework.Data;

<SPAN class="KEYWORD">namespace</SPAN> Example
{
    <SPAN class="KEYWORD">class</SPAN> Test
    {
        <SPAN class="KEYWORD">static</SPAN> <SPAN class="KEYWORD">void</SPAN> Main()
        {
            <SPAN class="KEYWORD">using</SPAN> (DbManager db = <SPAN class="KEYWORD">new</SPAN> DbManager())
            {
                DataSet dataSet = db.ExecuteSpDataSet(<SPAN class="STRING">"SalesByCategory"</SPAN>, <SPAN class="STRING">"Seafood"</SPAN>, <SPAN class="KEYWORD">null</SPAN>);
            }
        }
    }
}</font></PRE></TD></TR></TABLE></p>

<LI>Mapping an array of the object to the DataTable

<p><TABLE class="code" width="98%"><TR><TD><PRE><font size=2><SPAN class="KEYWORD">using</SPAN> System;
<SPAN class="KEYWORD">using</SPAN> System.Collections;
<SPAN class="KEYWORD">using</SPAN> System.Data;

<SPAN class="KEYWORD">using</SPAN> Rsdn.Framework.Data;

<SPAN class="KEYWORD">namespace</SPAN> Example
{
    <SPAN class="KEYWORD">public</SPAN> <SPAN class="KEYWORD">class</SPAN> BizEntity
    {
        <SPAN class="KEYWORD">public</SPAN> <SPAN class="KEYWORD">int</SPAN>    ID;
        <SPAN class="KEYWORD">public</SPAN> <SPAN class="KEYWORD">string</SPAN> Name;
    }

    <SPAN class="KEYWORD">class</SPAN> Test
    {
        <SPAN class="KEYWORD">static</SPAN> <SPAN class="KEYWORD">void</SPAN> Main()
        {
            DataTable table = <SPAN class="KEYWORD">new</SPAN> DataTable();

            table.Columns.Add(<SPAN class="STRING">"ID"</SPAN>,   <SPAN class="KEYWORD">typeof</SPAN>(<SPAN class="KEYWORD">int</SPAN>));
            table.Columns.Add(<SPAN class="STRING">"Name"</SPAN>, <SPAN class="KEYWORD">typeof</SPAN>(<SPAN class="KEYWORD">string</SPAN>));

            ArrayList array  = <SPAN class="KEYWORD">new</SPAN> ArrayList();
            BizEntity entity = <SPAN class="KEYWORD">new</SPAN> BizEntity();

            entity.ID   = 1;
            entity.Name = <SPAN class="STRING">"Example"</SPAN>;

            array.Add(entity);

            MapData.MapList(array, table);

            Console.WriteLine(<SPAN class="STRING">"ID  : {0}"</SPAN>, table.Rows[0][<SPAN class="STRING">"ID"</SPAN>]);
            Console.WriteLine(<SPAN class="STRING">"Name: {0}"</SPAN>, table.Rows[0][<SPAN class="STRING">"Name"</SPAN>]);
        }
    }
}</font></PRE></TD></TR></TABLE></p>

<LI>… and back

<p><TABLE class="code" width="98%"><TR><TD><PRE><font size=2><SPAN class="KEYWORD">using</SPAN> System;
<SPAN class="KEYWORD">using</SPAN> System.Collections;
<SPAN class="KEYWORD">using</SPAN> System.Data;

<SPAN class="KEYWORD">using</SPAN> Rsdn.Framework.Data;

<SPAN class="KEYWORD">namespace</SPAN> Example
{
    <SPAN class="KEYWORD">public</SPAN> <SPAN class="KEYWORD">class</SPAN> BizEntity
    {
        <SPAN class="KEYWORD">public</SPAN> <SPAN class="KEYWORD">int</SPAN>    ID;
        <SPAN class="KEYWORD">public</SPAN> <SPAN class="KEYWORD">string</SPAN> Name;
    }

    <SPAN class="KEYWORD">class</SPAN> Test
    {
        <SPAN class="KEYWORD">static</SPAN> <SPAN class="KEYWORD">void</SPAN> Main()
        {
            DataTable table = <SPAN class="KEYWORD">new</SPAN> DataTable();

            table.Columns.Add(<SPAN class="STRING">"ID"</SPAN>,   <SPAN class="KEYWORD">typeof</SPAN>(<SPAN class="KEYWORD">int</SPAN>));
            table.Columns.Add(<SPAN class="STRING">"Name"</SPAN>, <SPAN class="KEYWORD">typeof</SPAN>(<SPAN class="KEYWORD">string</SPAN>));

            table.Rows.Add(<SPAN class="KEYWORD">new</SPAN> <SPAN class="KEYWORD">object</SPAN>[] { 1, <SPAN class="STRING">"Example"</SPAN> });

            ArrayList array = MapData.MapList(table, <SPAN class="KEYWORD">typeof</SPAN>(BizEntity));

            Console.WriteLine(<SPAN class="STRING">"ID  : {0}"</SPAN>, ((BizEntity)array[0]).ID);
            Console.WriteLine(<SPAN class="STRING">"Name: {0}"</SPAN>, ((BizEntity)array[0]).Name);
        }
    }
}</font></PRE></TD></TR></TABLE></p>
</li></ol>

<!------ Article Ends ----->
</font>
</TD></TR>
			<TR>
				<TD class="ftr" colspan="2"><uc1:Footer runat="server" id=Footer1></uc1:Footer></TD>
			</TR></TABLE>
	</body>
</HTML>