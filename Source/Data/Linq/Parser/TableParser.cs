using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;
	using Reflection.Extension;

	class TableParser : ISequenceParser
	{
		int ISequenceParser.ParsingCounter { get; set; }

		public IParseInfo ParseSequence(IParseInfo parseInfo, Expression expression)
		{
			switch (expression.NodeType)
			{
				case ExpressionType.Constant:
					{
						var c = (ConstantExpression)expression;

						if (c.Value is IQueryable)
							return new TableInfo(parseInfo, expression, ((IQueryable)c.Value).ElementType);
					}

					break;

				case ExpressionType.Call:
					{
						var mc = (MethodCallExpression)expression;

						if (mc.Method.Name == "GetTable")
							goto case ExpressionType.MemberAccess;
					}

					break;

				case ExpressionType.MemberAccess:
					if (expression.Type.IsGenericType && expression.Type.GetGenericTypeDefinition() == typeof(Table<>))
						return new TableInfo(parseInfo, expression, expression.Type.GetGenericArguments()[0]);

					break;
			}

			return null;
		}

		class TableInfo : ParseInfoBase
		{
			readonly Type     _originalType;
			readonly Type     _objectType;
			readonly SqlTable _sqlTable;

			public TableInfo(IParseInfo parseInfo, Expression expression, Type originalType) : base(parseInfo, expression)
			{
				_originalType = originalType;
				_objectType   = GetObjectType();
				_sqlTable     = new SqlTable(parseInfo.Parser.MappingSchema, _objectType);

				SqlQuery.From.Table(_sqlTable);
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

			class MappingData
			{
				public MappingSchema  MappingSchema;
				public ObjectMapper   ObjectMapper;
				public int[]          Index;
				public IValueMapper[] ValueMappers;
			}

			static object MapDataReaderToObject(IDataContext dataContext, IDataReader dataReader, MappingData data)
			{
				var source = data.MappingSchema.CreateDataReaderMapper(dataReader);

				var initContext = new InitContext
				{
					MappingSchema = data.MappingSchema,
					DataSource    = source,
					SourceObject  = dataReader,
					ObjectMapper  = data.ObjectMapper
				};

				var destObject = dataContext.CreateInstance(initContext) ?? data.ObjectMapper.CreateInstance(initContext);

				if (initContext.StopMapping)
					return destObject;

				var smDest = destObject as ISupportMapping;

				if (smDest != null)
				{
					smDest.BeginMapping(initContext);

					if (initContext.StopMapping)
						return destObject;
				}

				if (data.ValueMappers == null)
				{
					var mappers = new IValueMapper[data.Index.Length];

					for (var i = 0; i < data.Index.Length; i++)
					{
						var n = data.Index[i];

						if (n < 0)
							continue;

						if (!data.ObjectMapper.SupportsTypedValues(i))
						{
							mappers[i] = data.MappingSchema.DefaultValueMapper;
							continue;
						}

						var sourceType = source.           GetFieldType(n);
						var destType   = data.ObjectMapper.GetFieldType(i);

						if (sourceType == null) sourceType = typeof(object);
						if (destType   == null) destType   = typeof(object);

						IValueMapper t;

						if (sourceType == destType)
						{
							lock (data.MappingSchema.SameTypeMappers)
								if (!data.MappingSchema.SameTypeMappers.TryGetValue(sourceType, out t))
									data.MappingSchema.SameTypeMappers.Add(sourceType, t = data.MappingSchema.GetValueMapper(sourceType, destType));
						}
						else
						{
							var key = new KeyValuePair<Type,Type>(sourceType, destType);

							lock (data.MappingSchema.DifferentTypeMappers)
								if (!data.MappingSchema.DifferentTypeMappers.TryGetValue(key, out t))
									data.MappingSchema.DifferentTypeMappers.Add(key, t = data.MappingSchema.GetValueMapper(sourceType, destType));
						}

						mappers[i] = t;
					}

					data.ValueMappers = mappers;
				}

				var dest = data.ObjectMapper;
				var idx  = data.Index;
				var ms   = data.ValueMappers;

				for (var i = 0; i < idx.Length; i++)
				{
					var n = idx[i];

					if (n >= 0)
						ms[i].Map(source, dataReader, n, dest, destObject, i);
				}

				if (smDest != null)
					smDest.EndMapping(initContext);

				return destObject;
			}

			static readonly MethodInfo _mapperMethod = ReflectionHelper.Expressor<object>.MethodExpressor(_ => MapDataReaderToObject(null, null, null));

			Expression GetTableExpression(int[] index)
			{
				var data = new MappingData
				{
					MappingSchema = Parser.MappingSchema,
					ObjectMapper  = Parser.MappingSchema.GetObjectMapper(_objectType),
					Index         = index
				};

				return Expression.Convert(
					Expression.Call(null, _mapperMethod,
						ExpressionParser.DataContextParam,
						ExpressionParser.DataReaderParam,
						Expression.Constant(data)),
					_objectType);
			}

			public override Expression BuildExpression(IParseInfo rootParse, Expression expression, int level)
			{
				if (expression == null)
				{
					var index = rootParse.ConvertToIndex(expression, 0, ConvertFlags.None);
					return GetTableExpression(index.ToArray());
				}

				throw new NotImplementedException();
			}

			public override IEnumerable<ISqlExpression> ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				if (expression == null)
				{
					foreach (var field in _sqlTable.Fields.Values)
						yield return field;
				}
				else
					throw new NotImplementedException();
			}

			readonly Dictionary<ISqlExpression,int> _indexes = new Dictionary<ISqlExpression,int>();

			public override IEnumerable<int> ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				foreach (var expr in ConvertToSql(expression, level, flags))
				{
					int n;

					if (!_indexes.TryGetValue(expr, out n))
					{
						if (expr is SqlField)
						{
							var field = (SqlField)expr;
							n = SqlQuery.Select.Add(field, field.Alias);
						}
						else
						{
							n = SqlQuery.Select.Add(expr);
						}

						_indexes.Add(expr, n);
					}

					yield return n;
				}
			}

			public override void SetAlias(string alias)
			{
				if (_sqlTable.Alias == null)
					_sqlTable.Alias = alias;
			}
		}
	}
}
