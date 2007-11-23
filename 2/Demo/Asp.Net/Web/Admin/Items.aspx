<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="Items.aspx.cs" Inherits="Admin_Items" Title="Items" %>
<%@ Register Assembly="BLToolkit.3" Namespace="BLToolkit.Web.UI" TagPrefix="cc1" %>

<asp:Content ID="Content" ContentPlaceHolderID="cph" Runat="Server">

<asp:GridView ID="grid" runat="server"
	AutoGenerateColumns = "False"
	DataSourceID        = "binder"
	CssClass            = "grid"
	AllowSorting        = "True">
<Columns>
	<asp:BoundField DataField="ID"          HeaderText="Item ID"      SortExpression="ID" />
	<asp:BoundField DataField="Name"        HeaderText="Name"         SortExpression="Name" />
	<asp:BoundField DataField="Price"       HeaderText="Price"        SortExpression="Price"    ItemStyle-HorizontalAlign="Right" />
	<asp:BoundField DataField="UnitCost"    HeaderText="Unit Cost"    SortExpression="UnitCost" ItemStyle-HorizontalAlign="Right" />
	<asp:BoundField DataField="Quantity"    HeaderText="Quantity"     SortExpression="Quantity" ItemStyle-HorizontalAlign="Right" />
	<asp:BoundField DataField="ProductName" HeaderText="Product Name" SortExpression="ProductName" />
	<asp:BoundField DataField="CategoryID"  HeaderText="Category"     SortExpression="CategoryID" />
</Columns>
</asp:GridView>

<cc1:WebObjectBinder ID="binder" runat="server" TypeName="PetShop.ObjectModel.Item" />

</asp:Content>
