using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.Reflection;

namespace BLToolkit.Data.Linq
{
	using DataProvider;
	using Mapping;
	using Data.Sql;
	using Common;

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

		#region ElementQuery

		public void SetElementQuery<TE>(Mapper<TE> mapper)
		{
			foreach (var sql in Queries)
				sql.SqlBuilder.FinalizeAndValidate();

			if (Queries.Count != 1)
				throw new InvalidOperationException();

			SqlProvider.SqlBuilder = Queries[0].SqlBuilder;

			GetElement = (ctx, db, expr, ps) => Query(ctx, db, expr, ps, mapper);
		}

		TE Query<TE>(QueryContext ctx, DbManager db, Expression expr, object[] parameters, Mapper<TE> mapper)
		{
			var dispose = db == null;

			if (db == null)
				db = new DbManager();

			try
			{
				using (var dr = GetReader(db, expr, parameters, 0))
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

		public void SetQuery()
		{
			SetQuery(null);
		}

		public void SetQuery(Mapper<T> mapper)
		{
			Queries[0].Mapper = mapper;

			foreach (var sql in Queries)
				sql.SqlBuilder.FinalizeAndValidate();

			if (Queries.Count != 1)
				throw new InvalidOperationException();

			Func<DbManager,Expression,object[],int,IEnumerable<IDataReader>> query = Query;

			SqlProvider.SqlBuilder = Queries[0].SqlBuilder;

			var select = Queries[0].SqlBuilder.Select;

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

				GetIEnumerable = (_, db, expr, ps) => Map(query(db, expr, ps, 0), GetMapperSlot(index));
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
				using (var dr = GetReader(db, expr, parameters, queryNumber))
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
				yield return mapper(this, context, dr, MappingSchema, expr, ps);
		}

		IDataReader GetReader(DbManager db, Expression expr, object[] parameters, int idx)
		{
			SetParameters(expr, parameters, idx);

			string             command;
			IDbDataParameter[] parms;

			lock (this)
			{
				command = GetCommand(idx);
				parms   = GetParameters(db, idx);
			}

			db.SetCommand(command, parms);

			//string s = sql.ToString();

#if DEBUG
			var info = string.Format("{0} {1}\n", DataProvider.Name, db.ConfigurationString);

			if (parms != null && parms.Length > 0)
			{
				foreach (var p in parms)
					info += string.Format("DECLARE {0} {1}\n",
						p.ParameterName,
						p.Value == null ? p.DbType.ToString() : p.Value.GetType().Name);

				info += "\n";

				foreach (var p in parms)
				{
					var value = p.Value;

					if (value is string || value is char)
						value = "'" + value.ToString().Replace("'", "''") + "'";

					info += string.Format("SET {0} = {1}\n", p.ParameterName, value);
				}

				info += "\n";
			}

			info += command;

			Debug.WriteLineIf(DbManager.TraceSwitch.TraceInfo, info, DbManager.TraceSwitch.DisplayName);
#endif

			return db.ExecuteReader();
		}

		private void SetParameters(Expression expr, object[] parameters, int idx)
		{
			foreach (var p in Queries[idx].Parameters)
			{
				if (p.OriginalSqlParameter == null)
					p.OriginalSqlParameter = p.SqlParameter;

				p.OriginalSqlParameter.Value = p.Accessor(this, expr, parameters);
			}
		}

		private IDbDataParameter[] GetParameters(DbManager db, int idx)
		{
			var sql        = Queries[idx].SqlBuilder;
			var parameters = Queries[idx].Parameters;

			if (parameters.Count == 0 && sql.Parameters.Count == 0)
				return null;

			var x = db.DataProvider.Convert("x", ConvertType.NameToQueryParameter).ToString();
			var y = db.DataProvider.Convert("y", ConvertType.NameToQueryParameter).ToString();

			var parms = new IDbDataParameter[x == y? sql.Parameters.Count: parameters.Count];

			if (x == y)
			{
				for (var i = 0; i < parms.Length; i++)
				{
					var sqlp = sql.Parameters[i];
					var parm = parameters.Count > i && parameters[i].SqlParameter == sqlp ? parameters[i] : parameters.First(p => p.SqlParameter == sqlp);

					parms[i] = db.Parameter(x, parm.SqlParameter.Value);
				}
			}
			else
			{
				int i = 0, j = 0;

				for (; i < parms.Length; i++)
				{
					var parm = parameters[i];

					if (sql.Parameters.Contains(parm.SqlParameter))
					{
						var name = db.DataProvider.Convert(parm.SqlParameter.Name, ConvertType.NameToQueryParameter).ToString();
						parms[j++] = db.Parameter(name, parm.SqlParameter.Value);
					}
				}

				if (i > j)
				{
					var parms1 = new IDbDataParameter[j];
					Array.Copy(parms, parms1, j);
					parms = parms1;
				}
			}

			return parms;
		}

		string GetCommand(int idx)
		{
			var query = Queries[idx];

			if (query.CommandText != null)
				return query.CommandText;

			if (query.OriginalSql == null)
				query.OriginalSql = query.SqlBuilder;

			var sql = query.SqlBuilder;

			if (query.OriginalSql.ParameterDependent)
			{
				var dic = new Dictionary<ICloneableElement,ICloneableElement>();

				query.SqlBuilder = sql = (SqlBuilder)query.OriginalSql.Clone(dic, _ => true);

				foreach (var p in query.Parameters)
				{
					ICloneableElement sp;
					if (dic.TryGetValue(p.OriginalSqlParameter, out sp))
						p.SqlParameter = (SqlParameter)sp;
				}

				SqlProvider.UpdateParameters(sql);
			}

			var sb      = new StringBuilder();
			SqlProvider.BuildSql(sql, sb, 0, 0);
			var command = sb.ToString();

			if (!query.OriginalSql.ParameterDependent)
				query.CommandText = command;

			return command;
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
				ms[i].Map(source, dataReader, index[i], dest, destObject, i);

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

				return new Grouping<TKey, TElement>(key, Common.Configuration.Linq.PreloadGroups ? values.ToList() : values);
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
							if (_queryableAccessorDic.Count > 0)
							{
								Func<Expression,IQueryable> func;

								if (_queryableAccessorDic.TryGetValue(expr1, out func))
									return Compare(func(expr1).Expression, func(expr2).Expression);
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

						if (e1.Arguments.Count != e2.Arguments.Count ||
							(e1.Members == null && e2.Members == null ||
							 e1.Members != null && e2.Members != null &&
							 e1.Members.Count != e2.Members.Count) ||
							e1.Constructor     != e2.Constructor)
							return false;

						for (var i = 0; i < e1.Members.Count; i++)
							if (e1.Members[i] != e2.Members[i])
								return false;

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

		#region Inner Types

		public delegate TE Mapper<TE>(ExpressionInfo<T> info, QueryContext qc, IDataReader rd, MappingSchema ms, Expression expr, object[] ps);

		public class Parameter
		{
			public Expression                                         Expression;
			public Func<ExpressionInfo<T>,Expression,object[],object> Accessor;
			public SqlParameter                                       SqlParameter;
			public SqlParameter                                       OriginalSqlParameter;
		}

		public class QueryInfo
		{
			public SqlBuilder      SqlBuilder = new SqlBuilder();
			public List<Parameter> Parameters = new List<Parameter>();
			public Mapper<T>       Mapper;
			public SqlBuilder      OriginalSql;
			public string          CommandText;
		}

		#endregion
	}
}
