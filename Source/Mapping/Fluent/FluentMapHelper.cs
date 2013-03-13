using BLToolkit.Reflection.Extension;

namespace BLToolkit.Mapping.Fluent
{
	public static class FluentMapHelper
	{
		public static void MergeExtensions(ExtensionList fromExt, ref ExtensionList toExt)
		{
			foreach (var kv in fromExt)
			{
				TypeExtension toType;
				if (toExt.TryGetValue(kv.Key, out toType))
				{
					MergeExtensions(kv.Value, ref toType);
				}
				else
				{
					toExt.Add(kv.Key, kv.Value);
				}
			}
		}

		public static void MergeExtensions(TypeExtension fromExt, ref TypeExtension toExt)
		{
			if (ReferenceEquals(fromExt, toExt))
			{
				return;
			}
			foreach (var attribute in fromExt.Attributes)
			{
				AttributeExtensionCollection attrs;
				if (toExt.Attributes.TryGetValue(attribute.Key, out attrs))
				{
					MergeExtensions(attribute.Value, ref attrs);
				}
				else
				{
					toExt.Attributes.Add(attribute.Key, attribute.Value);
				}
			}
			foreach (var member in fromExt.Members)
			{
				MemberExtension value;
				if (toExt.Members.TryGetValue(member.Key, out value))
				{
					MergeExtensions(member.Value, ref value);
				}
				else
				{
					toExt.Members.Add(member.Key, member.Value);
				}
			}
		}

		private static void MergeExtensions(MemberExtension fromExt, ref MemberExtension toExt)
		{
			foreach (var attribute in fromExt.Attributes)
			{
				if (toExt.Attributes.ContainsKey(attribute.Key))
				{
					toExt.Attributes.Remove(attribute.Key);
				}
				toExt.Attributes.Add(attribute.Key, attribute.Value);
			}
		}

		private static void MergeExtensions(AttributeExtensionCollection fromExt, ref AttributeExtensionCollection toExt)
		{
			toExt.AddRange(fromExt);
		}
	}
}