using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq
{
	using Mapping;
	using Builder;

	class CompiledTable<T>
	{
		public CompiledTable(LambdaExpression lambda, Expression expression)
		{
			_lambda     = lambda;
			_expression = expression;
		}

		readonly LambdaExpression _lambda;
		readonly Expression       _expression;
		readonly object           _sync = new object();

		string        _lastContextID;
		MappingSchema _lastMappingSchema;
		Query<T>      _lastQuery;

		readonly Dictionary<object,Query<T>> _infos = new Dictionary<object, Query<T>>();

		Query<T> GetInfo(IDataContext dataContext)
		{
			var dataContextInfo = DataContextInfo.Create(dataContext);

			string        lastContextID;
			MappingSchema lastMappingSchema;
			Query<T>      query;

			lock (_sync)
			{
				lastContextID     = _lastContextID;
				lastMappingSchema = _lastMappingSchema;
				query             = _lastQuery;
			}

			var contextID     = dataContextInfo.ContextID;
			var mappingSchema = dataContextInfo.MappingSchema;

			if (lastContextID != contextID || lastMappingSchema != mappingSchema)
				query = null;

			if (query == null)
			{
				var key = new { contextID, mappingSchema };

				lock (_sync)
					_infos.TryGetValue(key, out query);

				if (query == null)
				{
					lock (_sync)
					{
						_infos.TryGetValue(key, out query);

						if (query == null)
						{
#if !OLD_PARSER
							query = new ExpressionBuilder(new Query<T>(), dataContextInfo, _expression, _lambda.Parameters.ToArray())
								.Build<T>();
#else
							query = new ExpressionParserOld<T>().Parse(
								contextID,
								mappingSchema,
								dataContextInfo.CreateSqlProvider,
								_expression,
								_lambda.Parameters.ToArray());
#endif

							_infos.Add(key, query);

							_lastContextID     = contextID;
							_lastMappingSchema = mappingSchema;
							_lastQuery         = query;
						}
					}
				}
			}

			return query;
		}

		public IQueryable<T> Create(object[] parameters)
		{
			var db = (IDataContext)parameters[0];
			return new Table<T>(db, _expression) { Info = GetInfo(db), Parameters = parameters };
		}

		public T Execute(object[] parameters)
		{
			var db  = (IDataContext)parameters[0];
			var ctx = DataContextInfo.Create(db);

			return (T)GetInfo(db).GetElement(null, ctx, _expression, parameters);
		}
	}
}
