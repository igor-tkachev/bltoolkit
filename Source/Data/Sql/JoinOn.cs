using System;

using BLToolkit.Reflection.Extension;

namespace BLToolkit.Data.Sql
{
	public class JoinOn
	{
		public JoinOn()
		{
		}

		public JoinOn(string field, string otherField)
		{
			_field      = field;
			_otherField = otherField;
		}

		public JoinOn(string field, string otherField, string expression)
		{
			_field      = field;
			_otherField = otherField;
			_expression = expression;
		}

		public JoinOn(AttributeExtension ext)
		{
			_field      = (string)ext["Field"];
			_otherField = (string)ext["OtherField"];
			_expression = (string)ext["Expression"];
		}

		private string _field;
		public  string  Field { get { return _field; } set { _field = value; } }

		private string _otherField;
		public  string  OtherField { get { return _otherField; } set { _otherField = value; } }

		private string _expression;
		public  string  Expression { get { return _expression; } set { _expression = value; } }
	}
}
