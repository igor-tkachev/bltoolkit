using System;
using System.Collections;

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

		private   ArrayList _members;
		protected ArrayList  Members
		{
			get { return _members; }
		}

		private   Hashtable _nameToMember;
		protected Hashtable  NameToMember
		{
			get { return _nameToMember; }
		}

		private   TypeAccessor _typeAccessor;
		protected TypeAccessor  TypeAccessor
		{
			get { return _typeAccessor; }
		}

		internal void SetTypeAccessor(TypeAccessor typeAccessor)
		{
			_typeAccessor = typeAccessor;
		}

		#endregion

		#region IObjectMapper Members

		public virtual object CreateInstance()
		{
			return _typeAccessor.CreateInstance();
		}

		public virtual object CreateInstance(BLToolkit.Reflection.InitContext context)
		{
			return _typeAccessor.CreateInstance(context);
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
