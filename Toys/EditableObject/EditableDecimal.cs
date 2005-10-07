using System;

namespace Rsdn.Framework.EditableObject
{
	public class EditableDecimal : EditableValue
	{
		public EditableDecimal() : base(0m)
		{
		}

		public EditableDecimal(int value) : base(Convert(value))
		{
		}

		private static decimal Convert(int value)
		{
			return 
				value == int.MinValue? decimal.MinValue:
				value == int.MaxValue? decimal.MaxValue:
				(decimal)value;
		}
	}
}
