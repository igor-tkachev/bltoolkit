using System;
using System.Data.SqlTypes;

namespace BLToolkit.Common
{
	public static partial class Convert<T,P>
	{
		#region Type

		// Scalar Types.
		//
		sealed class T_S         : CB<Type,String>     { public override Type C(String p)      { return p == null      ? null: Type.GetType(p);                   } }
		sealed class T_AC        : CB<Type,Char[]>     { public override Type C(Char[] p)      { return p == null      ? null: Type.GetType(new string(p));       } }
		sealed class T_G         : CB<Type,Guid>       { public override Type C(Guid p)        { return p == Guid.Empty? null: Type.GetTypeFromCLSID(p);          } }

		// Nullable Types.
		//
		sealed class T_NG        : CB<Type,Guid?>      { public override Type C(Guid? p)       { return p.HasValue? Type.GetTypeFromCLSID(p.Value): null; } }

		// SqlTypes.
		//
		sealed class T_dbS       : CB<Type,SqlString>  { public override Type C(SqlString p)   { return p.IsNull       ? null: Type.GetType(p.Value);             } }
		sealed class T_dbAC      : CB<Type,SqlChars>   { public override Type C(SqlChars p)    { return p.IsNull       ? null: Type.GetType(new string(p.Value)); } }
		sealed class T_dbG       : CB<Type,SqlGuid>    { public override Type C(SqlGuid p)     { return p.IsNull       ? null: Type.GetTypeFromCLSID(p.Value);    } }

		sealed class T_O         : CB<Type ,object>    { public override Type C(object p)  
			{
				if (p == null) return null;

				// Scalar Types.
				//
				if (p is String)      return Convert<Type,String>     .I.C((String)p);
				if (p is Char[])      return Convert<Type,Char[]>     .I.C((Char[])p);
				if (p is Guid)        return Convert<Type,Guid>       .I.C((Guid)p);

				// Nullable Types.
				//
				if (p is Guid)        return Convert<Type,Guid>       .I.C((Guid)p);

				// SqlTypes.
				//
				if (p is SqlString)   return Convert<Type,SqlString>  .I.C((SqlString)p);
				if (p is SqlChars)    return Convert<Type,SqlChars>   .I.C((SqlChars)p);
				if (p is SqlGuid)     return Convert<Type,SqlGuid>    .I.C((SqlGuid)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static CB<T, P> GetTypeConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new T_S         ());
			if (t == typeof(Char[]))      return (CB<T, P>)(object)(new T_AC        ());
			if (t == typeof(Guid))        return (CB<T, P>)(object)(new T_G         ());

			// Nullable Types.
			//
			if (t == typeof(Guid?))       return (CB<T, P>)(object)(new T_NG        ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new T_dbS       ());
			if (t == typeof(SqlChars))    return (CB<T, P>)(object)(new T_dbAC      ());
			if (t == typeof(SqlGuid))     return (CB<T, P>)(object)(new T_dbG       ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new T_O         ());

			return (CB<T, P>)(object)Convert<Type, object>.I;
		}

		#endregion
	}
}
