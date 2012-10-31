using System;
using System.Data;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Method)]
	public class CommandBehaviorAttribute : Attribute
	{
		public CommandBehaviorAttribute()
		{
			_commandBehavior = CommandBehavior.Default;
		}

		public CommandBehaviorAttribute(CommandBehavior commandBehavior)
		{
			_commandBehavior = commandBehavior;
		}

		private CommandBehavior _commandBehavior;
		public  CommandBehavior  CommandBehavior
		{
			get { return _commandBehavior;  }
			set { _commandBehavior = value; }
		}
	}
}
