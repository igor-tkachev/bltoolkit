using System;
using System.Collections.Generic;

using BLToolkit.Data;
using BLToolkit.DataAccess;

namespace PetShop.BusinessLogic.DataAccess
{
	using ObjectModel;

	public abstract class ProfileAccessor : AccessorBase<ProfileAccessor.DB, ProfileAccessor>
	{
		public class DB : DbManager { public DB() : base("ProfileDB") {} }

		[SqlQuery(@"
			SELECT
				UniqueID
			FROM
				Profiles
			WHERE
				Username = @userName AND ApplicationName = @appName")]
		public abstract int? GetUniqueID(string @userName, string @appName);

		[SqlQuery(@"
			SELECT
				UniqueID
			FROM
				Profiles
			WHERE
				Username = @userName AND ApplicationName = @appName AND IsAnonymous != @isAuthenticated")]
		public abstract int? GetUniqueIDAuth(string @userName, string @appName, bool @isAuthenticated);

		[SqlQuery(@"
			INSERT INTO Profiles (
				Username,  ApplicationName, LastActivityDate, LastUpdatedDate, IsAnonymous
			) Values (
				@userName, @appName, getdate(), getdate(), CASE WHEN @isAuthenticated = 1 THEN 0 ELSE 1 END
			)

			SELECT SCOPE_IDENTITY()")]
		public abstract int CreateProfile(string @userName, string @appName, bool @isAuthenticated);

		[SqlQuery(@"
			SELECT
				a.FirstName as ToFirstName,
				a.LastName  as ToLastName,
				a.Address1  as Addr1,
				a.Address2  as Addr2,
				a.City,
				a.State,
				a.Zip,
				a.Country,
				a.Email,
				a.Phone
			FROM
				Account a
					JOIN Profiles p ON p.UniqueID = a.UniqueID
			WHERE
				p.Username = @userName AND p.ApplicationName = @appName;")]
		public abstract Address GetAccountInfo(string @userName, string @appName);

		[SqlQuery(@"
			SELECT
				c.ItemId,
				c.Name,
				c.Type,
				c.Price,
				c.CategoryId,
				c.ProductId,
				c.Quantity
			FROM
				Profiles p
					JOIN Cart c ON c.UniqueID = p.UniqueID
			WHERE
				p.Username        = @userName AND
				p.ApplicationName = @appName AND
				c.IsShoppingCart  = @isShoppingCart")]
		public abstract IList<CartItem> GetCartItems(string @userName, string @appName, bool @isShoppingCart);

		[SqlQuery(@"
			DELETE FROM Account WHERE UniqueID = @uniqueID

			INSERT INTO Account (
				UniqueID, Email, FirstName, LastName, Address1, Address2, City, State, Zip, Country, Phone
			) VALUES (
				@uniqueID, @Email, @ToFirstName, @ToLastName, @Addr1, @Addr2, @City, @State, @Zip, @Country, @Phone
			)")]
		public abstract void SetAccountInfo(int @uniqueID, Address address);

		// This method is not abstract as BLToolkit does not generate methods for the ExecuteForEach method.
		// It's virtual as we want to get statistic info for this method.
		// Counter and Log aspects wrap all abstract, virtual, and override members.
		//
		public virtual void SetCartItems(int uniqueID, ICollection<CartItem> cartItems, bool isShoppingCart)
		{
			using (DbManager db = GetDbManager())
			{
				db.BeginTransaction();

				db
					.SetCommand(@"
						DELETE FROM
							Cart
						WHERE
							UniqueID = @uniqueID AND IsShoppingCart = @isShoppingCart",
						db.Parameter("@uniqueID",       uniqueID),
						db.Parameter("@isShoppingCart", isShoppingCart))
					.ExecuteNonQuery();

				if (cartItems.Count > 0)
				{
					db
						.SetCommand(@"
							INSERT INTO Cart (
								UniqueID, ItemId, Name, Type, Price, CategoryId, ProductId, IsShoppingCart, Quantity
							) VALUES (
								@uniqueID, @ItemId, @Name, @Type, @Price, @CategoryId, @ProductId, @isShoppingCart, @Quantity
							)",
							db.CreateParameters(typeof(CartItem),
							db.Parameter("@uniqueID",       uniqueID),
							db.Parameter("@isShoppingCart", isShoppingCart)))
						.ExecuteForEach(cartItems);
				}

				db.CommitTransaction();
			}
		}

		[SqlQuery(@"
			UPDATE
				Profiles
			SET
				LastActivityDate = getdate()
			WHERE
				Username = @userName AND ApplicationName = @appName")]
		public abstract void UpdateActivityDate(string @userName, string @appName);

		[SqlQuery(@"
			UPDATE
				Profiles
			SET
				LastActivityDate = getdate(),
				LastUpdatedDate  = getdate()
			WHERE
				Username = @userName AND ApplicationName = @appName")]
		public abstract void UpdateActivityAndUdpateDates(string @userName, string @appName);

		[SqlQuery(@"DELETE FROM Profiles WHERE UniqueID = @uniqueID")]
		[ScalarSource(ScalarSourceType.AffectedRows)]
		public abstract int DeleteProfile(int @uniqueID);

		[SqlQuery(@"
			SELECT
				Username
			FROM
				Profiles
			WHERE ApplicationName = @appName AND LastActivityDate <= @userInactiveSinceDate")]
		public abstract IList<string> GetInactiveProfiles(DateTime userInactiveSinceDate, string appName);

		[SqlQuery(@"
			SELECT
				Username
			FROM
				Profiles
			WHERE ApplicationName = @appName AND LastActivityDate <= @userInactiveSinceDate AND IsAnonymous = @isAnonymous")]
		public abstract IList<string> GetInactiveProfiles(DateTime @userInactiveSinceDate, string @appName, bool @isAnonymous);

		const string _profileQuery = @"
			FROM
				Profiles
			WHERE
				ApplicationName = @appName AND
				(@isAnonymous           IS NULL OR IsAnonymous = @isAnonymous) AND
				(@userName              IS NULL OR Username LIKE @userName)    AND
				(@userInactiveSinceDate IS NULL OR LastActivityDate >= @userInactiveSinceDate)";

		[SqlQuery(@"
			SELECT @totalRecords = Count(*)" + _profileQuery + @"
			SELECT *" + _profileQuery)]
		public abstract IList<CustomProfile> GetProfile(
			bool? @isAnonymous, string @userName, DateTime? @userInactiveSinceDate, string @appName, out int @totalRecords);
	}
}
