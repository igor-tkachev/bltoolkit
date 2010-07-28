using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq
{
	using Mapping;

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

		string            _lastContextID;
		MappingSchema     _lastMappingSchema;
		ExpressionInfo<T> _lastInfo;

		readonly Dictionary<object,ExpressionInfo<T>> _infos = new Dictionary<object, ExpressionInfo<T>>();

		ExpressionInfo<T> GetInfo(IDataContext dataContext)
		{
			var dataContextInfo = DataContextInfo.Create(dataContext);

			string            lastContextID;
			MappingSchema     lastMappingSchema;
			ExpressionInfo<T> info;

			lock (_sync)
			{
				lastContextID     = _lastContextID;
				lastMappingSchema = _lastMappingSchema;
				info              = _lastInfo;
			}

			var contextID     = dataContextInfo.ContextID;
			var mappingSchema = dataContextInfo.MappingSchema;

			if (lastContextID != contextID || lastMappingSchema != mappingSchema)
				info = null;

			if (info == null)
			{
				var key = new { contextID, mappingSchema };

				lock (_sync)
					_infos.TryGetValue(key, out info);

				if (info == null)
				{
					lock (_sync)
					{
						_infos.TryGetValue(key, out info);

						if (info == null)
						{
							info = new ExpressionParser<T>().Parse(
								contextID,
								mappingSchema,
								dataContextInfo.CreateSqlProvider,
								_expression,
								_lambda.Parameters.ToArray());

							_infos.Add(key, info);

							_lastContextID     = contextID;
							_lastMappingSchema = mappingSchema;
							_lastInfo          = info;
						}
					}
				}
			}

			return info;
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
