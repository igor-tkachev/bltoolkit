using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq
{
	using DataProvider;
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

		DataProviderBase  _lastDataProvider;
		MappingSchema     _lastMappingSchema;
		ExpressionInfo<T> _lastInfo;

		readonly Dictionary<object,ExpressionInfo<T>> _infos = new Dictionary<object, ExpressionInfo<T>>();

		ExpressionInfo<T> GetInfo(DbManager db)
		{
			DataProviderBase  lastDataProvider;
			MappingSchema     lastMappingSchema;
			ExpressionInfo<T> info;

			lock (_sync)
			{
				lastDataProvider  = _lastDataProvider;
				lastMappingSchema = _lastMappingSchema;
				info              = _lastInfo;
			}

			var dataProvider  = db != null ? db.DataProvider  : DbManager.GetDataProvider(DbManager.DefaultConfiguration);
			var mappingSchema = db != null ? db.MappingSchema : Map.DefaultSchema;

			if (lastDataProvider != dataProvider || lastMappingSchema != mappingSchema)
				info = null;

			if (info == null)
			{
				var key = new { dataProvider, mappingSchema };

				lock (_sync)
					_infos.TryGetValue(key, out info);

				if (info == null)
				{
					lock (_sync)
					{
						_infos.TryGetValue(key, out info);

						if (info == null)
						{
							info = new ExpressionParser<T>().Parse(dataProvider, mappingSchema, _expression, _lambda.Parameters.ToArray());

							_infos.Add(key, info);

							_lastDataProvider  = dataProvider;
							_lastMappingSchema = mappingSchema;
							_lastInfo = info;
						}
					}
				}
			}

			return info;
		}

		public IQueryable<T> Create(object[] parameters)
		{
			var db = (DbManager)parameters[0];
			return new Table<T>(db, _expression) { Info = GetInfo(db), Parameters = parameters };
		}

		public T Execute(object[] parameters)
		{
			var db = (DbManager)parameters[0];
			return (T)GetInfo(db).GetElement(null, db, _expression, parameters);
		}
	}
}
