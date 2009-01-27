using System;

namespace BLToolkit.Data.SqlBuilder1
{
	using FJoin = Sql.FromClause.Join;

	public static class Extensions
	{
		public static FJoin InnerJoin    (this ITableSource table,               params FJoin[] joins) { return Sql.InnerJoin    (table,        joins); }
		public static FJoin InnerJoin    (this ITableSource table, string alias, params FJoin[] joins) { return Sql.InnerJoin    (table, alias, joins); }
		public static FJoin LeftJoin     (this ITableSource table,               params FJoin[] joins) { return Sql.LeftJoin     (table,        joins); }
		public static FJoin LeftJoin     (this ITableSource table, string alias, params FJoin[] joins) { return Sql.LeftJoin     (table, alias, joins); }
		public static FJoin Join         (this ITableSource table,               params FJoin[] joins) { return Sql.Join         (table,        joins); }
		public static FJoin Join         (this ITableSource table, string alias, params FJoin[] joins) { return Sql.Join         (table, alias, joins); }
																												
		public static FJoin WeakInnerJoin(this ITableSource table,               params FJoin[] joins) { return Sql.WeakInnerJoin(table,        joins); }
		public static FJoin WeakInnerJoin(this ITableSource table, string alias, params FJoin[] joins) { return Sql.WeakInnerJoin(table, alias, joins); }
		public static FJoin WeakLeftJoin (this ITableSource table,               params FJoin[] joins) { return Sql.WeakLeftJoin (table,        joins); }
		public static FJoin WeakLeftJoin (this ITableSource table, string alias, params FJoin[] joins) { return Sql.WeakLeftJoin (table, alias, joins); }
		public static FJoin WeakJoin     (this ITableSource table,               params FJoin[] joins) { return Sql.WeakJoin     (table,        joins); }
		public static FJoin WeakJoin     (this ITableSource table, string alias, params FJoin[] joins) { return Sql.WeakJoin     (table, alias, joins); }
	}
}
