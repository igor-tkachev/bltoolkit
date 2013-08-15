using System;
using System.Collections.Generic;

namespace BLToolkit.Reflection.Extension
{
	public class ExtensionList : Dictionary<string,TypeExtension>
	{
		public new TypeExtension this[string typeName]
		{
			get
			{
				TypeExtension value;
				lock (this)
					return TryGetValue(typeName, out value) ? value : TypeExtension.Null;
			}
		}

		public TypeExtension this[Type type]
		{
			get
			{
				lock (this)
					foreach (var ext in Values)
						if (ext.Name == type.Name || ext.Name == type.FullName)
							return ext;

				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
					return this[Nullable.GetUnderlyingType(type)];

				return TypeExtension.Null;
			}
		}

		public void Add(TypeExtension typeInfo)
		{
			lock (this)
				Add(typeInfo.Name, typeInfo);
		}
	}
}
