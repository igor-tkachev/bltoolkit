﻿using System;
using System.Reflection;
using BLToolkit.Reflection;

namespace BLToolkit.Data.Linq
{
	using Data.Sql;

	[SerializableAttribute]
	[AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class SqlPropertyAttribute : SqlFunctionAttribute
	{
		public SqlPropertyAttribute()
		{
		}

		public SqlPropertyAttribute(string name)
			: base(name)
		{
		}

		public SqlPropertyAttribute(string sqlProvider, string name)
			: base(sqlProvider, name)
		{
		}

		public override ISqlExpression GetExpression(MemberInfo member, params ISqlExpression[] args)
		{
			return new SqlExpression(TypeHelper.GetMemberType(member), Name ?? member.Name, Precedence.Primary);
		}
	}
}
