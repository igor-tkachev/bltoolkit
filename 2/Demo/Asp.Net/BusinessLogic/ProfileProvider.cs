using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Profile;

namespace PetShop.BusinessLogic
{
	using DataAccess;
	using ObjectModel;

	public sealed class ProfileProvider : System.Web.Profile.ProfileProvider
	{
		private static string _applicationName = ".NET Pet Shop 4.0";

		/// <summary>
		/// The name of the application using the custom profile provider.
		/// </summary>
		public override string ApplicationName
		{
			get { return _applicationName;  }
			set { _applicationName = value; }
		}

		/// <summary>
		/// Initializes the provider.
		/// </summary>
		/// <param name="name">The friendly name of the provider.</param>
		/// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
		public override void Initialize(string name, NameValueCollection config)
		{
			if (config == null)
				throw new ArgumentNullException("config");

			if (string.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", "Pet Shop Custom Profile Provider");
			}

			if (string.IsNullOrEmpty(name))
				name = "PetShopProfileProvider";

			if (config["applicationName"] != null && !string.IsNullOrEmpty(config["applicationName"].Trim()))
				_applicationName = config["applicationName"];

			base.Initialize(name, config);
		}

		private const string ERR_INVALID_PARAMETER = "Invalid Profile parameter:";
		private const string PROFILE_SHOPPINGCART  = "ShoppingCart";
		private const string PROFILE_WISHLIST      = "WishList";
		private const string PROFILE_ACCOUNT       = "AccountInfo";

		/// <summary>
		/// Returns the collection of settings property values for the specified application instance and settings property group.
		/// </summary>
		/// <param name="context">A System.Configuration.SettingsContext describing the current application use.</param>
		/// <param name="collection">A System.Configuration.SettingsPropertyCollection containing the settings property group whose values are to be retrieved.</param>
		/// <returns>A System.Configuration.SettingsPropertyValueCollection containing the values for the specified settings property group.</returns>
		public override SettingsPropertyValueCollection GetPropertyValues(
			SettingsContext context, SettingsPropertyCollection collection)
		{
			string username        = (string)context["UserName"];
			bool   isAuthenticated = (bool)  context["IsAuthenticated"];

			SettingsPropertyValueCollection svc = new SettingsPropertyValueCollection();

			foreach (SettingsProperty prop in collection)
			{
				SettingsPropertyValue pv = new SettingsPropertyValue(prop);

				switch (pv.Property.Name)
				{
					case PROFILE_SHOPPINGCART: pv.PropertyValue = GetCartItems(username, true);  break;
					case PROFILE_WISHLIST:     pv.PropertyValue = GetCartItems(username, false); break;
					case PROFILE_ACCOUNT:
						if (isAuthenticated)
							pv.PropertyValue = GetAccountInfo(username);
						break;

					default:
						throw new ApplicationException(ERR_INVALID_PARAMETER + " name.");
				}

				svc.Add(pv);
			}

			return svc;
		}

		/// <summary>
		/// Sets the values of the specified group of property settings.
		/// </summary>
		/// <param name="context">A System.Configuration.SettingsContext describing the current application usage.</param>
		/// <param name="collection">A System.Configuration.SettingsPropertyValueCollection representing the group of property settings to set.</param>
		public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
		{
			string username = (string)context["UserName"];

			CheckUserName(username);

			bool isAuthenticated = (bool)context["IsAuthenticated"];
			int  uniqueID        = GetUniqueID(username, isAuthenticated, false, ApplicationName);

			foreach (SettingsPropertyValue pv in collection)
			{
				if (pv.PropertyValue != null)
				{
					switch (pv.Property.Name)
					{
						case PROFILE_SHOPPINGCART: SetCartItems(uniqueID, (Cart)pv.PropertyValue, true);  break;
						case PROFILE_WISHLIST:     SetCartItems(uniqueID, (Cart)pv.PropertyValue, false); break;
						case PROFILE_ACCOUNT:
							if (isAuthenticated)
								SetAccountInfo(uniqueID, (Address)pv.PropertyValue);
							break;

						default:
							throw new ApplicationException(ERR_INVALID_PARAMETER + " name.");
					}
				}
			}

			UpdateActivityDates(username, false);
		}

