using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Parameter), CLSCompliant(false)]
	public abstract class Direction : Attribute
	{
		protected string[] _members;
		public    string[]  Members
		{
			get { return _members;  }
			set { _members = value; }
		}

		public class OutputAttribute : Direction
		{
			public OutputAttribute(params string[] members)
			{
				_members = members;
			}
		}

		public class InputOutputAttribute : Direction
		{
			public InputOutputAttribute(params string[] members)
			{
				_members = members;
			}
		}

		public class IgnoreAttribute : Direction
		{
			public IgnoreAttribute(params string[] members)
			{
				_members = members;
			}
		}

		public class ReturnValueAttribute : Direction
		{
			protected string _member;
			public    string  Member
			{
				get { return _member;  }
				set { _member = value; }
			}

			public ReturnValueAttribute(string member)
			{
				_member = member;
			}
		}
	}
}
