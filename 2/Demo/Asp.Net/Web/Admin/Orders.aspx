<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="Orders.aspx.cs" Inherits="Admin_Orders" Title="Orders" %>
<%@ Register Assembly="BLToolkit.2" Namespace="BLToolkit.Web.UI" TagPrefix="cc1" %>
<%@ Import Namespace="PetShop.ObjectModel" %>

<asp:Content ID="Content" ContentPlaceHolderID="cph" Runat="Server">

<asp:GridView ID="grid" runat="server"
	AutoGenerateColumns = "False"
	DataSourceID        = "binder"
	CssClass            = "grid"
	AllowSorting        = "True">
<Columns>
	<asp:BoundField DataField="ID"         HeaderText="Order ID"   SortExpression="ID"         ItemStyle-HorizontalAlign="Right" />
	<asp:BoundField DataField="UserID"     HeaderText="User ID"    SortExpression="UserID" />
	<asp:TemplateField HeaderText="Ship To">
		<ItemTemplate><%# FormatAddress(((Order)Container.DataItem).ShippingAddress) %>
		</ItemTemplate>
	</asp:TemplateField>
	<asp:BoundField DataField="OrderDate"  HeaderText="Date"       SortExpression="OrderDate" DataFormatString="{0:MM/dd/yy hh:mm tt}" HtmlEncode="false" />
	<asp:BoundField DataField="Courier"    HeaderText="Courier"    SortExpression="Courier" />
	<asp:BoundField DataField="TotalPrice" HeaderText="TotalPrice" SortExpression="TotalPrice" ItemStyle-HorizontalAlign="Right" />
	<asp:BoundField DataField="Status"     HeaderText="Status"     SortExpression="Status" />
</Columns>
</asp:GridView>

<cc1:WebObjectBinder ID="binder" runat="server" TypeName="PetShop.ObjectModel.Order" />

</asp:Content>
