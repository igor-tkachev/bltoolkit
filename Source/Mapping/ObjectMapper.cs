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

		protected virtual MemberMapper CreateMemberMapper(string name, MemberAccessor ma)
		{
			MemberMapper mm = MemberMapper.CreateMemberMapper(ma.Type);

			mm.Init(name, ma);

			return mm;
		}

		[SuppressMessage("Microsoft.Performance", "CA1807:AvoidUnnecessaryStringCreation", MessageId = "stack1")]
		protected void Add(MemberMapper memberMapper)
		{
			if (memberMapper == null) throw new ArgumentNullException("memberMapper");

			memberMapper.SetOrdinal(_members.Count);

			_members.     Add(memberMapper);
			_nameToMember.Add(memberMapper.Name.ToLower(CultureInfo.CurrentCulture),  memberMapper);
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

		public virtual void Init(TypeAccessor typeAccessor)
		{
			if (typeAccessor == null) throw new ArgumentNullException("typeAccessor");

			_typeAccessor = typeAccessor;

			foreach (MemberAccessor ma in typeAccessor)
			{
				MemberMapper mm = CreateMemberMapper(ma.Name, ma);
				Add(mm);
			}
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
			return ((MemberMapper)_nameToMember[name]).GetValue(o);
		}

		#endregion

		#region IMapDataDestination Members

		public virtual int GetOrdinal(string name)
		{
			return ((MemberMapper)_nameToMember[name]).Ordinal;
		}

		public virtual void SetValue(object o, int index, object value)
		{
			((MemberMapper)_members[index]).SetValue(o, value);
		}

		#endregion
	}
}
