//---------------------------------------------------------------------------------------------------
// This code was generated by BLToolkit.
//---------------------------------------------------------------------------------------------------
using System;
using System.Linq;
using System.Linq.Expressions;
using BLToolkit.Data.Linq;
using LightStation.API;
using LightStation.Storage.Relational;
using NUnit.Framework;

namespace LightStation.API
{
	public interface IStationObjectID
	{
	}
}

namespace LightStation.API
{
	[SerializableAttribute()]
	public class MachineIdentifier : LightStation.API.StationObject, LightStation.Utils.Validation.IValidatable
	{
		public LightStation.API.StationObjectID OrganizationId { get; set; }

		public string Value { get; set; }

		public LightStation.API.StationObjectID SymbolId { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime? EndDate { get; set; }
	}

	[SerializableAttribute()]
	public struct NullableStationObjectID : IStationObjectID
	{
		public NullableStationObjectID(int? value)
		{
			throw new NotImplementedException();
		}

		public int? Value { get; set; }
	}

	[System.SerializableAttribute()]
	public abstract class StationObject : Utils.Validation.IValidatable
	{
		public StationObjectID Id { get; set; }
	}

	[System.SerializableAttribute()]
	public struct StationObjectID : IStationObjectID
	{
		public StationObjectID(int value)
		{
		}

		public int Value { get; set; }
	}

	[Serializable]
	public class Symbol : LightStation.API.StationObject, LightStation.Utils.Validation.IValidatable
	{
		public string UniqueId { get; set; }

		public string TypeName { get; set; }

		public bool IsActive { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public string Description { get; set; }

		public string Extra { get; set; }

		public LightStation.API.NullableStationObjectID DataProvenanceId { get; set; }

		public LightStation.API.NullableStationObjectID CountryId { get; set; }

		public LightStation.API.NullableStationObjectID CurrencyId { get; set; }

		public LightStation.API.NullableStationObjectID DataProviderAccountId { get; set; }

		public LightStation.API.NullableStationObjectID PrimaryExchangeId { get; set; }

		public LightStation.API.NullableStationObjectID CompositeExchangeId { get; set; }

		public string Name { get; set; }

		public string PrimaryTicker { get; set; }

		public DateTime AsOfDate { get; set; }
	}
}

namespace LightStation.Storage.Relational
{
	//[System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	//class <>c__DisplayClass3
	//{
	//	public LightStation.Storage.Relational.IGenericQueryDataSource ds;
	//
	//	public Nullable<DateTime> asOfDate;
	//}

	public interface IGenericQueryDataSource
	{
		System.Linq.IQueryable<LightStation.API.MachineIdentifier> MachineIdentifiers { get; set; }
	}

	public interface IHasDbId<I>
	{
	}

	[BLToolkit.DataAccess.TableNameAttribute(Name = "ls_exchange")]
	public class ls_exchange : LightStation.Storage.Relational.IHasDbId<int>
	{
		public int organization_id { get; set; }
	}

	[BLToolkit.DataAccess.TableNameAttribute(Name = "ls_symbol")]
	public class ls_symbol : LightStation.Storage.Relational.IHasDbId<int>
	{
		[BLToolkit.DataAccess.PrimaryKeyAttribute((Int32)1)]
		[BLToolkit.DataAccess.IdentityAttribute()]
		public int id { get; set; }

		public string unique_id { get; set; }

		public bool is_active { get; set; }

		[BLToolkit.Mapping.NullableAttribute()]
		public string type_name { get; set; }

		public DateTime start_date { get; set; }

		[BLToolkit.Mapping.NullableAttribute()]
		public Nullable<DateTime> end_date { get; set; }

		[BLToolkit.Mapping.NullableAttribute()]
		public Nullable<int> data_provenance_id { get; set; }

		[BLToolkit.Mapping.NullableAttribute()]
		public Nullable<int> country_id { get; set; }

		[BLToolkit.Mapping.NullableAttribute()]
		public Nullable<int> currency_id { get; set; }

		[BLToolkit.Mapping.NullableAttribute()]
		public Nullable<int> data_provider_account_id { get; set; }

		[BLToolkit.Mapping.NullableAttribute()]
		public Nullable<int> primary_exchange_id { get; set; }

		[BLToolkit.Mapping.NullableAttribute()]
		public Nullable<int> composite_exchange_id { get; set; }

		[BLToolkit.Mapping.NullableAttribute()]
		public string description { get; set; }

		[BLToolkit.Mapping.NullableAttribute()]
		public string extra { get; set; }

