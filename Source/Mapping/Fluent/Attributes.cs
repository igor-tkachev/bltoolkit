namespace BLToolkit.Mapping.Fluent
{
    /// <summary>
    /// Used Attribute Names
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class FluentMap<T>
    {
        protected static class Attributes
        {
            public static class TableName
            {
                public const string Name = "TableName";

                public const string Database = "DatabaseName";

                public const string Owner = "OwnerName";
            }

            public static class MapField
            {
                public const string Name = "MapField";

                public const string Storage = "FieldStorage";

                public const string IsInheritanceDiscriminator = "IsInheritanceDiscriminator";

                public const string OrigName = "OrigName";

                public const string MapName = "MapName";
            }

            public static class PrimaryKey
            {
                public const string Order = "PrimaryKey";
            }

            public static class SqlIgnore
            {
                public const string Ignore = "SqlIgnore";
            }

            public static class MapIgnore
            {
                public const string Ignore = "MapIgnore";
            }

            public static class MapValue
            {
                public const string Name = "MapValue";

                public const string OrigValue = "OrigValue";
            }

            public static class Nullable
            {
                public const string IsNullable = "Nullable";
            }

            public static class LazyInstance
            {
                public const string IsLazyInstance = "LazyInstance";
            }

            public static class InheritanceMapping
            {
                public const string Name = "InheritanceMapping";

                public const string Code = "Code";

                public const string IsDefault = "IsDefault";

                public const string Type = "Type";
            }

            public static class Association
            {
                public const string ThisKey = "ThisKey";

                public const string OtherKey = "OtherKey";

                public const string Storage = "Storage";
            }

            public const string NonUpdatable = "NonUpdatable";

            public const string Identity = "Identity";

            public const string Trimmable = "Trimmable";

            public const string DefaultValue = "DefaultValue";

            public const string DbType = "DbType";

            public static class MemberMapper
            {
                public const string Name = "MemberMapper";

                public const string MemberType = "MemberType";

                public const string MemberMapperType = "MemberMapperType";
            }

            public const string NullValue = "NullValue";
        }
    }
}