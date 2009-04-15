using System;
using System.Collections.Generic;

namespace BLToolkit.Data.Sql
{
	public class ChildContainer<P,C> : Dictionary<string,C>, IDictionary<string,C>
		where C : IChild<P>
		where P : class
	{
		internal ChildContainer(P parent)
		{
			_parent = parent;
		}

		readonly P _parent;
		public   P  Parent { get { return _parent; } }

		public void Add(C item)
		{
			Add(item.Name, item);
		}

		public new void Add(string key, C value)
		{
			if (value.Parent != null) throw new InvalidOperationException("Invalid parent.");
			value.Parent = _parent;

			base.Add(key, value);
		}

		public void AddRange(IEnumerable<C> collection)
		{
			foreach (C item in collection)
				Add(item);
		}
	}
}
