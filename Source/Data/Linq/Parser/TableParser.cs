using System;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;
	using Reflection.Extension;

	class TableParser : ISequenceParser
	{
		int ISequenceParser.ParsingCounter { get; set; }

		public ParseInfo ParseSequence(ExpressionParser parser, Expression expression)
		{
			switch (expression.NodeType)
			{
				case ExpressionType.Constant:
					{
						var c = (ConstantExpression) expression;

						if (c.Value is IQueryable)
							return new TableInfo(parser, ((IQueryable) c.Value).ElementType);
					}

					break;

				case ExpressionType.Call:
					{
						var mc = (MethodCallExpression) expression;

						if (mc.Method.Name == "GetTable")
							goto case ExpressionType.MemberAccess;
					}

					break;

				case ExpressionType.MemberAccess:
					if (expression.Type.IsGenericType && expression.Type.GetGenericTypeDefinition() == typeof(Table<>))
						return new TableInfo(parser, expression.Type.GetGenericArguments()[0]);

					break;
			}

			return null;
		}

		class TableInfo : ParseInfo
		{
			readonly Type     _originalType;
			readonly Type     _objectType;
			readonly SqlTable _sqlTable;

			public TableInfo(ExpressionParser parser, Type originalType) : base(parser)
			{
				_originalType = originalType;
				_objectType   = GetObjectType();
				_sqlTable     = new SqlTable(parser.MappingSchema, _objectType);

				SqlQuery.From.Table(_sqlTable);
			}

			public override Query<T> BuildQuery<T>()
			{
				SqlQuery.Select.Columns.Clear();

				foreach (var field in _sqlTable.Fields.Values)
					SqlQuery.Select.Add(field, field.Alias);

				var query = new Query<T>(Parser);

				query.SetQuery(null, null);

				return query;
			}

			Type GetObjectType()
			{
				for (var type = _originalType.BaseType; type != null && type != typeof(object); type = type.BaseType)
				{
					var extension = TypeExtension.GetTypeExtension(type, Parser.MappingSchema.Extensions);
					var mapping   = Parser.MappingSchema.MetadataProvider.GetInheritanceMapping(type, extension);

					if (mapping.Length > 0)
						return type;
				}

				return _originalType;
			}

			public override void SetAlias(string alias)
			{
				if (_sqlTable.Alias == null)
					_sqlTable.Alias = alias;
			}
		}
	}
}
