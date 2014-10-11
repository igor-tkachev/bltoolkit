<%@ Page AutoEventWireup="true" Language="C#" MasterPageFile="~/MasterPage.master" Title="Search" %>
<%@ Register Src="Controls/SearchControl.ascx" TagName="SearchControl" TagPrefix="uc1" %>
<%@ Register TagPrefix="PetShopControl" Namespace="PetShop.Web" %> 

<asp:Content ID="cntPage" ContentPlaceHolderID="cphPage" runat="Server" EnableViewState="false">   
	<uc1:SearchControl runat="server" />
</asp:Content>
