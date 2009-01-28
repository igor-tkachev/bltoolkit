using System;

namespace BLToolkit.Data.Sql
{
	using FJoin = SqlBuilder.FromClause.Join;

	public static class Extensions
	{
		public static FJoin InnerJoin    (this ISqlTableSource table,               params FJoin[] joins) { return SqlBuilder.InnerJoin    (table,        joins); }
		public static FJoin InnerJoin    (this ISqlTableSource table, string alias, params FJoin[] joins) { return SqlBuilder.InnerJoin    (table, alias, joins); }
		public static FJoin LeftJoin     (this ISqlTableSource table,               params FJoin[] joins) { return SqlBuilder.LeftJoin     (table,        joins); }
		public static FJoin LeftJoin     (this ISqlTableSource table, string alias, params FJoin[] joins) { return SqlBuilder.LeftJoin     (table, alias, joins); }
		public static FJoin Join         (this ISqlTableSource table,               params FJoin[] joins) { return SqlBuilder.Join         (table,        joins); }
		public static FJoin Join         (this ISqlTableSource table, string alias, params FJoin[] joins) { return SqlBuilder.Join         (table, alias, joins); }

		public static FJoin WeakInnerJoin(this ISqlTableSource table,               params FJoin[] joins) { return SqlBuilder.WeakInnerJoin(table,        joins); }
		public static FJoin WeakInnerJoin(this ISqlTableSource table, string alias, params FJoin[] joins) { return SqlBuilder.WeakInnerJoin(table, alias, joins); }
		public static FJoin WeakLeftJoin (this ISqlTableSource table,               params FJoin[] joins) { return SqlBuilder.WeakLeftJoin (table,        joins); }
		public static FJoin WeakLeftJoin (this ISqlTableSource table, string alias, params FJoin[] joins) { return SqlBuilder.WeakLeftJoin (table, alias, joins); }
		public static FJoin WeakJoin     (this ISqlTableSource table,               params FJoin[] joins) { return SqlBuilder.WeakJoin     (table,        joins); }
		public static FJoin WeakJoin     (this ISqlTableSource table, string alias, params FJoin[] joins) { return SqlBuilder.WeakJoin     (table, alias, joins); }
	}
}
