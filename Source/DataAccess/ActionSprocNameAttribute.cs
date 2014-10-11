using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ActionSprocNameAttribute : Attribute
	{
		public ActionSprocNameAttribute(string actionName, string procedureName)
		{
			_actionName    = actionName;
			_procedureName = procedureName;
		}

		private readonly string _actionName;
		public           string  ActionName
		{
			get { return _actionName; }
		}

		private readonly string _procedureName;
		public           string  ProcedureName
		{
			get { return _procedureName; }
		}
	}
}