		private int GetUniqueID(string userName, bool isAuthenticated, bool ignoreAuthenticationType, string appName)
		{
			int? uniqueID = ignoreAuthenticationType ?
				Accessor.GetUniqueID    (userName, appName) :
				Accessor.GetUniqueIDAuth(userName, appName, isAuthenticated);

			return uniqueID ?? Accessor.CreateProfile(userName, appName, isAuthenticated);
		}

		private Address GetAccountInfo(string username)
		{
			return Accessor.GetAccountInfo(username, _applicationName);
		}

		private Cart GetCartItems(string username, bool isShoppingCart)
		{
			Cart cart = new Cart();

			foreach (CartItem cartItem in Accessor.GetCartItems(username, _applicationName, isShoppingCart))
				cart.Add(cartItem);

			return cart;
		}

		private void SetAccountInfo(int uniqueID, Address address)
		{
			Accessor.SetAccountInfo(uniqueID, address);
		}

		private void SetCartItems(int uniqueID, Cart cart, bool isShoppingCart)
		{
			Accessor.SetCartItems(uniqueID, cart.Items, isShoppingCart);
		}

		// UpdateActivityDates
		// Updates the LastActivityDate and LastUpdatedDate values 
		// when profile properties are accessed by the
		// GetPropertyValues and SetPropertyValues methods. 
		// Passing true as the activityOnly parameter will update
		// only the LastActivityDate.
		private void UpdateActivityDates(string username, bool activityOnly)
		{
			if (activityOnly)
				Accessor.UpdateActivityDate(username, _applicationName);
			else
				Accessor.UpdateActivityAndUdpateDates(username, _applicationName);
		}

		/// <summary>
		/// Deletes profile properties and information for the supplied list of profiles.
		/// </summary>
		/// <param name="profiles">A System.Web.Profile.ProfileInfoCollection of information about profiles that are to be deleted.</param>
		/// <returns>The number of profiles deleted from the data source.</returns>
		public override int DeleteProfiles(ProfileInfoCollection profiles)
		{
			int deleteCount = 0;

			foreach (ProfileInfo p in profiles)
				if (DeleteProfile(p.UserName))
					deleteCount++;

			return deleteCount;
		}

		/// <summary>
		/// Deletes profile properties and information for profiles that match the supplied list of user names.
		/// </summary>
		/// <param name="usernames">A string array of user names for profiles to be deleted.</param>
		/// <returns>The number of profiles deleted from the data source.</returns>
		public override int DeleteProfiles(string[] usernames)
		{
			int deleteCount = 0;

			foreach (string user in usernames)
				if (DeleteProfile(user))
					deleteCount++;

			return deleteCount;
		}

		// DeleteProfile
		// Deletes profile data from the database for the specified user name.
		private bool DeleteProfile(string username)
		{
			CheckUserName(username);

			int? uniqueID = Accessor.GetUniqueID(username, _applicationName);

			return uniqueID != null && Accessor.DeleteProfile(uniqueID.Value) > 0;
		}

		// Verifies user name for sise and comma
		private static void CheckUserName(string userName)
		{
			if (string.IsNullOrEmpty(userName) || userName.Length > 256 || userName.IndexOf(",") > 0)
				throw new ApplicationException(ERR_INVALID_PARAMETER + " user name.");
		}

		/// <summary>
		/// Deletes all user-profile data for profiles in which the last activity date occurred before the specified date.
		/// </summary>
		/// <param name="authenticationOption">One of the System.Web.Profile.ProfileAuthenticationOption values, specifying whether anonymous, authenticated, or both types of profiles are deleted.</param>
		/// <param name="userInactiveSinceDate">A System.DateTime that identifies which user profiles are considered inactive. If the System.Web.Profile.ProfileInfo.LastActivityDate value of a user profile occurs on or before this date and time, the profile is considered inactive.</param>
		/// <returns>The number of profiles deleted from the data source.</returns>
		public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
		{
			IList<string> list;

			switch (authenticationOption)
			{
				case ProfileAuthenticationOption.Anonymous:
					list = Accessor.GetInactiveProfiles(userInactiveSinceDate, ApplicationName, true);
					break;

				case ProfileAuthenticationOption.Authenticated:
					list = Accessor.GetInactiveProfiles(userInactiveSinceDate, ApplicationName, false);
					break;

				default:
					list = Accessor.GetInactiveProfiles(userInactiveSinceDate, ApplicationName);
					break;
			}

			string[] userArray = new string[list.Count];

			list.CopyTo(userArray, 0);

			return DeleteProfiles(userArray);
		}