		[BLToolkit.Mapping.AssociationAttribute(ThisKey = "primary_exchange_id", OtherKey = "id", CanBeNull = true)]
		public LightStation.Storage.Relational.ls_exchange symbolexchangeprimary { get; set; }
	}
}

namespace LightStation.Storage.Relational.Tests.IRelationalStorageTests
{
	//[System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	//class <>c__DisplayClassa5
	//{
	//	public LightStation.API.Symbol symbol;
	//}
}

namespace LightStation.Utils.Validation
{
	public interface IValidatable
	{
	}
}

namespace Data.Linq
{
	[TestFixture]
	public class UserTest : TestBase
	{
		[Test]
		public void Test()
		{
			// Table(ls_symbol).Select(symbol => new <>f__AnonymousType1`2(symbol = symbol, ids = value(LightStation.Storage.Relational.RelationalDbToBusinessObjectConverterExtensions+<>c__DisplayClass3).ds.MachineIdentifiers.Where(identifier => (((Convert(identifier.StartDate) <= value(LightStation.Storage.Relational.RelationalDbToBusinessObjectConverterExtensions+<>c__DisplayClass3).asOfDate) AndAlso ((identifier.EndDate == Convert(null)) OrElse (identifier.EndDate >= value(LightStation.Storage.Relational.RelationalDbToBusinessObjectConverterExtensions+<>c__DisplayClass3).asOfDate))) AndAlso (identifier.SymbolId.Value == symbol.id))))).Select(<>h__TransparentIdentifier0 => new <>f__AnonymousType2`2(<>h__TransparentIdentifier0 = <>h__TransparentIdentifier0, name = <>h__TransparentIdentifier0.ids.Where(x => (Convert(x.OrganizationId) == Convert(Organization.SelfId))).Select(x => x.Value).FirstOrDefault())).Select(<>h__TransparentIdentifier1 => new <>f__AnonymousType3`2(<>h__TransparentIdentifier1 = <>h__TransparentIdentifier1, primaryTicker = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.ids.Where(x => (Convert(x.OrganizationId) == <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.symbol.symbolexchangeprimary.organization_id)).Select(x => x.Value).FirstOrDefault())).Select(<>h__TransparentIdentifier2 => new Symbol() {Id = new StationObjectID() {Value = <>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.symbol.id}, UniqueId = <>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.symbol.unique_id, TypeName = <>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.symbol.type_name, IsActive = <>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.symbol.is_active, StartDate = <>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.symbol.start_date, EndDate = <>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.symbol.end_date, Description = <>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.symbol.description, Extra = <>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.symbol.extra, DataProvenanceId = new NullableStationObjectID() {Value = <>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.symbol.data_provenance_id}, CountryId = new NullableStationObjectID() {Value = <>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.symbol.country_id}, CurrencyId = new NullableStationObjectID() {Value = <>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.symbol.currency_id}, DataProviderAccountId = new NullableStationObjectID() {Value = <>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.symbol.data_provider_account_id}, PrimaryExchangeId = new NullableStationObjectID() {Value = <>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.symbol.primary_exchange_id}, CompositeExchangeId = new NullableStationObjectID() {Value = <>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.symbol.composite_exchange_id}, Name = <>h__TransparentIdentifier2.<>h__TransparentIdentifier1.name, PrimaryTicker = <>h__TransparentIdentifier2.primaryTicker, AsOfDate = value(LightStation.Storage.Relational.RelationalDbToBusinessObjectConverterExtensions+<>c__DisplayClass3).asOfDate.Value}).First(x => (Convert(x.Id) == Convert(value(LightStation.Storage.Relational.Tests.IRelationalStorageTests.SymbolTests+<>c__DisplayClassa5).symbol.Id)))
			ForEachProvider(db =>
				db.GetTable<ls_symbol>()
					.Select(
						// Unknown expression.
						symbol => new { symbol = symbol, ids = value(LightStation.Storage.Relational.RelationalDbToBusinessObjectConverterExtensions+<>c__DisplayClass3).ds.MachineIdentifiers.Where(identifier => (((Convert(identifier.StartDate) <= value(LightStation.Storage.Relational.RelationalDbToBusinessObjectConverterExtensions+<>c__DisplayClass3).asOfDate) AndAlso ((identifier.EndDate == Convert(null)) OrElse (identifier.EndDate >= value(LightStation.Storage.Relational.RelationalDbToBusinessObjectConverterExtensions+<>c__DisplayClass3).asOfDate))) AndAlso (identifier.SymbolId.Value == symbol.id)))))
					.Select(
						// Unknown expression.
						tp0 => new { tp0 = tp0, name = tp0.ids.Where(x => (Convert(x.OrganizationId) == Convert(Organization.SelfId))).Select(x => x.Value).FirstOrDefault()) }
					.}Select(
						// Unknown expression.
						tp1 => new <>f__AnonymousType3`2(tp1 = tp1, primaryTicker = tp1.tp0.ids.Where(x => (Convert(x.OrganizationId) == tp1.tp0.symbol.symbolexchangeprimary.organization_id)).Select(x => x.Value).FirstOrDefault()))
					.Select(
						// Unknown expression.
						tp2 => new Symbol() {Id = new StationObjectID() {Value = tp2.tp1.tp0.symbol.id}, UniqueId = tp2.tp1.tp0.symbol.unique_id, TypeName = tp2.tp1.tp0.symbol.type_name, IsActive = tp2.tp1.tp0.symbol.is_active, StartDate = tp2.tp1.tp0.symbol.start_date, EndDate = tp2.tp1.tp0.symbol.end_date, Description = tp2.tp1.tp0.symbol.description, Extra = tp2.tp1.tp0.symbol.extra, DataProvenanceId = new NullableStationObjectID() {Value = tp2.tp1.tp0.symbol.data_provenance_id}, CountryId = new NullableStationObjectID() {Value = tp2.tp1.tp0.symbol.country_id}, CurrencyId = new NullableStationObjectID() {Value = tp2.tp1.tp0.symbol.currency_id}, DataProviderAccountId = new NullableStationObjectID() {Value = tp2.tp1.tp0.symbol.data_provider_account_id}, PrimaryExchangeId = new NullableStationObjectID() {Value = tp2.tp1.tp0.symbol.primary_exchange_id}, CompositeExchangeId = new NullableStationObjectID() {Value = tp2.tp1.tp0.symbol.composite_exchange_id}, Name = tp2.tp1.name, PrimaryTicker = tp2.primaryTicker, AsOfDate = value(LightStation.Storage.Relational.RelationalDbToBusinessObjectConverterExtensions+<>c__DisplayClass3).asOfDate.Value})
					.First<LightStation.API.Symbol>(
						// Unknown expression.
						x => (Convert(x.Id) == Convert(value(LightStation.Storage.Relational.Tests.IRelationalStorageTests.SymbolTests+<>c__DisplayClassa5).symbol.Id))));
		}
	}
}

