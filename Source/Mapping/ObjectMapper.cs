using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class ObjectMapper : IObjectMapper
	{
		#region Constructor

		public ObjectMapper()
		{
			_members      = new ArrayList();
			_nameToMember = new Hashtable();
		}

		#endregion

		#region Protected Members

		protected virtual MemberMapper CreateMemberMapper(MapMemberInfo mapMemberInfo)
		{
			MemberMapper mm = MemberMapper.CreateMemberMapper(mapMemberInfo);

			mm.Init(mapMemberInfo);

			return mm;
		}

		[SuppressMessage("Microsoft.Performance", "CA1807:AvoidUnnecessaryStringCreation", MessageId = "stack1")]
		protected void Add(MemberMapper memberMapper)
		{
			if (memberMapper == null) throw new ArgumentNullException("memberMapper");

			memberMapper.SetOrdinal(_members.Count);

			_members.     Add(memberMapper);
			_nameToMember.Add(memberMapper.Name.ToLower(),  memberMapper);
		}

		private   ArrayList _members;
		protected MemberMapper this[int index]
		{
			get { return (MemberMapper)_members[index]; }
		}

		private   Hashtable _nameToMember;
		protected MemberMapper this[string name]
		{
			get
			{
				if (name == null) throw new ArgumentNullException("name");

				MemberMapper mm = (MemberMapper)_nameToMember[name];

				if (mm == null)
				{
					mm = (MemberMapper)_nameToMember[name.ToLower(CultureInfo.CurrentCulture)];

					if (mm != null)
						_nameToMember[name] = mm;
				}

				return mm;
			}
		}

		private   TypeAccessor _typeAccessor;
		protected TypeAccessor  TypeAccessor
		{
			get { return _typeAccessor;  }
			//set { _typeAccessor = value; }
		}

		#endregion

		#region IObjectMappper Members

		public virtual object CreateInstance()
		{
			return _typeAccessor.CreateInstance();
		}

		public virtual object CreateInstance(InitContext context)
		{
			return _typeAccessor.CreateInstance(context);
		}

		public virtual void Init(Mapper mapper, TypeAccessor typeAccessor)
		{
			if (typeAccessor == null) throw new ArgumentNullException("typeAccessor");

			_typeAccessor = typeAccessor;

			foreach (MemberAccessor ma in typeAccessor)
			{
				if (GetIgnore(ma))
					continue;

				MapMemberInfo mi = new MapMemberInfo();

				mi.MemberAccessor = ma;
				mi.Name           = ma.Name;
				mi.Mapper         = mapper;
				mi.IsTrimmable    = GetIsTrimmable(ma);

				Add(CreateMemberMapper(mi));
			}
		}

		protected virtual bool GetIgnore(MemberAccessor memberAccessor)
		{
			MapIgnoreAttribute attr = 
				(MapIgnoreAttribute)memberAccessor.GetAttribute(typeof(MapIgnoreAttribute));

			if (attr != null)
				return attr.Ignore;

			Type type = memberAccessor.Type;

			return type.IsClass && type != typeof(string);
		}

		protected virtual bool GetIsTrimmable(MemberAccessor memberAccessor)
		{
			TrimmableAttribute attr1 = 
				(TrimmableAttribute)memberAccessor.GetAttribute(typeof(TrimmableAttribute));

			if (attr1 != null)
				return attr1.IsTrimmable;

			TrimStringsAttribute attr2 = (TrimStringsAttribute)TypeHelper.GetFirstAttribute(
				memberAccessor.MemberInfo.DeclaringType, typeof(TrimStringsAttribute));

			if (attr2 != null)
				return attr2.IsTrimmable;

			return true;
		}

		#endregion

		#region IMapDataSource Members

		public virtual int Count
		{
			get { return _members.Count; }
		}

		public virtual string GetName(int index)
		{
			return ((MemberMapper)_members[index]).Name;
		}

		public virtual object GetValue(object o, int index)
		{
			return ((MemberMapper)_members[index]).GetValue(o);
		}

		public virtual object GetValue(object o, string name)
		{
			MemberMapper mm = (MemberMapper)_nameToMember[name];

			if (mm == null)
				mm = this[name];

			return mm == null? null: mm.GetValue(o);
		}

		#endregion

		#region IMapDataDestination Members

		public virtual int GetOrdinal(string name)
		{
			MemberMapper mm = (MemberMapper)_nameToMember[name];

			if (mm == null)
				mm = this[name];

			return mm == null? -1: mm.Ordinal;
		}

		public virtual void SetValue(object o, int index, object value)
		{
			((MemberMapper)_members[index]).SetValue(o, value);
		}

		public virtual void SetValue(object o, string name, object value)
		{
			SetValue(o, GetOrdinal(name), value);
		}

		#endregion
	}
}
