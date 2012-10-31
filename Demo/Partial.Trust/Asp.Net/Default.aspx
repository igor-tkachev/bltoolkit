<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Partial.Trust.Asp.Net._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
	<title>Partial Trust Demo</title>
</head>
<body style="font-family: Verdana; font-size: small">
<form id="form1" runat="server">

<div style="padding: 10px">
To use BLToolkit in Partial Trust Environment you should perform the following steps:

	<div style="padding-left: 20px; padding-top: 10px;">
	For all assemblies containing classes for which BLToolkit generates new types such as Partial.Trust.Components.dll in this demo:

		<div style="padding-left: 20px; padding-top: 10px;">
		<ul>
		<li>
			Sign the assembly.
		</li>
		<li style="padding-top: 10px">
			Add the AllowPartiallyTrustedCallers attribute:
			<pre style="font-size: small">[assembly: AllowPartiallyTrustedCallers]</pre>
		</li>

		<li>
			Use BLTgen.exe to generate BLToolkit extensions at the post-build step. For example:<br/><br/>

			$(ProjectDir)..\..\..\Tools\BLTgen\bin\$(ConfigurationName)\BLTgen.4.exe $(TargetPath) /O:$(ProjectDir)..\Asp.Net\bin /K:$(ProjectDir)Partial.Trust.snk /D<br/><br/>

			Extension assembly must be signed as well (use /K flag).
		</li>
		</ul>
		</div>

	Turn the TypeFactory.LoadTypes flag on.

		<div style="padding-left: 20px; padding-top: 10px;">
		Add the following section in the Web.config file:

<pre style="font-size: small; padding-left: 20px;">
&lt;configSections&gt;
	&lt;section name="bltoolkit" type="BLToolkit.Configuration.BLToolkitSection, BLToolkit.4" requirePermission="false"/&gt;
&lt;/configSections&gt;
&lt;bltoolkit&gt;
	&lt;typeFactory loadTypes="true" /&gt;
&lt;/bltoolkit&gt;
</pre>

			- or<br/><br/>

			set 

				<pre style="font-size: small; padding-left: 20px;">TypeFactory.LoadTypes = true;</pre>

			somewhere before the first use of BLToolkit (Global.asax for Web applications).

		</div>
	</div>

<br/>
<br/>

Sample output:
<br/>
<br/>

<div style="padding-left: 20px;">
<table>
<tr><td>DataAccessor:</td>       <td><asp:Label ID="Label1" runat="server" ></asp:Label></td></tr>
<tr><td>Linq query:</td>         <td><asp:Label ID="Label2" runat="server" ></asp:Label></td></tr>
<tr><td>Compiled Linq query:</td><td><asp:Label ID="Label3" runat="server" ></asp:Label></td></tr>
</table>
</div>

</div>
</form>
</body>
</html>
