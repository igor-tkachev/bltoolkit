using System;

namespace BLToolkit.Common
{
	/// <summary>
	/// This argument adapter class allows either names (strings) or
	/// indices (ints) to be passed to a function.
	/// </summary>
	public struct NameOrIndexParameter
	{
		public static implicit operator NameOrIndexParameter(string name)
		{
			if (null == name)
				throw new ArgumentNullException("name");
			
			NameOrIndexParameter p = new NameOrIndexParameter();
			p._name = name;
			return p;
		}

		public static implicit operator NameOrIndexParameter(int index)
		{
			if (index < 0)
				throw new ArgumentException(
					"The index parameter must be greater or equal to zero.",
					"index");

			NameOrIndexParameter p = new NameOrIndexParameter();
			p._index = index;
			return p;
		}
		
		public bool ByName
		{
			get { return null != _name; }
		}

		public string Name
		{
			get
			{
				if (null == _name)
					throw new InvalidOperationException(
						"This instance was initialized by index");
				
				 return _name;
			}
		}
		
		public int Index
		{
			get
			{
				if (null != _name)
					throw new InvalidOperationException(
						"This instance was initialized by name");

				return _index;
			}
		}
		
		private string _name;
		private int _index;
	}
}
