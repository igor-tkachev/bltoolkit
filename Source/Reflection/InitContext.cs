using System;
using System.Collections;
using System.Diagnostics;

using BLToolkit.Mapping;

namespace BLToolkit.Reflection
{
	public class InitContext
	{
		public object[]       MemberParameters { get; set; }
		public object[]       Parameters       { get; set; }
		public bool           IsInternal       { get; set; }
		public bool           IsLazyInstance   { get; set; }
		public object         Parent           { get; set; }
		public object         SourceObject     { get; set; }
		public ObjectMapper   ObjectMapper     { get; set; }
		public MappingSchema  MappingSchema    { get; set; }
		public bool           IsSource         { get; set; }
		public bool           StopMapping      { get; set; }
		[CLSCompliant(false)]
		public IMapDataSource DataSource       { get; set; }

		private Hashtable _items;
		public  Hashtable  Items
		{
			[DebuggerStepThrough] 
			get 
			{
				if (_items == null)
					_items = new Hashtable();

				return _items;
			}
		}

		public  bool  IsDestination
		{
			[DebuggerStepThrough] get { return !IsSource;  }
			[DebuggerStepThrough] set { IsSource = !value; }
		}
	}
}