		/// <summary>
		/// Retrieves profile information for profiles in which the user name matches the specified user names.
		/// </summary>
		/// <param name="authenticationOption">One of the System.Web.Profile.ProfileAuthenticationOption values, specifying whether anonymous, authenticated, or both types of profiles are returned.</param>
		/// <param name="usernameToMatch">The user name to search for.</param>
		/// <param name="pageIndex">The index of the page of results to return.</param>
		/// <param name="pageSize">The size of the page of results to return.</param>
		/// <param name="totalRecords">When this method returns, contains the total number of profiles.</param>
		/// <returns>A System.Web.Profile.ProfileInfoCollection containing user-profile information
		///     for profiles where the user name matches the supplied usernameToMatch parameter.</returns>
		public override ProfileInfoCollection FindProfilesByUserName(
			ProfileAuthenticationOption authenticationOption, string usernameToMatch,
			int pageIndex, int pageSize, out int totalRecords)
		{
			CheckParameters(pageIndex, pageSize);

			return GetProfileInfo(authenticationOption, usernameToMatch, null, pageIndex, pageSize, out totalRecords);
		}

		/// <summary>
		/// Retrieves profile information for profiles in which the last activity date occurred on or before the specified date and the user name matches the specified user name.
		/// </summary>
		/// <param name="authenticationOption">One of the System.Web.Profile.ProfileAuthenticationOption values, specifying whether anonymous, authenticated, or both types of profiles are returned.</param>
		/// <param name="usernameToMatch">The user name to search for.</param>
		/// <param name="userInactiveSinceDate">A System.DateTime that identifies which user profiles are considered inactive. If the System.Web.Profile.ProfileInfo.LastActivityDate value of a user profile occurs on or before this date and time, the profile is considered inactive.</param>
		/// <param name="pageIndex">The index of the page of results to return.</param>
		/// <param name="pageSize">The size of the page of results to return.</param>
		/// <param name="totalRecords">When this method returns, contains the total number of profiles.</param>
		/// <returns>A System.Web.Profile.ProfileInfoCollection containing user profile information for inactive profiles where the user name matches the supplied usernameToMatch parameter.</returns>	
		public override ProfileInfoCollection FindInactiveProfilesByUserName(
			ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate,
			int pageIndex, int pageSize, out int totalRecords)
		{
			CheckParameters(pageIndex, pageSize);

			return GetProfileInfo(authenticationOption, usernameToMatch, userInactiveSinceDate, pageIndex, pageSize, out totalRecords);
		}

