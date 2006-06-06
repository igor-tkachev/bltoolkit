using System;

namespace BLToolkit.Common
{
	/// <summary>
	/// This argument adapter class allows either names (strings) or
	/// indices (ints) to be passed to a function.
	/// </summary>
	[System.Diagnostics.DebuggerStepThrough]
	public struct NameOrIndexParameter
	{
		public NameOrIndexParameter(string name)
		{
			if (null == name)
				throw new ArgumentNullException("name");

			if (name.Length == 0)
				throw new ArgumentException("Name must be a valid string.", "name");
			
			_name  = name;
			_index = 0;
		}

		public NameOrIndexParameter(int index)
		{
			if (index < 0)
				throw new ArgumentException(
					"The index parameter must be greater or equal to zero.", "index");

			_name  = null;
			_index = index;
		}

		public static implicit operator NameOrIndexParameter(string name)
		{
			return new NameOrIndexParameter(name);
		}

		public static implicit operator NameOrIndexParameter(int index)
		{
			return new NameOrIndexParameter(index);
		}

		#region Public properties
		
		public bool ByName
		{
			get { return null != _name; }
		}

		private readonly string _name;
		public           string  Name
		{
			get
			{
				if (null == _name)
					throw new InvalidOperationException(
						"This instance was initialized by index");
				
				 return _name;
			}
		}

		private readonly int _index;
		public           int  Index
		{
			get
			{
				if (null != _name)
					throw new InvalidOperationException(
						"This instance was initialized by name");

				return _index;
			}
		}

		#endregion

		#region Static methods

		public static NameOrIndexParameter[] FromStringArray(string[] names)
		{
#if FW2
			return Array.ConvertAll<string, NameOrIndexParameter>(names,
						delegate(string name) { return new NameOrIndexParameter(name); });
#else
			NameOrIndexParameter[] nips = new NameOrIndexParameter[names.Length];
			for (int i = 0; i < nips.Length; ++i)
			{
				nips[i] = new NameOrIndexParameter(names[i]);
			}
			return nips;
#endif
		}

		public static NameOrIndexParameter[] FromIndexArray(int[] indices)
		{
#if FW2
			return Array.ConvertAll<int, NameOrIndexParameter>(indices,
						delegate(int index) { return new NameOrIndexParameter(index); });
#else
			NameOrIndexParameter[] nips =  new NameOrIndexParameter[indices.Length];
			for (int i = 0; i < nips.Length; ++i)
			{
				nips[i] = new NameOrIndexParameter(indices[i]);
			}
			return nips;
#endif
		}

		#endregion
		
		#region System.Object members

		public override bool Equals(object obj)
		{
			if (obj is NameOrIndexParameter)
			{
				NameOrIndexParameter nip = (NameOrIndexParameter)obj;
				if (null != _name && null != nip._name && _name == nip._name)
					return true; // Same name
				
				if (null == _name && null == nip._name && _index == nip._index)
					return true; // Same index

				return false;
			}

			if (obj is string)
			{
				string name = (string)obj;
				return (null != _name && _name == name);
			}

			if (obj is int)
			{
				int index = (int)obj;
				return (null == _name && _index == index);
			}
			
			//return base.Equals(obj);
			return false;
		}

		public override int GetHashCode()
		{
			return (null != _name) ? _name.GetHashCode() : _index.GetHashCode();
		}
		
		public override string ToString()
		{
			return (null != _name) ? _name : "#" + _index;
		}

		#endregion

	}
}
