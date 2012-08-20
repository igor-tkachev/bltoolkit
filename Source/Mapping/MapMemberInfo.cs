using System;
using System.Data;
using System.Diagnostics;
using BLToolkit.DataAccess;
using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Mapping
{
	[DebuggerStepThrough]
	public class MapMemberInfo
	{
		public MapMemberInfo()
		{
			DbType = DbType.Object;
		}

		public MemberAccessor  MemberAccessor             { get; set; }
		public MemberAccessor  ComplexMemberAccessor      { get; set; }
		public string          Name                       { get; set; }
		public string          MemberName                 { get; set; }
		public string          Storage                    { get; set; }
		public bool            IsInheritanceDiscriminator { get; set; }
		public bool            Trimmable                  { get; set; }
		public bool            SqlIgnore                  { get; set; }
		public bool            Nullable                   { get; set; }
		public object          NullValue                  { get; set; }
		public object          DefaultValue               { get; set; }
		public Type            Type                       { get; set; }
		public int             DbSize                     { get; set; }
		public bool            IsDbTypeSet                { get; set; }
		public bool            IsDbSizeSet                { get; set; }
		public MappingSchema   MappingSchema              { get; set; }
		public MapValue[]      MapValues                  { get; set; }
		public MemberExtension MemberExtension            { get; set; }
		public DbType          DbType                     { get; set; }

        /// <summary>
        /// TODO : Create PrimaryKeyMapMemberInfo?
        /// </summary>
        public KeyGenerator KeyGenerator { get; set; }
	}
}
