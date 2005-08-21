using System;

namespace Rsdn.Framework.DataAccess
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class SprocNameAttribute : Attribute
	{
		public SprocNameAttribute(string actionName, string procedureName)
		{
			_actionName    = actionName;
			_procedureName = procedureName;
		}

		private string _actionName;
		public  string  ActionName
		{
			get { return _actionName; }
		}

		private string _procedureName;
		public  string  ProcedureName
		{
			get { return _procedureName; }
		}
	}
}
