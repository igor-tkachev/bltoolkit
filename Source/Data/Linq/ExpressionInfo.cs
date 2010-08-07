using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq
{
	using Common;
	using Data.Sql;
	using Data.Sql.SqlProvider;
	using Mapping;
	using Reflection;

	class ExpressionInfo<T> : ReflectionHelper
	{
		#region Properties & Fields

		public ExpressionInfo<T>     Next;
		public Expression            Expression;
		public ParameterExpression[] Parameters;
		public string                ContextID;
		public MappingSchema         MappingSchema;
		public List<QueryInfo>       Queries = new List<QueryInfo>(1);
		public Func<ISqlProvider>    CreateSqlProvider;

		public Func<QueryContext,IDataContextInfo,Expression,object[],IEnumerable<T>> GetIEnumerable;
		public Func<QueryContext,IDataContextInfo,Expression,object[],object>         GetElement;

		private ISqlProvider _sqlProvider; 
		public  ISqlProvider  SqlProvider
		{
			get { return _sqlProvider ?? (_sqlProvider = CreateSqlProvider()); }
		}

		#endregion

		#region GetInfo

		static          ExpressionInfo<T> _first;
		static readonly object            _sync      = new object();
		const           int               _cacheSize = 100;

		public static ExpressionInfo<T> GetExpressionInfo(IDataContextInfo dataContextInfo, Expression expr)
		{
			var info = FindInfo(dataContextInfo, expr);

			if (info == null)
			{
				lock (_sync)
				{
					info = FindInfo(dataContextInfo, expr);

					if (info == null)
					{
						info = new ExpressionParser<T>().Parse(
							dataContextInfo.ContextID,
							dataContextInfo.MappingSchema,
							dataContextInfo.CreateSqlProvider,
							expr,
							null);
						info.Next = _first;
						_first = info;
					}
				}
			}

			return info;
		}

		static ExpressionInfo<T> FindInfo(IDataContextInfo dataContextInfo, Expression expr)
		{
			ExpressionInfo<T> prev = null;
			var n = 0;

			for (var info = _first; info != null; info = info.Next)
			{
				if (info.Compare(dataContextInfo.ContextID, dataContextInfo.MappingSchema, expr))
				{
					if (prev != null)
					{
						lock (_sync)
						{
							prev.Next = info.Next;
							info.Next = _first;
							_first    = info;
						}
					}

					return info;
				}

				if (n++ >= _cacheSize)
				{
					info.Next = null;
					return null;
				}

				prev = info;
			}

			return null;
		}

		#endregion

		#region NonQueryQuery

		private void FinalizeQuery()
		{
			foreach (var sql in Queries)
			{
				sql.SqlQuery   = SqlProvider.Finalize(sql.SqlQuery);
				sql.Parameters = sql.Parameters
					.Select(p => new { p, idx = sql.SqlQuery.Parameters.IndexOf(p.SqlParameter) })
					.OrderBy(p => p.idx)
					.Select(p => p.p)
					.ToList();
			}
		}

		public void SetNonQueryQuery()
		{
			FinalizeQuery();

			if (Queries.Count != 1)
				throw new InvalidOperationException();

			SqlProvider.SqlQuery = Queries[0].SqlQuery;

			GetElement = (ctx, db, expr, ps) => NonQueryQuery(db, expr, ps);
		}

		int NonQueryQuery(IDataContextInfo dataContextInfo, Expression expr, object[] parameters)
		{
			var dataContext = dataContextInfo.DataContext;

			object query = null;

			try
			{
				query = SetCommand(dataContext, expr, parameters, 0);
				return dataContext.ExecuteNonQuery(query);
			}
			finally
			{
				if (query != null)
					dataContext.ReleaseQuery(query);

				if (dataContextInfo.DisposeContext)
					dataContext.Dispose();
			}
		}

		#endregion

		#region ScalarQuery

		public void SetScalarQuery<TS>()
		{
			FinalizeQuery();

			if (Queries.Count != 1)
				throw new InvalidOperationException();

			SqlProvider.SqlQuery = Queries[0].SqlQuery;

			GetElement = (ctx, db, expr, ps) => ScalarQuery<TS>(db, expr, ps);
		}

		TS ScalarQuery<TS>(IDataContextInfo dataContextInfo, Expression expr, object[] parameters)
		{
			var dataContext = dataContextInfo.DataContext;

			object query = null;

			try
			{
				query = SetCommand(dataContext, expr, parameters, 0);
				return (TS)dataContext.ExecuteScalar(query);
			}
			finally
			{
				if (query != null)
					dataContext.ReleaseQuery(query);

				if (dataContextInfo.DisposeContext)
					dataContext.Dispose();
			}
		}

		#endregion

		#region ElementQuery

		public void SetElementQuery<TE>(Mapper<TE> mapper)
		{
			FinalizeQuery();

			if (Queries.Count != 1)
				throw new InvalidOperationException();

			SqlProvider.SqlQuery = Queries[0].SqlQuery;

			GetElement = (ctx, db, expr, ps) => Query(ctx, db, expr, ps, mapper);
		}

		TE Query<TE>(QueryContext ctx, IDataContextInfo dataContextInfo, Expression expr, object[] parameters, Mapper<TE> mapper)
		{
			var dataContext = dataContextInfo.DataContext;

			object query = null;

			try
			{
				query = SetCommand(dataContext, expr, parameters, 0);

				using (var dr = dataContext.ExecuteReader(query))
					while (dr.Read())
						return mapper(this, ctx, dataContext, dr, MappingSchema, expr, parameters);

				return Array<TE>.Empty.First();
			}
			finally
			{
				if (query != null)
					dataContext.ReleaseQuery(query);

				if (dataContextInfo.DisposeContext)
					dataContext.Dispose();
			}
		}

		#endregion

		#region Query

		public void SetQuery(Mapper<T> mapper)
		{
			Queries[0].Mapper = mapper;

			FinalizeQuery();

			if (Queries.Count != 1)
				throw new InvalidOperationException();

			Func<IDataContextInfo,Expression,object[],int,IEnumerable<IDataReader>> query = Query;

			SqlProvider.SqlQuery = Queries[0].SqlQuery;

			var select = Queries[0].SqlQuery.Select;

			if (select.SkipValue != null && !SqlProvider.IsSkipSupported)
			{
				var q = query;

				if (select.SkipValue is SqlValue)
				{
					var n = (int)((IValueContainer)select.SkipValue).Value;

					if (n > 0)
						query = (db, expr, ps, qn) => q(db, expr, ps, qn).Skip(n);
				}
				else if (select.SkipValue is SqlParameter)
				{
					var i = GetParameterIndex(select.SkipValue);
					query = (db, expr, ps, qn) => q(db, expr, ps, qn).Skip((int)Queries[0].Parameters[i].Accessor(this, expr, ps));
				}
			}

			if (select.TakeValue != null && !SqlProvider.IsTakeSupported)
			{
				var q = query;

				if (select.TakeValue is SqlValue)
				{
					var n = (int)((IValueContainer)select.TakeValue).Value;

					if (n > 0)
						query = (db, expr, ps, qn) => q(db, expr, ps, qn).Take(n);
				}
				else if (select.TakeValue is SqlParameter)
				{
					var i = GetParameterIndex(select.TakeValue);
					query = (db, expr, ps, qn) => q(db, expr, ps, qn).Take((int)Queries[0].Parameters[i].Accessor(this, expr, ps));
				}
			}

			if (mapper != null)
			{
				GetIEnumerable = (ctx, db, expr, ps) => Map(query(db, expr, ps, 0), ctx, db, expr, ps, mapper);
			}
			else
			{
				var index = new int[select.Columns.Count];

				for (var i = 0; i < index.Length; i++)
					index[i] = i;

				var slot = GetMapperSlot(index);

				GetIEnumerable = (_, db, expr, ps) => Map(db.DataContext, query(db, expr, ps, 0), slot);
			}
		}

		int GetParameterIndex(ISqlExpression parameter)
		{
			for (var i = 0; i < Queries[0].Parameters.Count; i++)
			{
				var p = Queries[0].Parameters[i].SqlParameter;
						
				if (p == parameter)
					return i;
			}

			throw new InvalidOperationException();
		}

		IEnumerable<IDataReader> Query(IDataContextInfo dataContextInfo, Expression expr, object[] parameters, int queryNumber)
		{
			var dataContext = dataContextInfo.DataContext;

			object query = null;

			try
			{
				query = SetCommand(dataContext, expr, parameters, queryNumber);

				using (var dr = dataContext.ExecuteReader(query))
					while (dr.Read())
						yield return dr;
			}
			finally
			{
				if (query != null)
					dataContext.ReleaseQuery(query);

				if (dataContextInfo.DisposeContext)
					dataContext.Dispose();
			}
		}

		IEnumerable<T> Map(IDataContext dataContext, IEnumerable<IDataReader> data, int slot)
		{
			foreach (var dr in data)
				yield return (T)MapDataReaderToObject(typeof(T), dataContext, dr, slot);
		}

		IEnumerable<T> Map(IEnumerable<IDataReader> data, QueryContext context, IDataContextInfo dataContextInfo, Expression expr, object[] ps, Mapper<T> mapper)
		{
			if (context == null)
				context = new QueryContext { RootDataContext = dataContextInfo };

			foreach (var dr in data)
			{
				context.AfterQuery();
				yield return mapper(this, context, dataContextInfo.DataContext, dr, MappingSchema, expr, ps);
			}
		}

		object SetCommand(IDataContext dataContext, Expression expr, object[] parameters, int idx)
		{
			lock (this)
			{
				SetParameters(expr, parameters, idx);
				return dataContext.SetQuery(Queries[idx]);
			}
		}

		void SetParameters(Expression expr, object[] parameters, int idx)
		{
			foreach (var p in Queries[idx].Parameters)
				p.SqlParameter.Value = p.Accessor(this, expr, parameters);
		}

		#endregion

		#region Mapping

		class MapperSlot
		{
			public ObjectMapper   ObjectMapper;
			public IValueMapper[] ValueMappers;
			public int[]          Index;
		}

		MapperSlot[] _mapperSlots;

		internal int GetMapperSlot(int[] index)
		{
			if (_mapperSlots == null)
			{
				_mapperSlots = new [] { new MapperSlot { Index = index } };
			}
			else
			{
				var slots = new MapperSlot[_mapperSlots.Length + 1];

				slots[_mapperSlots.Length] = new MapperSlot { Index = index };

				_mapperSlots.CopyTo(slots, 0);
				_mapperSlots = slots;
			}

			return _mapperSlots.Length - 1;
		}

		protected object MapDataReaderToObject(Type destObjectType, IDataContext dataContext, IDataReader dataReader, int slotNumber)
		{
			var slot   = _mapperSlots[slotNumber];
			var source = MappingSchema.CreateDataReaderMapper(dataReader);
			var dest   = slot.ObjectMapper ?? (slot.ObjectMapper = MappingSchema.GetObjectMapper(destObjectType));

			var initContext = new InitContext
			{
				MappingSchema = MappingSchema,
				DataSource    = source,
				SourceObject  = dataReader,
				ObjectMapper  = dest
			};

			var destObject = dataContext.CreateInstance(initContext) ?? dest.CreateInstance(initContext);

			if (initContext.StopMapping)
				return destObject;

			var smDest = destObject as ISupportMapping;

			if (smDest != null)
			{
				smDest.BeginMapping(initContext);

				if (initContext.StopMapping)
					return destObject;
			}

			var index = slot.Index;

			if (slot.ValueMappers == null)
			{
				var mappers = new IValueMapper[index.Length];

				for (var i = 0; i < index.Length; i++)
				{
					var n = index[i];

					if (n < 0)
						continue;

					if (!dest.SupportsTypedValues(i))
					{
						mappers[i] = MappingSchema.DefaultValueMapper;
						continue;
					}

					var sourceType = source.GetFieldType(n);
					var destType   = dest.  GetFieldType(i);

					if (sourceType == null) sourceType = typeof(object);
					if (destType   == null) destType   = typeof(object);

					if (sourceType == destType)
					{
						var t = (IValueMapper)MappingSchema.SameTypeMappers[sourceType];

						if (t == null)
						{
							lock (MappingSchema.SameTypeMappers.SyncRoot)
							{
								t = (IValueMapper)MappingSchema.SameTypeMappers[sourceType];
								if (t == null)
									MappingSchema.SameTypeMappers[sourceType] = t = MappingSchema.GetValueMapper(sourceType, destType);
							}
						}

						mappers[i] = t;
					}
					else
					{
						var key = new KeyValuePair<Type,Type>(sourceType, destType);
						var t   = (IValueMapper)MappingSchema.DifferentTypeMappers[key];

						if (t == null)
						{
							lock (MappingSchema.DifferentTypeMappers.SyncRoot)
							{
								t = (IValueMapper)MappingSchema.DifferentTypeMappers[key];
								if (t == null)
									MappingSchema.DifferentTypeMappers[key] = t = MappingSchema.GetValueMapper(sourceType, destType);
							}
						}

						mappers[i] = t;
					}
				}

				slot.ValueMappers = mappers;
			}

			var ms = slot.ValueMappers;

			for (var i = 0; i < index.Length; i++)
			{
				var n = index[i];

				if (n >= 0)
					ms[i].Map(source, dataReader, n, dest, destObject, i);
			}

			if (smDest != null)
				smDest.EndMapping(initContext);

			return destObject;
		}

		public MethodInfo GetMapperMethodInfo()
		{
			return Expressor<ExpressionInfo<T>>.MethodExpressor(e => e.MapDataReaderToObject(null, null, null, 0));
		}

		#endregion

		#region GroupJoinEnumerator

		static IEnumerable<TElement> GetGroupJoinEnumerator<TElement>(int count, TElement item)
		{
			for (var i = 0; i < count; i++)
				yield return item;
		}

		public IEnumerable<TElement> GetGroupJoinEnumerator<TElement>(
			QueryContext     qc,
			IDataContext     dataContext,
			IDataReader      dataReader,
			Expression       expr,
			object[]         ps,
			int              counterIndex,
			Mapper<TElement> itemReader)
		{
			var count = MappingSchema.ConvertToInt32(dataReader[counterIndex]);
			return GetGroupJoinEnumerator(count, count == 0? default(TElement): itemReader(this, qc, dataContext, dataReader, MappingSchema, expr, ps));
		}

		public MethodInfo GetGroupJoinEnumeratorMethodInfo<TElement>()
		{
			return Expressor<ExpressionInfo<T>>.MethodExpressor(e => e.GetGroupJoinEnumerator<TElement>(null, null, null, null, null, 0, null));
		}

		#endregion

		#region Grouping

		class Grouping<TKey,TElement> : IGrouping<TKey,TElement>
		{
			public Grouping(TKey key, IEnumerable<TElement> items)
			{
				Key    = key;
				_items = items;
			}

			readonly IEnumerable<TElement> _items;

			public TKey Key { get; set; }

			public IEnumerator<TElement> GetEnumerator()
			{
				return _items.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		public IGrouping<TKey,TElement> GetGrouping<TKey,TElement>(
			QueryContext             context,
			IDataContext             dataContext,
			IDataReader              dataReader,
			Expression               expr,
			object[]                 ps,
			Mapper<TKey>             keyReader,
			ExpressionInfo<TElement> valueReader)
		{
			var db = context.GetDataContext();

			try
			{
				var key = keyReader(this, context, dataContext, dataReader, MappingSchema, expr, ps);

				ps = ps == null ? new object[1] : ps.ToArray();
				ps[0] = key;

				var values = valueReader.GetIEnumerable(context, db.DataContextInfo, valueReader.Expression, ps);

				return new Grouping<TKey, TElement>(key, Configuration.Linq.PreloadGroups ? values.ToList() : values);
			}
			finally
			{
				context.ReleaseDataContext(db);
			}
		}

		public MethodInfo GetGroupingMethodInfo<TKey,TElement>()
		{
			return Expressor<ExpressionInfo<T>>.MethodExpressor(e => e.GetGrouping<TKey,TElement>(null, null, null, null, null, null, null));
		}

		#endregion

		#region Element Operations

		public void MakeElementOperator(ElementMethod em)
		{
			switch (em)
			{
				case ElementMethod.First           : GetElement = (ctx, db, expr, ps) => GetIEnumerable(ctx, db, expr, ps).First();           break;
				case ElementMethod.FirstOrDefault  : GetElement = (ctx, db, expr, ps) => GetIEnumerable(ctx, db, expr, ps).FirstOrDefault();  break;
				case ElementMethod.Single          : GetElement = (ctx, db, expr, ps) => GetIEnumerable(ctx, db, expr, ps).Single();          break;
				case ElementMethod.SingleOrDefault : GetElement = (ctx, db, expr, ps) => GetIEnumerable(ctx, db, expr, ps).SingleOrDefault(); break;
			}
		}

		#endregion

		#region Compare

		public bool Compare(string contextID, MappingSchema mappingSchema, Expression expr)
		{
			return
				ContextID.Length == contextID.Length &&
				ContextID        == contextID        &&
				MappingSchema    == mappingSchema    &&
				ExpressionHelper.Compare(Expression, expr, _queryableAccessorDic);
		}

		readonly Dictionary<Expression,Func<Expression,IQueryable>> _queryableAccessorDic  = new Dictionary<Expression,Func<Expression,IQueryable>>();
		readonly List<Func<Expression,IQueryable>>                  _queryableAccessorList = new List<Func<Expression,IQueryable>>();

		public int AddQueryableAccessors(Expression expr, Func<Expression,IQueryable> qe)
		{
			_queryableAccessorDic. Add(expr, qe);
			_queryableAccessorList.Add(qe);

			return _queryableAccessorList.Count - 1;
		}

		public Expression GetIQueryable(int n, Expression expr)
		{
			return _queryableAccessorList[n](expr).Expression;
		}

		#endregion

		#region GetSqlText

		public string GetSqlText(IDataContext dataContext, Expression expr, object[] parameters, int idx)
		{
			var query = SetCommand(dataContext, expr, parameters, 0);
			return dataContext.GetSqlText(query);
		}

		#endregion

		#region Inner Types

		public delegate TE Mapper<TE>(ExpressionInfo<T> info, QueryContext qc, IDataContext dc, IDataReader rd, MappingSchema ms, Expression expr, object[] ps);

		public class Parameter
		{
			public Expression                                         Expression;
			public Func<ExpressionInfo<T>,Expression,object[],object> Accessor;
			public SqlParameter                                       SqlParameter;
		}

		public class QueryInfo : IQueryContext
		{
			public QueryInfo()
			{
				SqlQuery = new SqlQuery();
			}

			public SqlQuery SqlQuery { get; set; }
			public object   Context  { get; set; }

			public SqlParameter[] GetParameters()
			{
				var ps = new SqlParameter[Parameters.Count];

				for (var i = 0; i < ps.Length; i++)
					ps[i] = Parameters[i].SqlParameter;

				return ps;
			}

			public List<Parameter> Parameters = new List<Parameter>();
			public Mapper<T>       Mapper;
		}

		#endregion

		#region Object Operations

		static class ObjectOperation<T1>
		{
			public static readonly Dictionary<object,ExpressionInfo<int>>    Insert             = new Dictionary<object,ExpressionInfo<int>>();
			public static readonly Dictionary<object,ExpressionInfo<object>> InsertWithIdentity = new Dictionary<object,ExpressionInfo<object>>();
			public static readonly Dictionary<object,ExpressionInfo<int>>    Update             = new Dictionary<object,ExpressionInfo<int>>();
			public static readonly Dictionary<object,ExpressionInfo<int>>    Delete             = new Dictionary<object,ExpressionInfo<int>>();
		}

		static ExpressionInfo<TR>.Parameter GetParameter<TR>(IDataContext dataContext, SqlField field)
		{
			var exprParam = Expression.Parameter(typeof(Expression), "expr");
			var mapper    = Expression.Lambda<Func<ExpressionInfo<TR>,Expression,object[],object>>(
				Expression.Convert(
					Expression.PropertyOrField(
						Expression.Convert(
							Expression.Property(
								Expression.Convert(exprParam, typeof(ConstantExpression)),
								Constant.Value),
							typeof(T)),
						field.Name),
					typeof(object)),
				new []
					{
						Expression.Parameter(typeof(ExpressionInfo<TR>), "info"),
						exprParam,
						Expression.Parameter(typeof(object[]), "ps")
					});

			var param = new ExpressionInfo<TR>.Parameter
			{
				Expression   = null,
				Accessor     = mapper.Compile(),
				SqlParameter = new SqlParameter(field.SystemType, field.Name, null)
			};

			if (field.SystemType.IsEnum)
				param.SqlParameter.SetEnumConverter(field.SystemType, dataContext.MappingSchema);

			return param;
		}

		#region Insert

		public static int Insert(IDataContextInfo dataContextInfo, T obj)
		{
			if (Equals(default(T), obj))
				return 0;

			ExpressionInfo<int> ei;

			var key = new { dataContextInfo.MappingSchema, dataContextInfo.ContextID };

			if (!ObjectOperation<T>.Insert.TryGetValue(key, out ei))
				lock (_sync)
					if (!ObjectOperation<T>.Insert.TryGetValue(key, out ei))
					{
						var sqlTable = new SqlTable<T>(dataContextInfo.MappingSchema);
						var sqlQuery = new SqlQuery { QueryType = QueryType.Insert };

						sqlQuery.Set.Into = sqlTable;

						ei = new ExpressionInfo<int>
						{
							MappingSchema     = dataContextInfo.MappingSchema,
							ContextID         = dataContextInfo.ContextID,
							CreateSqlProvider = dataContextInfo.CreateSqlProvider,
							Queries           = { new ExpressionInfo<int>.QueryInfo { SqlQuery = sqlQuery, } }
						};

						foreach (var field in sqlTable.Fields)
						{
							if (field.Value.IsInsertable)
							{
								var param = GetParameter<int>(dataContextInfo.DataContext, field.Value);

								ei.Queries[0].Parameters.Add(param);

								sqlQuery.Set.Items.Add(new SqlQuery.SetExpression(field.Value, param.SqlParameter));
							}
						}

						ei.SetNonQueryQuery();

						ObjectOperation<T>.Insert.Add(key, ei);
					}

			return (int)ei.GetElement(null, dataContextInfo, Expression.Constant(obj), null);
		}

		#endregion

		#region InsertWithIdentity

		public static object InsertWithIdentity(IDataContextInfo dataContextInfo, T obj)
		{
			if (Equals(default(T), obj))
				return 0;

			ExpressionInfo<object> ei;

			var key = new { dataContextInfo.MappingSchema, dataContextInfo.ContextID };

			if (!ObjectOperation<T>.InsertWithIdentity.TryGetValue(key, out ei))
				lock (_sync)
					if (!ObjectOperation<T>.InsertWithIdentity.TryGetValue(key, out ei))
					{
						var sqlTable = new SqlTable<T>(dataContextInfo.MappingSchema);
						var sqlQuery = new SqlQuery { QueryType = QueryType.Insert };

						sqlQuery.Set.Into         = sqlTable;
						sqlQuery.Set.WithIdentity = true;

						ei = new ExpressionInfo<object>
						{
							MappingSchema     = dataContextInfo.MappingSchema,
							ContextID         = dataContextInfo.ContextID,
							CreateSqlProvider = dataContextInfo.CreateSqlProvider,
							Queries           = { new ExpressionInfo<object>.QueryInfo { SqlQuery = sqlQuery, } }
						};

						foreach (var field in sqlTable.Fields)
						{
							if (field.Value.IsInsertable)
							{
								var param = GetParameter<object>(dataContextInfo.DataContext, field.Value);

								ei.Queries[0].Parameters.Add(param);

								sqlQuery.Set.Items.Add(new SqlQuery.SetExpression(field.Value, param.SqlParameter));
							}
						}

						ei.SetScalarQuery<object>();

						ObjectOperation<T>.InsertWithIdentity.Add(key, ei);
					}

			return ei.GetElement(null, dataContextInfo, Expression.Constant(obj), null);
		}

		#endregion

		#region Update

		public static int Update(IDataContextInfo dataContextInfo, T obj)
		{
			if (Equals(default(T), obj))
				return 0;

			ExpressionInfo<int> ei;

			var key = new { dataContextInfo.MappingSchema, dataContextInfo.ContextID };

			if (!ObjectOperation<T>.Update.TryGetValue(key, out ei))
				lock (_sync)
					if (!ObjectOperation<T>.Update.TryGetValue(key, out ei))
					{
						var sqlTable = new SqlTable<T>(dataContextInfo.MappingSchema);
						var sqlQuery = new SqlQuery { QueryType = QueryType.Update };

						sqlQuery.From.Table(sqlTable);

						ei = new ExpressionInfo<int>
						{
							MappingSchema     = dataContextInfo.MappingSchema,
							ContextID         = dataContextInfo.ContextID,
							CreateSqlProvider = dataContextInfo.CreateSqlProvider,
							Queries           = { new ExpressionInfo<int>.QueryInfo { SqlQuery = sqlQuery, } }
						};

						var keys   = sqlTable.GetKeys(true).Cast<SqlField>();
						var fields = sqlTable.Fields.Values.Where(f => f.IsUpdatable).Except(keys).ToList();

						if (fields.Count == 0)
							throw new LinqException(
								string.Format("There are no fields to update in the type '{0}'.", sqlTable.Name));

						foreach (var field in fields)
						{
							var param = GetParameter<int>(dataContextInfo.DataContext, field);

							ei.Queries[0].Parameters.Add(param);

							sqlQuery.Set.Items.Add(new SqlQuery.SetExpression(field, param.SqlParameter));
						}

						foreach (var field in keys)
						{
							var param = GetParameter<int>(dataContextInfo.DataContext, field);

							ei.Queries[0].Parameters.Add(param);

							sqlQuery.Where.Field(field).Equal.Expr(param.SqlParameter);
						}

						ei.SetNonQueryQuery();

						ObjectOperation<T>.Update.Add(key, ei);
					}

			return (int)ei.GetElement(null, dataContextInfo, Expression.Constant(obj), null);
		}

		#endregion

		#region Delete

		public static int Delete(IDataContextInfo dataContextInfo, T obj)
		{
			if (Equals(default(T), obj))
				return 0;

			ExpressionInfo<int> ei;

			var key = new { dataContextInfo.MappingSchema, dataContextInfo.ContextID };

			if (!ObjectOperation<T>.Delete.TryGetValue(key, out ei))
				lock (_sync)
					if (!ObjectOperation<T>.Delete.TryGetValue(key, out ei))
					{
						var sqlTable = new SqlTable<T>(dataContextInfo.MappingSchema);
						var sqlQuery = new SqlQuery { QueryType = QueryType.Delete };

						sqlQuery.From.Table(sqlTable);

						ei = new ExpressionInfo<int>
						{
							MappingSchema     = dataContextInfo.MappingSchema,
							ContextID         = dataContextInfo.ContextID,
							CreateSqlProvider = dataContextInfo.CreateSqlProvider,
							Queries           = { new ExpressionInfo<int>.QueryInfo { SqlQuery = sqlQuery, } }
						};

						var keys = sqlTable.GetKeys(true).Cast<SqlField>().ToList();

						if (keys.Count == 0)
							throw new LinqException(
								string.Format("Table '{0}' does not have primary key.", sqlTable.Name));

						foreach (var field in keys)
						{
							var param = GetParameter<int>(dataContextInfo.DataContext, field);

							ei.Queries[0].Parameters.Add(param);

							sqlQuery.Where.Field(field).Equal.Expr(param.SqlParameter);
						}

						ei.SetNonQueryQuery();

						ObjectOperation<T>.Delete.Add(key, ei);
					}

			return (int)ei.GetElement(null, dataContextInfo, Expression.Constant(obj), null);
		}

		#endregion

		#endregion
	}
}
