using System;

namespace BLToolkit.Common
{
	using Mapping;

	public static class Configuration
	{
		static Configuration()
		{
			NotifyOnEqualSet                      = true;
			TrimDictionaryKey                     = true;
			CheckNullReturnIfNull                 = NullEquivalent.DBNull;
			OpenNewConnectionToDiscoverParameters = true;
		}

		public enum NullEquivalent { DBNull, Null, Value }

		/// <summary>
		/// Specifies what value should be returned by <c>TypeAccessor.CheckNull</c>
		/// if <see cref="BLToolkit.Reflection.IsNullHandler"/> was specified and interpreted current property 
		/// value as null. Default is: <see cref="DBNull"/>.
		/// </summary>
		public static NullEquivalent CheckNullReturnIfNull { get; set; }

		/// <summary>
		/// Controls global trimming behaviour of mapper. Specifies whether trailing spaces
		/// should be trimmed when mapping from one entity to another. Default is: false. 
		/// To specify trimming behaviour other than global, please user <see cref="TrimmableAttribute"/>.
		/// </summary>
		public static bool TrimOnMapping { get; set; }

		/// <summary>
		/// Controls global trimming behaviour of mapper for dictionary keys. Specifies whether trailing spaces
		/// should be trimmed when adding keys to dictionaries. Default is: true. 
		/// </summary>
		public static bool TrimDictionaryKey { get; set; }

		/// <summary>
		/// Specifies default behavior for PropertyChange generation. If set to true, <see cref="BLToolkit.EditableObjects.EditableObject.OnPropertyChanged"/>
		/// is invoked even when current value is same as new one. If set to false,  <see cref="BLToolkit.EditableObjects.EditableObject.OnPropertyChanged"/> 
		/// is invoked only when new value is being assigned. To specify notification behaviour other than default, please see 
		/// <see cref="BLToolkit.TypeBuilder.PropertyChangedAttribute"/>
		/// </summary>
		public static bool NotifyOnEqualSet { get; set; }

		/// <summary>
		/// Controls whether attributes specified on base types should be always added to list of attributes
		/// when scanning hierarchy tree or they should be compared to attributes found on derived classes
		/// and added only when not present already. Default value: false;
		/// WARNING: setting this flag to "true" can significantly affect initial object generation/access performance
		/// use only when side effects are noticed with attribute being present on derived and base classes. 
		/// For builder attributes use provided attribute compatibility mechanism.
		/// </summary>
		public static bool FilterOutBaseEqualAttributes { get; set; }

		/// <summary>
		/// Controls whether attributes specified on base types should be always added to list of attributes
		/// when scanning hierarchy tree or they should be compared to attributes found on derived classes
		/// and added only when not present already. Default value: false;
		/// WARNING: setting this flag to "true" can significantly affect initial object generation/access performance
		/// use only when side effects are noticed with attribute being present on derived and base classes. 
		/// For builder attributes use provided attribute compatibility mechanism.
		/// </summary>
		public static bool OpenNewConnectionToDiscoverParameters { get; set; }

		public static class ExpressionMapper
		{
			public static bool IncludeComplexMapping { get; set; }
		}

		public static class Linq
		{
			static Linq()
			{
				ClassTypeParameterCanAlwaysBeNull = true;
			}

			public static bool PreloadGroups                     { get; set; }
			public static bool IgnoreEmptyUpdate                 { get; set; }
			public static bool AllowMultipleQuery                { get; set; }
			public static bool GenerateExpressionTest            { get; set; }
			public static bool ClassTypeParameterCanAlwaysBeNull { get; set; }
		}

		public static class NullableValues
		{
			public static Int32          Int32          = 0;
			public static Double         Double         = 0;
			public static Int16          Int16          = 0;
			public static Boolean        Boolean        = false;
			[CLSCompliant(false)]
			public static SByte          SByte          = 0;
			public static Int64          Int64          = 0;
			public static Byte           Byte           = 0;
			[CLSCompliant(false)]
			public static UInt16         UInt16         = 0;
			[CLSCompliant(false)]
			public static UInt32         UInt32         = 0;
			[CLSCompliant(false)]
			public static UInt64         UInt64         = 0;
			public static Single         Single         = 0;
			public static Char           Char           = '\x0';
			public static DateTime       DateTime       = DateTime.MinValue;
			public static TimeSpan       TimeSpan       = TimeSpan.MinValue;
			public static DateTimeOffset DateTimeOffset = DateTimeOffset.MinValue;
			public static Decimal        Decimal        = 0m;
			public static Guid           Guid           = Guid.Empty;
			public static String         String         = string.Empty;
		}
	}
}
