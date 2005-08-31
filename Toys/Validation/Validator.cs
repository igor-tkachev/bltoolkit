using System;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.Validation
{
	public class Validator
	{
		public static void Validate(object obj)
		{
			MapDescriptor md = Map.Descriptor(obj.GetType());

			foreach (IMemberMapper mm in md)
			{
				object[] attrs = mm.MemberInfo.GetCustomAttributes(typeof(ValidatorBaseAttribute), true);

				foreach (ValidatorBaseAttribute attr in attrs)
					attr.Validate(mm.GetValue(obj), mm.MemberInfo);
			}
		}
	}
}
