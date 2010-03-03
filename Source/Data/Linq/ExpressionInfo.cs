using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BLToolkit.Data.Linq
{
	using Common;
	using Data.Sql;
	using Data.Sql.SqlProvider;
	using DataProvider;
	using Mapping;
	using Reflection;

	class ExpressionInfo<T> : ReflectionHelper
	{
		public ExpressionInfo<T>     Next;
		public Expression            Expression;
		public ParameterExpression[] Parameters;
		public DataProviderBase      DataProvider;
		public MappingSchema         MappingSchema;
		public List<QueryInfo>       Queries = new List<QueryInfo>(1);

		public Func<QueryContext,DbManager,Expression,object[],IEnumerable<T>> GetIEnumerable;
		public Func<QueryContext,DbManager,Expression,object[],object>         GetElement;

		private ISqlProvider _sqlProvider; 
		public  ISqlProvider  SqlProvider
		{
			get { return _sqlProvider ?? (_sqlProvider = DataProvider.CreateSqlProvider()); }
		}

		#region GetInfo

		static          ExpressionInfo<T> _first;
		static readonly object            _sync      = new object();
		const           int               _cacheSize = 100;

		public static ExpressionInfo<T> GetExpressionInfo(DataProviderBase dataProvider, MappingSchema mappingSchema, Expression expr)
		{
			var info = FindInfo(dataProvider, mappingSchema, expr);

			if (info == null)
			{
				lock (_sync)
				{
					info = FindInfo(dataProvider, mappingSchema, expr);

					if (info == null)
					{
						info = new ExpressionParser<T>().Parse(dataProvider, mappingSchema, expr, null);
						info.Next = _first;
						_first = info;
					}
				}
			}

			return info;
		}

		static ExpressionInfo<T> FindInfo(DataProviderBase dataProvider, MappingSchema mappingSchema, Expression expr)
		{
			ExpressionInfo<T> prev = null;
			var n = 0;

			for (var info = _first; info != null; info = info.Next)
			{
				if (info.Compare(dataProvider, mappingSchema, expr))
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
				sql.SqlQuery = SqlProvider.Finalize(sql.SqlQuery);
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

		int NonQueryQuery(DbManager db, Expression expr, object[] parameters)
		{
			var dispose = db == null;

			if (db == null)
				db = new DbManager();

			try
			{
				SetCommand(db, expr, parameters, 0);
				return db.ExecuteNonQuery();
			}
			finally
			{
				if (dispose)
					db.Dispose();
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

		TS ScalarQuery<TS>(DbManager db, Expression expr, object[] parameters)
		{
			var dispose = db == null;

			if (db == null)
				db = new DbManager();

			try
			{
				var commands = SetCommand(db, expr, parameters, 0);

				IDbDataParameter idparam = null;

				if (SqlProvider.IsIdentityParameterRequired)
				{
					var sql = Queries[0].SqlQuery;

					if (sql.QueryType == QueryType.Insert && sql.Set.WithIdentity)
					{
						var pname = DataProvider.Convert("IDENTITY_PARAMETER", ConvertType.NameToQueryParameter).ToString();
						idparam   = db.OutputParameter(pname, DbType.Decimal);
						DataProvider.AttachParameter(db.Command, idparam);
					}
				}

				if (commands.Length == 1)
				{
					var ret = db.ExecuteScalar<TS>();
					return idparam != null ? (TS)idparam.Value : ret;
				}

				db.ExecuteNonQuery();

				return db.SetCommand(commands[1]).ExecuteScalar<TS>();
			}
			finally
			{
				if (dispose)
					db.Dispose();
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

		TE Query<TE>(QueryContext ctx, DbManager db, Expression expr, object[] parameters, Mapper<TE> mapper)
		{
			var dispose = db == null;

			if (db == null)
				db = new DbManager();

			try
			{
				SetCommand(db, expr, parameters, 0);
				using (var dr = db.ExecuteReader())
					while (dr.Read())
						return mapper(this, ctx, dr, MappingSchema, expr, parameters);

				return Array<TE>.Empty.First();
			}
			finally
			{
				if (dispose)
					db.Dispose();
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

			Func<DbManager,Expression,object[],int,IEnumerable<IDataReader>> query = Query;

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

				GetIEnumerable = (_, db, expr, ps) => Map(query(db, expr, ps, 0), slot);
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

		IEnumerable<IDataReader> Query(DbManager db, Expression expr, object[] parameters, int queryNumber)
		{
			var dispose = db == null;

			if (db == null)
				db = new DbManager();

			try
			{
				SetCommand(db, expr, parameters, queryNumber);
				using (var dr = db.ExecuteReader())
					while (dr.Read())
						yield return dr;
			}
			finally
			{
				if (dispose)
					db.Dispose();
			}
		}

		IEnumerable<T> Map(IEnumerable<IDataReader> data, int slot)
		{
			foreach (var dr in data)
				yield return (T)MapDataReaderToObject(typeof(T), dr, slot);
		}

		IEnumerable<T> Map(IEnumerable<IDataReader> data, QueryContext context, DbManager db, Expression expr, object[] ps, Mapper<T> mapper)
		{
			if (context == null)
				context = new QueryContext { RootDbManager = db };

			foreach (var dr in data)
			{
				context.AfterQuery();
				yield return mapper(this, context, dr, MappingSchema, expr, ps);
			}
		}

		string[] SetCommand(DbManager db, Expression expr, object[] parameters, int idx)
		{
			string[]           command;
			IDbDataParameter[] parms;

			lock (this)
			{
				SetParameters(expr, parameters, idx);

				List<SqlParameter> ps;
				command = GetCommand(idx, out ps);
				parms   = GetParameters(db, idx, ps);
			}

			db.SetCommand(command[0], parms);

#if DEBUG
			Debug.WriteLineIf(DbManager.TraceSwitch.TraceInfo, GetSqlText(db, parms, command), DbManager.TraceSwitch.DisplayName);
#endif

			return command;
		}

		void SetParameters(Expression expr, object[] parameters, int idx)
		{
			foreach (var p in Queries[idx].Parameters)
				p.SqlParameter.Value = p.Accessor(this, expr, parameters);
		}

		IDbDataParameter[] GetParameters(DbManager db, int idx, IList<SqlParameter> sqlParameters)
		{
			//var sql        = Queries[idx].SqlQuery;
			var parameters = Queries[idx].Parameters;

			if (parameters.Count == 0 && sqlParameters.Count == 0)
				return null;

			var x = db.DataProvider.Convert("x", ConvertType.NameToQueryParameter).ToString();
			var y = db.DataProvider.Convert("y", ConvertType.NameToQueryParameter).ToString();

			var parms = new List<IDbDataParameter>(x == y? sqlParameters.Count: parameters.Count);

			if (x == y)
			{
				for (var i = 0; i < sqlParameters.Count; i++)
				{
					var sqlp = sqlParameters[i];

					if (sqlp.IsQueryParameter)
					{
						var parm = parameters.Count > i && parameters[i].SqlParameter == sqlp ? parameters[i] : parameters.First(p => p.SqlParameter == sqlp);
						parms.Add(db.Parameter(x, parm.SqlParameter.Value));
					}
				}
			}
			else
			{
				for (var i = 0; i < parameters.Count; i++)
				{
					var parm = parameters[i].SqlParameter;

					if (parm.IsQueryParameter && sqlParameters.Contains(parm))
					{
						var name = db.DataProvider.Convert(parm.Name, ConvertType.NameToQueryParameter).ToString();
						parms.Add(db.Parameter(name, parm.Value));
					}
				}
			}

			return parms.ToArray();
		}

		string[] GetCommand(int idx, out List<SqlParameter> parameters)
		{
			var query = Queries[idx];

			if (query.CommandText != null)
			{
				parameters = query.SqlQuery.Parameters;
				return query.CommandText;
			}

			var sql = query.SqlQuery;

			if (sql.ParameterDependent)
			{
				sql = new QueryVisitor().Convert(query.SqlQuery, e =>
				{
					if (e.ElementType == QueryElementType.SqlParameter)
					{
						var p = (SqlParameter)e;

						if (p.Value == null)
							return new SqlValue(null);
					}

					return null;
				});
			}

			var cc = SqlProvider.CommandCount(sql);
			var sb = new StringBuilder();

			var commands = new string[cc];

			for (var i = 0; i < cc; i++)
			{
				sb.Length = 0;

				SqlProvider.BuildSql(i, sql, sb, 0, 0, false);
				commands[i] = sb.ToString();
			}

			if (!query.SqlQuery.ParameterDependent)
				query.CommandText = commands;

			parameters = sql.Parameters;

			return commands;
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

		protected object MapDataReaderToObject(Type destObjectType, IDataReader dataReader, int slotNumber)
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

			var destObject = dest.CreateInstance(initContext);

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
			return Expressor<ExpressionInfo<T>>.MethodExpressor(e => e.MapDataReaderToObject(null, null, 0));
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
			IDataReader      dataReader,
			Expression       expr,
			object[]         ps,
			int              counterIndex,
			Mapper<TElement> itemReader)
		{
			var count = MappingSchema.ConvertToInt32(dataReader[counterIndex]);
			return GetGroupJoinEnumerator(count, count == 0? default(TElement): itemReader(this, qc, dataReader, MappingSchema, expr, ps));
		}

		public MethodInfo GetGroupJoinEnumeratorMethodInfo<TElement>()
		{
			return Expressor<ExpressionInfo<T>>.MethodExpressor(e => e.GetGroupJoinEnumerator<TElement>(null, null, null, null, 0, null));
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
			QueryContext                        context,
			IDataReader                         dataReader,
			Expression                          expr,
			object[]                            ps,
			Mapper<TKey>                        keyReader,
			ExpressionInfo<TElement>            valueReader)
		{
			var db = context.GetDbManager();

			try
			{
				var key = keyReader(this, context, dataReader, MappingSchema, expr, ps);

				ps = ps == null ? new object[1] : ps.ToArray();
				ps[0] = key;

				var values = valueReader.GetIEnumerable(context, db.DbManager, valueReader.Expression, ps);

				return new Grouping<TKey, TElement>(key, Configuration.Linq.PreloadGroups ? values.ToList() : values);
			}
			finally
			{
				context.ReleaseDbManager(db);
			}
		}

		public MethodInfo GetGroupingMethodInfo<TKey,TElement>()
		{
			return Expressor<ExpressionInfo<T>>.MethodExpressor(e => e.GetGrouping<TKey,TElement>(null, null, null, null, null, null));
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

		public bool Compare(DataProviderBase dataProvider, MappingSchema mappingSchema, Expression expr)
		{
			return DataProvider == dataProvider && MappingSchema == mappingSchema && Compare(Expression, expr);
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

		bool Compare(Expression expr1, Expression expr2)
		{
			if (expr1 == expr2)
				return true;

			if (expr1 == null || expr2 == null || expr1.NodeType != expr2.NodeType || expr1.Type != expr2.Type)
				return false;

			switch (expr1.NodeType)
			{
				case ExpressionType.Add:
				case ExpressionType.AddChecked:
				case ExpressionType.And:
				case ExpressionType.AndAlso:
				case ExpressionType.ArrayIndex:
				case ExpressionType.Coalesce:
				case ExpressionType.Divide:
				case ExpressionType.Equal:
				case ExpressionType.ExclusiveOr:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.LeftShift:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
				case ExpressionType.Modulo:
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyChecked:
				case ExpressionType.NotEqual:
				case ExpressionType.Or:
				case ExpressionType.OrElse:
				case ExpressionType.Power:
				case ExpressionType.RightShift:
				case ExpressionType.Subtract:
				case ExpressionType.SubtractChecked:
					{
						var e1 = (BinaryExpression)expr1;
						var e2 = (BinaryExpression)expr2;
						return
							e1.Method == e2.Method &&
							Compare(e1.Conversion, e2.Conversion) &&
							Compare(e1.Left,       e2.Left) &&
							Compare(e1.Right,      e2.Right);
					}

				case ExpressionType.ArrayLength:
				case ExpressionType.Convert:
				case ExpressionType.ConvertChecked:
				case ExpressionType.Negate:
				case ExpressionType.NegateChecked:
				case ExpressionType.Not:
				case ExpressionType.Quote:
				case ExpressionType.TypeAs:
				case ExpressionType.UnaryPlus:
					{
						var e1 = (UnaryExpression)expr1;
						var e2 = (UnaryExpression)expr2;
						return e1.Method == e2.Method && Compare(e1.Operand, e2.Operand);
					}

				case ExpressionType.Call:
					{
						var e1 = (MethodCallExpression)expr1;
						var e2 = (MethodCallExpression)expr2;

						if (e1.Arguments.Count != e2.Arguments.Count || e1.Method != e2.Method)
							return false;

						if (_queryableAccessorDic.Count > 0)
						{
							Func<Expression,IQueryable> func;

							if (_queryableAccessorDic.TryGetValue(expr1, out func))
								return Compare(func(expr1).Expression, func(expr2).Expression);
						}

						if (!Compare(e1.Object, e2.Object))
							return false;

						for (var i = 0; i < e1.Arguments.Count; i++)
							if (!Compare(e1.Arguments[i], e2.Arguments[i]))
								return false;

						return true;
					}

				case ExpressionType.Conditional:
					{
						var e1 = (ConditionalExpression)expr1;
						var e2 = (ConditionalExpression)expr2;
						return Compare(e1.Test, e2.Test) && Compare(e1.IfTrue, e2.IfTrue) && Compare(e1.IfFalse, e2.IfFalse);
					}

				case ExpressionType.Constant:
					{
						var e1 = (ConstantExpression)expr1;
						var e2 = (ConstantExpression)expr2;

						return e1.Value == null && e2.Value == null || ExpressionParser<T>.IsConstant(e1.Type)? Equals(e1.Value, e2.Value): true;
					}

				case ExpressionType.Invoke:
					{
						var e1 = (InvocationExpression)expr1;
						var e2 = (InvocationExpression)expr2;

						if (e1.Arguments.Count != e2.Arguments.Count || !Compare(e1.Expression, e2.Expression))
							return false;

						for (var i = 0; i < e1.Arguments.Count; i++)
							if (!Compare(e1.Arguments[i], e2.Arguments[i]))
								return false;

						return true;
					}

				case ExpressionType.Lambda:
					{
						var e1 = (LambdaExpression)expr1;
						var e2 = (LambdaExpression)expr2;

						if (e1.Parameters.Count != e2.Parameters.Count || !Compare(e1.Body, e2.Body))
							return false;

						for (var i = 0; i < e1.Parameters.Count; i++)
							if (!Compare(e1.Parameters[i], e2.Parameters[i]))
								return false;

						return true;
					}

				case ExpressionType.ListInit:
					{
						var e1 = (ListInitExpression)expr1;
						var e2 = (ListInitExpression)expr2;

						if (e1.Initializers.Count != e2.Initializers.Count || !Compare(e1.NewExpression, e2.NewExpression))
							return false;

						for (var i = 0; i < e1.Initializers.Count; i++)
						{
							var i1 = e1.Initializers[i];
							var i2 = e2.Initializers[i];

							if (i1.Arguments.Count != i2.Arguments.Count || i1.AddMethod != i2.AddMethod)
								return false;

							for (var j = 0; j < i1.Arguments.Count; j++)
								if (!Compare(i1.Arguments[j], i2.Arguments[j]))
									return false;
						}

						return true;
					}

				case ExpressionType.MemberAccess:
					{
						var e1 = (MemberExpression)expr1;
						var e2 = (MemberExpression)expr2;

						if (e1.Member == e2.Member)
						{
							if (e1.Expression == e2.Expression)
							{
								if (_queryableAccessorDic.Count > 0)
								{
									Func<Expression,IQueryable> func;

									if (_queryableAccessorDic.TryGetValue(expr1, out func))
										return Compare(func(expr1).Expression, func(expr2).Expression);
								}
							}

							return Compare(e1.Expression, e2.Expression);
						}

						return false;
					}

				case ExpressionType.MemberInit:
					{
						var e1 = (MemberInitExpression)expr1;
						var e2 = (MemberInitExpression)expr2;

						if (e1.Bindings.Count != e2.Bindings.Count || !Compare(e1.NewExpression, e2.NewExpression))
							return false;

						Func<MemberBinding,MemberBinding,bool> compareBindings = null; compareBindings = (b1,b2) =>
						{
							if (b1 == b2)
								return true;

							if (b1 == null || b2 == null || b1.BindingType != b2.BindingType || b1.Member != b2.Member)
								return false;

							switch (b1.BindingType)
							{
								case MemberBindingType.Assignment:
									return Compare(((MemberAssignment)b1).Expression, ((MemberAssignment)b2).Expression);

								case MemberBindingType.ListBinding:
									var ml1 = (MemberListBinding)b1;
									var ml2 = (MemberListBinding)b2;

									if (ml1.Initializers.Count != ml2.Initializers.Count)
										return false;

									for (var i = 0; i < ml1.Initializers.Count; i++)
									{
										var ei1 = ml1.Initializers[i];
										var ei2 = ml2.Initializers[i];

										if (ei1.AddMethod != ei2.AddMethod || ei1.Arguments.Count != ei2.Arguments.Count)
											return false;

										for (var j = 0; j < ei1.Arguments.Count; j++)
											if (!Compare(ei1.Arguments[j], ei2.Arguments[j]))
												return false;
									}

									break;

								case MemberBindingType.MemberBinding:
									var mm1 = (MemberMemberBinding)b1;
									var mm2 = (MemberMemberBinding)b2;

									if (mm1.Bindings.Count != mm2.Bindings.Count)
										return false;

									for (var i = 0; i < mm1.Bindings.Count; i++)
										if (!compareBindings(mm1.Bindings[i], mm2.Bindings[i]))
											return false;

									break;
							}

							return true;
						};

						for (var i = 0; i < e1.Bindings.Count; i++)
						{
							var b1 = e1.Bindings[i];
							var b2 = e2.Bindings[i];

							if (!compareBindings(b1, b2))
								return false;
						}

						return true;
					}

				case ExpressionType.New:
					{
						var e1 = (NewExpression)expr1;
						var e2 = (NewExpression)expr2;

						if (e1.Arguments.Count != e2.Arguments.Count)
							return false;

						if (e1.Members == null && e2.Members != null)
							return false;

						if (e1.Members != null && e2.Members == null)
							return false;

						if (e1.Constructor != e2.Constructor)
							return false;

						if (e1.Members != null)
						{
							if (e1.Members.Count != e2.Members.Count)
								return false;

							for (var i = 0; i < e1.Members.Count; i++)
								if (e1.Members[i] != e2.Members[i])
									return false;
						}

						for (var i = 0; i < e1.Arguments.Count; i++)
							if (!Compare(e1.Arguments[i], e2.Arguments[i]))
								return false;

						return true;
					}

				case ExpressionType.NewArrayBounds:
				case ExpressionType.NewArrayInit:
					{
						var e1 = (NewArrayExpression)expr1;
						var e2 = (NewArrayExpression)expr2;

						if (e1.Expressions.Count != e2.Expressions.Count)
							return false;

						for (var i = 0; i < e1.Expressions.Count; i++)
							if (!Compare(e1.Expressions[i], e2.Expressions[i]))
								return false;

						return true;
					}

				case ExpressionType.Parameter:
					{
						var e1 = (ParameterExpression)expr1;
						var e2 = (ParameterExpression)expr2;
						return e1.Name == e2.Name;
					}

				case ExpressionType.TypeIs:
					{
						var e1 = (TypeBinaryExpression)expr1;
						var e2 = (TypeBinaryExpression)expr2;
						return e1.TypeOperand == e2.TypeOperand && Compare(e1.Expression, e2.Expression);
					}
			}

			throw new InvalidOperationException();
		}

		#endregion

		#region GetSqlText

		public string GetSqlText(DbManager db, Expression expr, object[] parameters, int idx)
		{
			string[]           command;
			IDbDataParameter[] parms;

			lock (this)
			{
				SetParameters(expr, parameters, idx);

				List<SqlParameter> ps;
				command = GetCommand(idx, out ps);
				parms   = GetParameters(db, idx, ps);
			}

			return GetSqlText(db, parms, command);
		}

		string GetSqlText(DbManager db, ICollection<IDbDataParameter> parms, IEnumerable<string> commands)
		{
			var sb = new StringBuilder();

			sb.Append("-- ").Append(db.ConfigurationString);

			if (db.ConfigurationString != DataProvider.Name)
				sb.Append(' ').Append(DataProvider.Name);

			if (DataProvider.Name != SqlProvider.Name)
				sb.Append(' ').Append(SqlProvider.Name);

			sb.AppendLine();

			if (parms != null && parms.Count > 0)
			{
				foreach (var p in parms)
					sb
						.Append("-- DECLARE ")
						.Append(p.ParameterName)
						.Append(' ')
						.Append(p.Value == null ? p.DbType.ToString() : p.Value.GetType().Name)
						.AppendLine();

				sb.AppendLine();

				foreach (var p in parms)
				{
					var value = p.Value;

					if (value is string || value is char)
						value = "'" + value.ToString().Replace("'", "''") + "'";

					sb
						.Append("-- SET ")
						.Append(p.ParameterName)
						.Append(" = ")
						.Append(value)
						.AppendLine();
				}

				sb.AppendLine();
			}

			foreach (var command in commands)
				sb.AppendLine(command);

			return sb.ToString();
		}

		#endregion

		#region Inner Types

		public delegate TE Mapper<TE>(ExpressionInfo<T> info, QueryContext qc, IDataReader rd, MappingSchema ms, Expression expr, object[] ps);

		public class Parameter
		{
			public Expression                                         Expression;
			public Func<ExpressionInfo<T>,Expression,object[],object> Accessor;
			public SqlParameter                                       SqlParameter;
		}

		public class QueryInfo
		{
			public SqlQuery        SqlQuery   = new SqlQuery();
			public List<Parameter> Parameters = new List<Parameter>();
			public Mapper<T>       Mapper;
			public string[]        CommandText;
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

		static ExpressionInfo<TR>.Parameter GetParameter<TR>(DbManager dataContext, SqlField field)
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
			{
				var ms = dataContext.MappingSchema;
				var tp = field.SystemType;
				param.SqlParameter.ValueConverter = o => ms.MapEnumToValue(o, tp);
			}

			return param;
		}

		#region Insert

		public static int Insert(DbManager dataContext, T obj)
		{
			if (Equals(default(T), obj))
				return 0;

			ExpressionInfo<int> ei;

			var key = new { dataContext.MappingSchema, dataContext.DataProvider };

			if (!ObjectOperation<T>.Insert.TryGetValue(key, out ei))
				lock (_sync)
					if (!ObjectOperation<T>.Insert.TryGetValue(key, out ei))
					{
						var sqlTable = new SqlTable<T>(dataContext.MappingSchema);
						var sqlQuery = new SqlQuery { QueryType = QueryType.Insert };

						sqlQuery.Set.Into = sqlTable;

						ei = new ExpressionInfo<int>
						{
							MappingSchema = dataContext.MappingSchema,
							DataProvider  = dataContext.DataProvider,
							Queries       = { new ExpressionInfo<int>.QueryInfo { SqlQuery = sqlQuery, } }
						};

						foreach (var field in sqlTable.Fields)
						{
							if (field.Value.IsInsertable)
							{
								var param = GetParameter<int>(dataContext, field.Value);

								ei.Queries[0].Parameters.Add(param);

								sqlQuery.Set.Items.Add(new SqlQuery.SetExpression(field.Value, param.SqlParameter));
							}
						}

						ei.SetNonQueryQuery();

						ObjectOperation<T>.Insert.Add(key, ei);
					}

			return (int)ei.GetElement(null, dataContext, Expression.Constant(obj), null);
		}

		#endregion

		#region InsertWithIdentity

		public static object InsertWithIdentity(DbManager dataContext, T obj)
		{
			if (Equals(default(T), obj))
				return 0;

			ExpressionInfo<object> ei;

			var key = new { dataContext.MappingSchema, dataContext.DataProvider };

			if (!ObjectOperation<T>.InsertWithIdentity.TryGetValue(key, out ei))
				lock (_sync)
					if (!ObjectOperation<T>.InsertWithIdentity.TryGetValue(key, out ei))
					{
						var sqlTable = new SqlTable<T>(dataContext.MappingSchema);
						var sqlQuery = new SqlQuery { QueryType = QueryType.Insert };

						sqlQuery.Set.Into         = sqlTable;
						sqlQuery.Set.WithIdentity = true;

						ei = new ExpressionInfo<object>
						{
							MappingSchema = dataContext.MappingSchema,
							DataProvider  = dataContext.DataProvider,
							Queries       = { new ExpressionInfo<object>.QueryInfo { SqlQuery = sqlQuery, } }
						};

						foreach (var field in sqlTable.Fields)
						{
							if (field.Value.IsInsertable)
							{
								var param = GetParameter<object>(dataContext, field.Value);

								ei.Queries[0].Parameters.Add(param);

								sqlQuery.Set.Items.Add(new SqlQuery.SetExpression(field.Value, param.SqlParameter));
							}
						}

						ei.SetScalarQuery<object>();

						ObjectOperation<T>.InsertWithIdentity.Add(key, ei);
					}

			return ei.GetElement(null, dataContext, Expression.Constant(obj), null);
		}

		#endregion

		#region Update

		public static int Update(DbManager dataContext, T obj)
		{
			if (Equals(default(T), obj))
				return 0;

			ExpressionInfo<int> ei;

			var key = new { dataContext.MappingSchema, dataContext.DataProvider };

			if (!ObjectOperation<T>.Update.TryGetValue(key, out ei))
				lock (_sync)
					if (!ObjectOperation<T>.Update.TryGetValue(key, out ei))
					{
						var sqlTable = new SqlTable<T>(dataContext.MappingSchema);
						var sqlQuery = new SqlQuery { QueryType = QueryType.Update };

						sqlQuery.From.Table(sqlTable);

						ei = new ExpressionInfo<int>
						{
							MappingSchema = dataContext.MappingSchema,
							DataProvider  = dataContext.DataProvider,
							Queries       = { new ExpressionInfo<int>.QueryInfo { SqlQuery = sqlQuery, } }
						};

						var keys   = sqlTable.GetKeyFields();
						var fields = sqlTable.Fields.Values.Where(f => f.IsUpdatable).Except(keys).ToList();

						if (fields.Count == 0)
							throw new LinqException(
								string.Format("There are no fields to update in the type '{0}'.", sqlTable.Name));

						foreach (var field in fields)
						{
							var param = GetParameter<int>(dataContext, field);

							ei.Queries[0].Parameters.Add(param);

							sqlQuery.Set.Items.Add(new SqlQuery.SetExpression(field, param.SqlParameter));
						}

						foreach (var field in keys)
						{
							var param = GetParameter<int>(dataContext, field);

							ei.Queries[0].Parameters.Add(param);

							sqlQuery.Where.Field(field).Equal.Expr(param.SqlParameter);
						}

						ei.SetNonQueryQuery();

						ObjectOperation<T>.Update.Add(key, ei);
					}

			return (int)ei.GetElement(null, dataContext, Expression.Constant(obj), null);
		}

		#endregion

		#region Delete

		public static int Delete(DbManager dataContext, T obj)
		{
			if (Equals(default(T), obj))
				return 0;

			ExpressionInfo<int> ei;

			var key = new { dataContext.MappingSchema, dataContext.DataProvider };

			if (!ObjectOperation<T>.Delete.TryGetValue(key, out ei))
				lock (_sync)
					if (!ObjectOperation<T>.Delete.TryGetValue(key, out ei))
					{
						var sqlTable = new SqlTable<T>(dataContext.MappingSchema);
						var sqlQuery = new SqlQuery { QueryType = QueryType.Delete };

						sqlQuery.From.Table(sqlTable);

						ei = new ExpressionInfo<int>
						{
							MappingSchema = dataContext.MappingSchema,
							DataProvider  = dataContext.DataProvider,
							Queries       = { new ExpressionInfo<int>.QueryInfo { SqlQuery = sqlQuery, } }
						};

						var keys = sqlTable.GetKeyFields();

						if (keys.Count == 0)
							throw new LinqException(
								string.Format("Table '{0}' does not have primary key.", sqlTable.Name));

						foreach (var field in keys)
						{
							var param = GetParameter<int>(dataContext, field);

							ei.Queries[0].Parameters.Add(param);

							sqlQuery.Where.Field(field).Equal.Expr(param.SqlParameter);
						}

						ei.SetNonQueryQuery();

						ObjectOperation<T>.Delete.Add(key, ei);
					}

			return (int)ei.GetElement(null, dataContext, Expression.Constant(obj), null);
		}

		#endregion


		#endregion
	}
}