		/// <summary>
		/// Retrieves user profile data for all profiles in the data source.
		/// </summary>
		/// <param name="authenticationOption">One of the System.Web.Profile.ProfileAuthenticationOption values, specifying whether anonymous, authenticated, or both types of profiles are returned.</param>
		/// <param name="pageIndex">The index of the page of results to return.</param>
		/// <param name="pageSize">The size of the page of results to return.</param>
		/// <param name="totalRecords">When this method returns, contains the total number of profiles.</param>
		/// <returns>A System.Web.Profile.ProfileInfoCollection containing user-profile information for all profiles in the data source.</returns>		
		public override ProfileInfoCollection GetAllProfiles(
			ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
		{
			CheckParameters(pageIndex, pageSize);

			return GetProfileInfo(authenticationOption, null, null, pageIndex, pageSize, out totalRecords);
		}

		/// <summary>
		/// Retrieves user-profile data from the data source for profiles in which the last activity date occurred on or before the specified date.
		/// </summary>
		/// <param name="authenticationOption">One of the System.Web.Profile.ProfileAuthenticationOption values, specifying whether anonymous, authenticated, or both types of profiles are returned.</param>
		/// <param name="userInactiveSinceDate">A System.DateTime that identifies which user profiles are considered inactive. If the System.Web.Profile.ProfileInfo.LastActivityDate of a user profile occurs on or before this date and time, the profile is considered inactive.</param>
		/// <param name="pageIndex">The index of the page of results to return.</param>
		/// <param name="pageSize">The size of the page of results to return.</param>
		/// <param name="totalRecords">When this method returns, contains the total number of profiles.</param>
		/// <returns>A System.Web.Profile.ProfileInfoCollection containing user-profile information about the inactive profiles.</returns>
		public override ProfileInfoCollection GetAllInactiveProfiles(
			ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate,
			int pageIndex, int pageSize, out int totalRecords)
		{
			CheckParameters(pageIndex, pageSize);

			return GetProfileInfo(authenticationOption, null, userInactiveSinceDate, pageIndex, pageSize, out totalRecords);
		}

		/// <summary>
		/// Returns the number of profiles in which the last activity date occurred on or before the specified date.
		/// </summary>
		/// <param name="authenticationOption">One of the System.Web.Profile.ProfileAuthenticationOption values, specifying whether anonymous, authenticated, or both types of profiles are returned.</param>
		/// <param name="userInactiveSinceDate">A System.DateTime that identifies which user profiles are considered inactive. If the System.Web.Profile.ProfileInfo.LastActivityDate of a user profile occurs on or before this date and time, the profile is considered inactive.</param>
		/// <returns>The number of profiles in which the last activity date occurred on or before the specified date.</returns>
		public override int GetNumberOfInactiveProfiles(
			ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
		{
			int inactiveProfiles = 0;

			ProfileInfoCollection profiles = GetProfileInfo(authenticationOption, null, userInactiveSinceDate, 0, 0, out inactiveProfiles);

			return inactiveProfiles;
		}

		private static void CheckParameters(int pageIndex, int pageSize)
		{
			if (pageIndex < 1 || pageSize < 1)
				throw new ApplicationException(ERR_INVALID_PARAMETER + " page index.");
		}

		// GetProfileInfo
		// Retrieves a count of profiles and creates a 
		// ProfileInfoCollection from the profile data in the 
		// database. Called by GetAllProfiles, GetAllInactiveProfiles,
		// FindProfilesByUserName, FindInactiveProfilesByUserName, 
		// and GetNumberOfInactiveProfiles.
		// Specifying a pageIndex of 0 retrieves a count of the results only.
		private ProfileInfoCollection GetProfileInfo(
			ProfileAuthenticationOption authenticationOption,
			string    usernameToMatch,
			DateTime? userInactiveSinceDate,
			int       pageIndex,
			int       pageSize,
			out int   totalRecords)
		{
			totalRecords = 0;

			ProfileInfoCollection profiles = new ProfileInfoCollection();

			// Count profiles only.
			if (pageSize == 0)
				return profiles;

			int counter    = 0;
			int startIndex = pageSize * (pageIndex - 1);
			int endIndex   = startIndex + pageSize - 1;

			bool? isAnonymous = null;

			if (authenticationOption == ProfileAuthenticationOption.Anonymous)
				isAnonymous = true;
			else if (authenticationOption == ProfileAuthenticationOption.Authenticated)
				isAnonymous = false;

			foreach (CustomProfile profile in Accessor.GetProfile(
				isAnonymous, usernameToMatch, userInactiveSinceDate, _applicationName, out totalRecords))
			{
				if (counter >= startIndex)
				{
					ProfileInfo p = new ProfileInfo(
						profile.UserName,
						profile.IsAnonymous ?? true,
						profile.LastActivityDate ?? DateTime.MinValue,
						profile.LastUpdatedDate  ?? DateTime.MinValue, 0);

					profiles.Add(p);
				}

				if (counter >= endIndex)
					break;

				counter++;
			}

			return profiles;
		}

		#region Accessor

		ProfileAccessor Accessor
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return ProfileAccessor.CreateInstance(); }
		}

		#endregion
	}
}
