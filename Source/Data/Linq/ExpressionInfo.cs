using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using BLToolkit.Reflection;

namespace BLToolkit.Data.Linq
{
	using DataProvider;
	using Mapping;
	using Data.Sql;

	class ExpressionInfo<T> : ReflectionHelper
	{
		public Expression        Expression;
		public DataProviderBase  DataProvider;
		public MappingSchema     MappingSchema;
		public SqlBuilder        SqlBuilder;
		public ExpressionInfo<T> Next;
		public List<Parameter>   Parameters = new List<Parameter>();

		public Func<DbManager,Expression,IEnumerable<T>> GetIEnumerable;

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
						info = new ExpressionParser<T>().Parse(dataProvider, mappingSchema, expr);
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

		#region Query

		internal void SetQuery()
		{
			SqlBuilder.FinalizeAndValidate();

			var index = new int[SqlBuilder.Select.Columns.Count];

			for (var i = 0; i < index.Length; i++)
				index[i] = i;

			GetIEnumerable = (db, expr) => Query(db, expr, GetMapperSlot(), index);
		}

		IEnumerable<T> Query(DbManager db, Expression expr, int slot, int[] index)
		{
			var dispose = db == null;
			if (db == null)
				db = new DbManager();

			try
			{
				using (var dr = GetReader(db, expr))
					while (dr.Read())
						yield return (T)MapDataReaderToObject(typeof(T), dr, slot, index);
			}
			finally
			{
				if (dispose)
					db.Dispose();
			}
		}

		internal void SetQuery(Func<ExpressionInfo<T>,IDataReader,MappingSchema,Expression,T> mapper)
		{
			SqlBuilder.FinalizeAndValidate();
			GetIEnumerable = (db, expr) => Query(db, expr, mapper);
		}

		IEnumerable<T> Query(DbManager db, Expression expr, Func<ExpressionInfo<T>,IDataReader,MappingSchema,Expression,T> mapper)
		{
			var dispose = db == null;
			if (db == null)
				db = new DbManager();

			try
			{
				using (var dr = GetReader(db, expr))
					while (dr.Read())
						yield return mapper(this, dr, MappingSchema, expr);
			}
			finally
			{
				if (dispose)
					db.Dispose();
			}
		}

		IDataReader GetReader(DbManager db, Expression expr)
		{
			SetParameters(expr);

			var command = GetCommand();
			var parms   = GetParameters(db, expr);

			//string s = sql.ToString();

#if DEBUG
			var info = string.Format("{0} {1}\n", DataProvider.Name, db.ConfigurationString);

			if (parms != null && parms.Length > 0)
			{
				foreach (var p in parms)
					info += string.Format("DECLARE {0} {1}\n", p.ParameterName, p.DbType);

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

			return db.SetCommand(command, parms).ExecuteReader();
		}

		private void SetParameters(Expression expr)
		{
			foreach (var p in Parameters)
				p.SqlParameter.Value = p.Accessor(expr);
		}

		private IDbDataParameter[] GetParameters(DbManager db, Expression expr)
		{
			if (Parameters.Count == 0 && SqlBuilder.Parameters.Count == 0)
				return null;

			var x = db.DataProvider.Convert("x", ConvertType.NameToQueryParameter).ToString();
			var y = db.DataProvider.Convert("y", ConvertType.NameToQueryParameter).ToString();

			var parms = new IDbDataParameter[x == y? SqlBuilder.Parameters.Count: Parameters.Count];

			if (x == y)
			{
				for (var i = 0; i < parms.Length; i++)
				{
					var sqlp = SqlBuilder.Parameters[i];
					var parm = Parameters.Count > i && Parameters[i].SqlParameter == sqlp ? Parameters[i] : Parameters.First(p => p.SqlParameter == sqlp);

					parms[i] = db.Parameter(x, parm.SqlParameter.Value);
				}
			}
			else
			{
				int i = 0, j = 0;

				for (; i < parms.Length; i++)
				{
					var parm = Parameters[i];

					if (SqlBuilder.Parameters.Contains(parm.SqlParameter))
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

		string _command;

		string GetCommand()
		{
			if (_command != null)
				return _command;

			var command = DataProvider.CreateSqlProvider().BuildSql(SqlBuilder, new StringBuilder(), 0).ToString();

			if (!SqlBuilder.ParameterDependent)
				_command = command;

			return command;
		}

		#endregion

		#region Mapping

		class MapperSlot
		{
			public ObjectMapper   ObjectMapper;
			public IValueMapper[] ValueMappers;
		}

		MapperSlot[] _mapperSlots;

		internal int GetMapperSlot()
		{
			if (_mapperSlots == null)
			{
				_mapperSlots = new [] { new MapperSlot() };
			}
			else
			{
				var slots = new MapperSlot[_mapperSlots.Length + 1];

				slots[_mapperSlots.Length] = new MapperSlot();

				_mapperSlots.CopyTo(slots, 0);
				_mapperSlots = slots;
			}

			return _mapperSlots.Length - 1;
		}

		protected object MapDataReaderToObject(Type destObjectType, IDataReader dataReader, int slotNumber, int[] index)
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

			var mappers = slot.ValueMappers ?? (slot.ValueMappers = initContext.MappingSchema.GetValueMappers(source, dest, index));

			MappingSchema.MapInternal(source, dataReader, dest, destObject, index, mappers);

			if (smDest != null)
				smDest.EndMapping(initContext);

			return destObject;
		}

		public MethodInfo GetMapperMethodInfo()
		{
			return Expressor<ExpressionInfo<T>>.MethodExpressor(e => e.MapDataReaderToObject(null, null, 0, null));
		}

		#endregion

		#region Compare

		public bool Compare(DataProviderBase dataProvider, MappingSchema mappingSchema, Expression expr)
		{
			return DataProvider == dataProvider && MappingSchema == mappingSchema && Compare(expr, Expression);
		}

		static bool Compare(Expression expr1, Expression expr2)
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

						if (e1.Arguments.Count != e2.Arguments.Count || e1.Method != e2.Method || !Compare(e1.Object, e2.Object))
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
						return e1.Member == e2.Member && Compare(e1.Expression, e2.Expression);
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

		public class Parameter
		{
			public Expression              Expression;
			public Func<Expression,object> Accessor;
			public SqlParameter            SqlParameter;
		}
	}
}
