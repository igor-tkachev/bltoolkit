<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShoppingCart.aspx.cs" Inherits="ShoppingCart" Title="Shopping Cart" %>
<%@ Register Src="Controls/ShoppingCartControl.ascx" TagName="ShoppingCartControl" TagPrefix="PetShopControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" Runat="Server">
<div align="center" class="cartPosition">   
<PetShopControl:shoppingcartcontrol id="ShoppingCartControl1" runat="server" />
<table border="0" cellpadding="0" cellspacing="0" align="center" width="387">
<tr><td class="linkCheckOut" colspan="2">&nbsp;</td></tr>
<tr>
	<td nowrap="nowrap" align="right" width="100%"><a href="javascript:history.back();"><img border="0" src="Comm_Images/button-home.gif"></a></td>
	<td align="left" nowrap="nowrap"><a class="linkCheckOut" href="javascript:history.back();">Continue Shopping</a></td>
</tr>
<tr><td class="linkCheckOut" colspan="2">&nbsp;</td></tr>
<tr>
	<td align="right" nowrap="nowrap"><a href="CheckOut.aspx"><img border="0" src="Comm_Images/button-checkout.gif"></a></td>
	<td nowrap="nowrap" align="left"><a class="linkCheckOut" href="CheckOut.aspx">Check Out</a></td>
</tr>
</table>
</div>
</asp:Content>

