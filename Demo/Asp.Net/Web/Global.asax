<%@ Application Language="C#" %>
<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="PetShop.ObjectModel" %>

<script RunAt="server">
	
// Carry over profile property values from an anonymous to an authenticated state.
//
void Profile_MigrateAnonymous(object sender, ProfileMigrateEventArgs e)
{
	ProfileCommon anonProfile = Profile.GetProfile(e.AnonymousID);

	// Merge anonymous shopping cart items to the authenticated shopping cart items.
	//
	foreach (CartItem cartItem in anonProfile.ShoppingCart.Items)
		Profile.ShoppingCart.Add(cartItem);

	// Merge anonymous wishlist items to the authenticated wishlist items.
	//
	foreach (CartItem cartItem in anonProfile.WishList.Items)
		Profile.WishList.Add(cartItem);

	// Clean up anonymous profile.
	//
	ProfileManager.DeleteProfile(e.AnonymousID);
	AnonymousIdentificationModule.ClearAnonymousIdentifier();
    
	// Save profile.
	//
	Profile.Save();
}

private static string LOG_SOURCE = ConfigurationManager.AppSettings["Event Log Source"];

// If an exception is thrown in the application then log it to an event log.
//
protected void Application_Error(object sender, EventArgs e)
{
	Exception x = Server.GetLastError().GetBaseException();
	
	EventLog.WriteEntry(LOG_SOURCE, x.ToString(), EventLogEntryType.Error);
}

</script>
