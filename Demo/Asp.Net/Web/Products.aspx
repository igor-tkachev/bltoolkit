<%@ Page AutoEventWireup="true" Language="C#" MasterPageFile="~/MasterPage.master" Title="Products" Inherits="PetShop.Web.Products" CodeFile="~/Products.aspx.cs" %>
<%@ Register Src="Controls/ProductsControl.ascx" TagName="ProductsControl" TagPrefix="PetShopControl" %>

<asp:Content ID="cntPage" ContentPlaceHolderID="cphPage" runat="Server" EnableViewState="false">
	<PetShopControl:ProductsControl runat="server" />
</asp:Content>
