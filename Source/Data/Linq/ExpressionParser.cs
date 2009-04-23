using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Sql.SqlProvider;

namespace BLToolkit.Data.Linq
{
	using Mapping;
	using Reflection;
	using Sql;

	class ExpressionParser<T> : ReflectionHelper
	{
		#region Init

		public ExpressionParser()
		{
			_info.SqlBuilder = new SqlBuilder();
		}

		readonly ExpressionInfo<T>   _info            = new ExpressionInfo<T>();
		readonly ParameterExpression _expressionParam = Expression.Parameter(typeof(Expression),        "expr");
		readonly ParameterExpression _dataReaderParam = Expression.Parameter(typeof(IDataReader),       "rd");
		readonly ParameterExpression _mapSchemaParam  = Expression.Parameter(typeof(MappingSchema),     "ms");
		readonly ParameterExpression _infoParam       = Expression.Parameter(typeof(ExpressionInfo<T>), "info");

		#endregion

		#region Parse

		public ExpressionInfo<T> Parse(DataProviderBase dataProvider, MappingSchema mappingSchema, Expression expression)
		{
			_info.DataProvider  = dataProvider;
			_info.MappingSchema = mappingSchema;
			_info.Expression    = expression;

			ParseInfo.CreateRoot(expression, _expressionParam).Match(
				//
				// db.Select(() => ...)
				//
				pi => pi.IsLambda(0, body => { BuildScalarSelect(null, body); return true; }),
				//
				// db.Table.ToList()
				//
				pi => pi.IsConstant<IQueryable>((value,_) => SimpleQuery(value.ElementType, null)),
				//
				// from p in db.Table select p
				// db.Table.Select(p => p)
				//
				pi => pi.IsMethod(typeof(Queryable), "Select",
					obj => obj.IsConstant<IQueryable>(),
					arg => arg.IsLambda<T>(
						body => body.NodeType == ExpressionType.Parameter,
						l    => SimpleQuery(typeof(T), l.Expr.Parameters[0].Name))),
				//
				// everything else
				//
				pi =>
				{
					BuildSelect(ParseSequence(pi));
					return true;
				}
			);

			return _info;
		}

		QuerySource ParseSequence(ParseInfo<Expression> info)
		{
			QuerySource select = null;

			if (info.IsConstant<IQueryable>((value,expr) =>
				{
					select = new QuerySource.Table(_info.MappingSchema, _info.SqlBuilder, expr);
					return true;
				}))
				return select;

			if (info.NodeType != ExpressionType.Call)
				throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'.", info.Expr), "info");

			info.ConvertTo<MethodCallExpression>().Match
			(
				pi => pi.IsQueryableMethod("Select", seq => select = ParseSequence(seq), (p, b) => select = ParseSelect(select, p, b)),
				pi => pi.IsQueryableMethod("Where",  seq => select = ParseSequence(seq), (p, b) => select = ParseWhere (select, p, b)),

				pi => { throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'.", pi.Expr), "info"); }
			);

			return select;
		}

		#endregion

		#region Parse Select

		QuerySource ParseSelect(QuerySource select, ParseInfo<ParameterExpression> parm, ParseInfo body)
		{
			if (select != null)
				SetAlias(select, parm.Expr.Name);

			switch (body.NodeType)
			{
				case ExpressionType.Parameter   : return select;
				case ExpressionType.New         : return new QuerySource.Expr  (_info.SqlBuilder, select, body.ConvertTo<NewExpression>());
				case ExpressionType.MemberInit  : return new QuerySource.Expr  (_info.SqlBuilder, select, body.ConvertTo<MemberInitExpression>());
				default                         : return new QuerySource.Scalar(_info.SqlBuilder, select, body);
			}
		}

		#endregion

		#region Parse Where

		QuerySource ParseWhere(QuerySource select, ParseInfo<ParameterExpression> parm, ParseInfo body)
		{
			SetAlias(select, parm.Expr.Name);

			if (CheckForSubQuery(select, body))
			{
				var subQuery = new QuerySource.SubQuery(_info.SqlBuilder, select);

				_info.SqlBuilder = subQuery.SqlBuilder;

				select = subQuery;
			}

			ParseSearchCondition(_info.SqlBuilder.Where.SearchCondition.Conditions, select, body);

			return select;
		}

		static bool CheckForSubQuery(QuerySource query, ParseInfo expr)
		{
			var makeSubquery = false;

			expr.Walk(pi =>
			{
				if (pi.NodeType == ExpressionType.MemberAccess)
				{
					var field = query.GetField(pi.Expr);

					if (field is QueryField.ExprColumn)
						makeSubquery = pi.StopWalking = true;
				}

				return pi;
			});

			return makeSubquery;
		}

		#endregion

		#region Build Select

		void BuildSelect(QuerySource query)
		{
			query.Match
			(
				table    => { table.Select(this); _info.SetQuery(); }, // QueryInfo.Table
				expr     => BuildNew(query, expr.Expression),          // QueryInfo.Expr
				subQuery => _info.SetQuery(),                          // QueryInfo.SubQuery
				scalar   => BuildNew(query, scalar.Expression)         // QueryInfo.Scalar
			);
		}

		void BuildNew(QuerySource query, ParseInfo expr)
		{
			var info = BuildNewExpression(query, expr);

			var mapper = Expression.Lambda<Func<ExpressionInfo<T>,IDataReader,MappingSchema,Expression,T>>(
				info, new [] { _infoParam, _dataReaderParam, _mapSchemaParam, _expressionParam });

			_info.SetQuery(mapper.Compile());
		}

		ParseInfo BuildNewExpression(QuerySource query, ParseInfo expr)
		{
			return expr.Walk(pi =>
			{
				switch (pi.NodeType)
				{
					case ExpressionType.MemberAccess:
						{
							var ma = pi.ConvertTo<MemberExpression>();
							var ex = pi.Create(((MemberExpression)pi.Expr).Expression, pi.Property(Member.Expression));

							if (query.ParentQuery != null)
							{
								var field = query.ParentQuery.GetField(ma);

								if (field != null)
								{
									if (field is QueryField.Column || field is QueryField.SubQueryColumn)
										return BuildField(ma, field);

									if (field is QueryField.ExprColumn)
									{
										var col = (QueryField.ExprColumn)field;

										pi = BuildNewExpression(col.QuerySource, col.Expr);
										pi.IsReplaced = pi.StopWalking = true;

										return pi;
									}

									if (field is QuerySource.Table)
									{
										var table = (QuerySource.Table) field;
										var index = table.Select(this).Select(i => Expression.Constant(i, typeof(int)) as Expression);

										return ma.Parent.Replace(
											Expression.Convert(
												Expression.Call(_infoParam, _info.GetMapperMethodInfo(),
													Expression.Constant(table.ObjectType, typeof(Type)),
													_dataReaderParam,
													Expression.Constant(_info.GetMapperSlot(), typeof(int)),
													Expression.NewArrayInit(typeof(int), index)),
												table.ObjectType),
											ma.ParamAccessor);
									}

									throw new InvalidOperationException();
								}

								if (query is QuerySource.Scalar && ex.NodeType == ExpressionType.Constant)
									return BuildField(query, ma);
							}
							else
							{
								var field = query.GetField(ma);

								if (field != null)
									return BuildField(ma, field);
							}

							if (ex.NodeType == ExpressionType.Constant)
							{
								// field = localVariable
								//
								var c = ex.Parent.Create((ConstantExpression)ex.Expr, ex.Property<ConstantExpression>(Constant.Value));

								return pi.Parent.Replace(
									Expression.MakeMemberAccess(
										Expression.Convert(c.ParamAccessor, ex.Expr.Type),
										ma.Expr.Member),
									c.ParamAccessor);
							}

							break;
						}

					case ExpressionType.Parameter:
						{
							var field = query.ParentQuery.GetField(pi.Expr);

							if (field != null)
							{
								if (field is QuerySource.Table)
								{
									var table = (QuerySource.Table)field;
									var index = table.Select(this).Select(i => Expression.Constant(i, typeof(int)) as Expression);

									return pi.Parent.Replace(
										Expression.Convert(
											Expression.Call(_infoParam, _info.GetMapperMethodInfo(),
												Expression.Constant(table.ObjectType, typeof(Type)),
												_dataReaderParam,
												Expression.Constant(_info.GetMapperSlot(), typeof(int)),
												Expression.NewArrayInit(typeof(int), index)),
											table.ObjectType),
										pi.ParamAccessor);
								}

								if (field is QuerySource.Scalar)
								{
									var ma = (QuerySource.Scalar)field;
									return BuildNewExpression(ma, ma.Expression);
								}

								throw new InvalidOperationException();
							}

							break;
						}

					case ExpressionType.Constant:
						{
							if (query.ParentQuery == null)
							{
								var field = query.GetField(pi);

								if (field != null)
								{
									var idx = field.Select(this);
									return BuildField(pi, idx, pi.Expr.Type);
								}
							}

							if (query is QuerySource.Scalar)
								return BuildField(query, pi);

							break;
						}

					case ExpressionType.Coalesce:
					//case ExpressionType.Conditional:
						return BuildField(query.ParentQuery, pi);
				}

				return pi;
			});
		}

		ParseInfo BuildField(QuerySource query, ParseInfo pi)
		{
			var sqlex = ParseExpression(query, pi);
			var idx   = _info.SqlBuilder.Select.Add(sqlex);

			return BuildField(pi, new[] { idx }, pi.Expr.Type);
		}

		ParseInfo BuildField(ParseInfo<MemberExpression> ma, QueryField field)
		{
			var memberType = ma.Expr.Member.MemberType == MemberTypes.Field ?
				((FieldInfo)   ma.Expr.Member).FieldType :
				((PropertyInfo)ma.Expr.Member).PropertyType;

			var idx = field.Select(this);

			return BuildField(ma, idx, memberType);
		}

		private ParseInfo BuildField(ParseInfo ma, int[] idx, Type memberType)
		{
			MethodInfo mi;

			if (!MapSchema.Converters.TryGetValue(memberType, out mi))
				throw new LinqException("Cannot find converter for the '{0}' type.", memberType.FullName);

			return ma.Parent.Replace(
				Expression.Call(_mapSchemaParam, mi,
					Expression.Call(_dataReaderParam, DataReader.GetValue,
						Expression.Constant(idx[0], typeof(int)))),
				ma.ParamAccessor);
		}

		static void SetAlias(QuerySource query, string alias)
		{
			query.Match
			(
				table  =>
				{
					if (table.SqlTable.Alias == null)
						table.SqlTable.Alias = alias;
				},
				_ => {},
				subQuery =>
				{
					var table = subQuery.SqlBuilder.From.Tables[0];
					if (table.Alias == null)
						table.Alias = alias;
				},
				_ => {}
			);
		}

		bool SimpleQuery(Type type, string alias)
		{
			var table = new SqlTable(_info.MappingSchema, type) { Alias = alias };

			foreach (var field in table.Fields.Values)
				_info.SqlBuilder.Select.Expr(field);

			_info.SqlBuilder.From.Table(table);
			_info.SetQuery();

			return true;
		}

		#endregion

		#region Build Scalar Select

		void BuildScalarSelect(QuerySource query, ParseInfo parseInfo)
		{
			switch (parseInfo.NodeType)
			{
				case ExpressionType.New:
				case ExpressionType.MemberInit:
					BuildNew(ParseSelect(query, null, parseInfo), parseInfo);
					return;
			}

			var expr = ParseExpression(query, parseInfo);

			_info.SqlBuilder.Select.Expr(expr);

			var pi = BuildField(parseInfo, new[] { 0 }, parseInfo.Expr.Type);

			var mapper = Expression.Lambda<Func<ExpressionInfo<T>,IDataReader,MappingSchema,Expression,T>>(
				pi, new [] { _infoParam, _dataReaderParam, _mapSchemaParam, _expressionParam });

			_info.SetQuery(mapper.Compile());
		}

		#endregion

		#region Build Constant

		readonly Dictionary<Expression,SqlValue> _constants = new Dictionary<Expression,SqlValue>();

		SqlValue BuildConstant(ParseInfo expr)
		{
			SqlValue value;

			if (_constants.TryGetValue(expr.Expr, out value))
				return value;

			var lambda = Expression.Lambda<Func<object>>(Expression.Convert(expr,typeof(object)));

			value = new SqlValue(lambda.Compile()());

			_constants.Add(expr.Expr, value);

			return value;
		}

		#endregion

		#region Build Parameter

		readonly Dictionary<Expression,ExpressionInfo<T>.Parameter> _parameters = new Dictionary<Expression, ExpressionInfo<T>.Parameter>();

		ExpressionInfo<T>.Parameter BuildParameter(ParseInfo expr)
		{
			ExpressionInfo<T>.Parameter p;

			if (_parameters.TryGetValue(expr.Expr, out p))
				return p;

			string name = null;

			var newExpr = expr.Walk(pi =>
			{
				pi.IsConstant(c =>
				{
					if (!TypeHelper.IsScalar(pi.Expr.Type))
					{
						var e = Expression.Convert(c.ParamAccessor, pi.Expr.Type);
						pi = pi.Parent.Replace(e, c.ParamAccessor);

						if (pi.Parent.NodeType == ExpressionType.MemberAccess)
						{
							var ma = (MemberExpression)pi.Parent.Expr;
							name = ma.Member.Name;
						}
					}

					return true;
				});

				return pi;
			});

			var mapper = Expression.Lambda<Func<Expression,object>>(Expression.Convert(newExpr, typeof(object)), new [] { _expressionParam });

			p = new ExpressionInfo<T>.Parameter
			{
				Expression   = expr.Expr,
				Accessor     = mapper.Compile(),
				SqlParameter = new SqlParameter(name, null)
			};

			_parameters.Add(expr.Expr, p);
			_info.Parameters.Add(p);

			return p;
		}

		#endregion

		#region Expression Parser

		ISqlProvider _sqlProvider; 

		ISqlExpression Convert(ISqlExpression expr)
		{
			return (_sqlProvider ?? (_sqlProvider = _info.DataProvider.CreateSqlProvider())).ConvertExpression(expr);
		}

		ISqlPredicate Convert(ISqlPredicate predicate)
		{
			return (_sqlProvider ?? (_sqlProvider = _info.DataProvider.CreateSqlProvider())).ConvertPredicate(predicate);
		}

		public ISqlExpression ParseExpression(QuerySource query, ParseInfo parseInfo)
		{
			if (parseInfo.NodeType == ExpressionType.Parameter && query is QuerySource.Scalar)
			{
				var ma = (QuerySource.Scalar)query;
				return ParseExpression(ma.ParentQuery, ma.Expression);
			}

			if (CanBeConstant(parseInfo))
				return BuildConstant(parseInfo);

			if (CanBeCompiled(parseInfo))
				return BuildParameter(parseInfo).SqlParameter;

			switch (parseInfo.NodeType)
			{
				case ExpressionType.AndAlso:
				case ExpressionType.OrElse:
				case ExpressionType.Not:
				case ExpressionType.Equal:
				case ExpressionType.NotEqual:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
					{
						var condition = new SqlBuilder.SearchCondition();
						ParseSearchCondition(condition.Conditions, query, parseInfo);
						return condition;
					}

				case ExpressionType.Add:
				case ExpressionType.AddChecked:
				case ExpressionType.And:
				case ExpressionType.Divide:
				case ExpressionType.ExclusiveOr:
				case ExpressionType.Modulo:
				case ExpressionType.Multiply:
				case ExpressionType.Or:
				case ExpressionType.Power:
				case ExpressionType.Subtract:
				case ExpressionType.SubtractChecked:
				case ExpressionType.Coalesce:
					{
						var pi = parseInfo.Convert<BinaryExpression>();
						var e  = parseInfo.Expr as BinaryExpression;
						var l  = ParseExpression(query, pi.Create(e.Left,  pi.Property(Binary.Left)));
						var r  = ParseExpression(query, pi.Create(e.Right, pi.Property(Binary.Right)));

						switch (parseInfo.NodeType)
						{
							case ExpressionType.Add            :
							case ExpressionType.AddChecked     : return Convert(new SqlBinaryExpression(l, "+", r, Precedence.Additive));
							case ExpressionType.And            : return Convert(new SqlBinaryExpression(l, "&", r, Precedence.Bitwise));
							case ExpressionType.Divide         : return Convert(new SqlBinaryExpression(l, "/", r, Precedence.Multiplicative));
							case ExpressionType.ExclusiveOr    : return Convert(new SqlBinaryExpression(l, "^", r, Precedence.Bitwise));
							case ExpressionType.Modulo         : return Convert(new SqlBinaryExpression(l, "%", r, Precedence.Multiplicative));
							case ExpressionType.Multiply       : return Convert(new SqlBinaryExpression(l, "*", r, Precedence.Multiplicative));
							case ExpressionType.Or             : return Convert(new SqlBinaryExpression(l, "|", r, Precedence.Bitwise));
							case ExpressionType.Power          : return Convert(new SqlFunction("POWER", l, r));
							case ExpressionType.Subtract       :
							case ExpressionType.SubtractChecked: return Convert(new SqlBinaryExpression(l, "-", r, Precedence.Subtraction));
							case ExpressionType.Coalesce       :
								{
									if (r is SqlFunction)
									{
										var c = (SqlFunction)r;

										if (c.Name == "COALESCE")
										{
											var parms = new ISqlExpression[c.Parameters.Length + 1];

											parms[0] = l;
											c.Parameters.CopyTo(parms, 1);

											return Convert(new SqlFunction("COALESCE", parms));
										}
									}

									return Convert(new SqlFunction("COALESCE", l, r));
								}
						}

						break;
					}

				case ExpressionType.Conditional:
					{
						var pi = parseInfo.Convert<ConditionalExpression>();
						var e  = parseInfo.Expr as ConditionalExpression;
						var s  = ParseExpression(query, pi.Create(e.Test,    pi.Property(Conditional.Test)));
						var t  = ParseExpression(query, pi.Create(e.IfTrue,  pi.Property(Conditional.IfTrue)));
						var f  = ParseExpression(query, pi.Create(e.IfFalse, pi.Property(Conditional.IfFalse)));

						if (f is SqlFunction)
						{
							var c = (SqlFunction)f;

							if (c.Name == "CASE")
							{
								var parms = new ISqlExpression[c.Parameters.Length + 2];

								parms[0] = s;
								parms[1] = t;
								c.Parameters.CopyTo(parms, 2);

								return Convert(new SqlFunction("CASE", parms));
							}
						}

						return Convert(new SqlFunction("CASE", s, t, f));
					}

				case ExpressionType.MemberAccess:
					{
						var    ma = (MemberExpression)parseInfo.Expr;
						string name;

						if (Functions.Member.TryGetValue(ma.Member, out name))
						{
							var pi = parseInfo.ConvertTo<MemberExpression>();
							var ex = pi.Create(pi.Expr.Expression, pi.Property(Member.Expression));
							return Convert(new SqlFunction(name, (ParseExpression(query, ex))));
						}

						var field = query.GetField(parseInfo.Expr);

						if (field != null)
							return field.GetExpression(this);

						break;
					}

				case ExpressionType.Call:
					{
						var    e  = parseInfo.Expr as MethodCallExpression;
						string name;

						if (Functions.Member.TryGetValue(e.Method, out name))
						{
							var pi = parseInfo.Convert<MethodCallExpression>();

							var parms = new List<ISqlExpression>();

							if (e.Object != null)
								parms.Add(ParseExpression(query, pi.Create(e.Object, pi.Property(MethodCall.Object))));

							for (var i = 0; i < e.Arguments.Count; i++)
								parms.Add(ParseExpression(query, pi.Create(e.Arguments[i], pi.Index(e.Arguments, MethodCall.Arguments, i))));

							return Convert(new SqlFunction(name, parms.ToArray()));
						}

						break;
					}
			}

			throw new LinqException("'{0}' cannot be converted to SQL.", parseInfo.Expr);
		}

		static bool CanBeConstant(ParseInfo expr)
		{
			var canbe = true;

			expr.Walk(pi =>
			{
				var ex = pi.Expr;

				if (ex is BinaryExpression || ex is UnaryExpression)
					return pi;

				switch (ex.NodeType)
				{
					case ExpressionType.Constant:
						{
							var c = (ConstantExpression)ex;

							if (c.Value == null || IsConstant(ex.Type))
								return pi;

							break;
						}

					case ExpressionType.MemberAccess:
						{
							var ma = (MemberExpression)ex;

							if (IsConstant(ma.Member.DeclaringType))
								return pi;

							break;
						}

					case ExpressionType.Call:
						{
							var mc = (MethodCallExpression)ex;

							if (IsConstant(mc.Method.DeclaringType))
								return pi;

							break;
						}
				}

				canbe = false;
				pi.StopWalking = true;

				return pi;
			});

			return canbe;
		}

		static bool CanBeCompiled(ParseInfo expr)
		{
			var canbe = true;

			expr.Walk(pi =>
			{
				switch (pi.NodeType)
				{
					case ExpressionType.Parameter:
						canbe = false;
						pi.StopWalking = true;
						break;
				}

				return pi;
			});

			return canbe;
		}

		public static bool IsConstant(Type type)
		{
			return type == typeof(int) || type == typeof(string) || type == typeof(char) || type == typeof(long);
		}

		#endregion

		#region Predicate Parser

		ISqlPredicate ParsePredicate(QuerySource query, ParseInfo parseInfo)
		{
			switch (parseInfo.NodeType)
			{
				case ExpressionType.Equal:
				case ExpressionType.NotEqual:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
					{
						var pi = parseInfo.Convert<BinaryExpression>();
						var e  = parseInfo.Expr as BinaryExpression;
						var l  = ParseExpression(query, pi.Create(e.Left,  pi.Property(Binary.Left)));
						var r  = ParseExpression(query, pi.Create(e.Right, pi.Property(Binary.Right)));

						SqlBuilder.Predicate.Operator op;

						switch (parseInfo.NodeType)
						{
							case ExpressionType.Equal             : op = SqlBuilder.Predicate.Operator.Equal;          break;
							case ExpressionType.NotEqual          : op = SqlBuilder.Predicate.Operator.NotEqual;       break;
							case ExpressionType.GreaterThan       : op = SqlBuilder.Predicate.Operator.Greater;        break;
							case ExpressionType.GreaterThanOrEqual: op = SqlBuilder.Predicate.Operator.GreaterOrEqual; break;
							case ExpressionType.LessThan          : op = SqlBuilder.Predicate.Operator.Less;           break;
							case ExpressionType.LessThanOrEqual   : op = SqlBuilder.Predicate.Operator.LessOrEqual;    break;
							default: throw new InvalidOperationException();
						}

						return Convert(new SqlBuilder.Predicate.ExprExpr(l, op, r));
					}

				case ExpressionType.Call:
					{
						var pi = parseInfo.Convert<MethodCallExpression>();
						var e  = pi.Expr as MethodCallExpression;

						ISqlPredicate predicate = null;

						if      (e.Method == Functions.String.Contains)   predicate = BuildLikePredicate(query, pi, "%", "%");
						else if (e.Method == Functions.String.StartsWith) predicate = BuildLikePredicate(query, pi, "",  "%");
						else if (e.Method == Functions.String.EndsWith)   predicate = BuildLikePredicate(query, pi, "%", "");
						else if (e.Method == Functions.String.Like11)     predicate = BuildLikePredicate(query, pi);
						else if (e.Method == Functions.String.Like12)     predicate = BuildLikePredicate(query, pi);
						else if (e.Method == Functions.String.Like21)     predicate = BuildLikePredicate(query, pi);
						else if (e.Method == Functions.String.Like22)     predicate = BuildLikePredicate(query, pi);

						if (predicate != null)
							return Convert(predicate);

						break;
					}
			}

			throw new InvalidOperationException();
		}

		#region LIKE predicate

		private ISqlPredicate BuildLikePredicate(QuerySource query, ParseInfo pi, string start, string end)
		{
			var e  = pi.Expr as MethodCallExpression;

			var o = ParseExpression(query, pi.Create(e.Object,       pi.Property(MethodCall.Object)));
			var a = ParseExpression(query, pi.Create(e.Arguments[0], pi.Index(e.Arguments, MethodCall.Arguments, 0)));

			if (a is SqlValue)
			{
				var value = ((SqlValue)a).Value;

				if (value == null)
					throw new LinqException("NULL cannot be used as a LIKE predicate parameter.");

				return value.ToString().IndexOfAny(new[] { '%', '_' }) < 0?
					new SqlBuilder.Predicate.Like(o, false, new SqlValue(start + value + end), null):
					new SqlBuilder.Predicate.Like(o, false, new SqlValue(start + EscapeLikeText(value.ToString()) + end), new SqlValue('~'));
			}

			if (a is SqlParameter)
			{
				var p  = (SqlParameter)a;
				var ep = (from pm in _info.Parameters where pm.SqlParameter == p select pm).First();

				ep = new ExpressionInfo<T>.Parameter
				{
					Expression   = ep.Expression,
					Accessor     = ep.Accessor,
					SqlParameter = new SqlParameter(p.Name, p.Value, GetLikeEscaper(start, end))
				};

				_parameters.Add(e, ep);
				_info.Parameters.Add(ep);

				return new SqlBuilder.Predicate.Like(o, false, ep.SqlParameter, new SqlValue('~'));
			}

			return null;
		}

		private ISqlPredicate BuildLikePredicate(QuerySource query, ParseInfo pi)
		{
			var e  = pi.Expr as MethodCallExpression;
			var a1 = ParseExpression(query, pi.Create(e.Arguments[0], pi.Index(e.Arguments, MethodCall.Arguments, 0)));
			var a2 = ParseExpression(query, pi.Create(e.Arguments[1], pi.Index(e.Arguments, MethodCall.Arguments, 1)));

			ISqlExpression a3 = null;

			if (e.Arguments.Count == 3)
				a3 = ParseExpression(query, pi.Create(e.Arguments[2], pi.Index(e.Arguments, MethodCall.Arguments, 2)));

			return new SqlBuilder.Predicate.Like(a1, false, a2, a3);
		}

		static string EscapeLikeText(string text)
		{
			if (text.IndexOfAny(new[] { '%', '_' }) < 0)
				return text;

			var builder = new StringBuilder(text.Length);

			foreach (var ch in text)
			{
				switch (ch)
				{
					case '%':
					case '_':
					case '~':
						builder.Append('~');
						break;
				}

				builder.Append(ch);
			}

			return builder.ToString();
		}

		static Converter<object,object> GetLikeEscaper(string start, string end)
		{
			return value => value == null? null: start + EscapeLikeText(value.ToString()) + end;
		}

		#endregion

		#endregion

		#region Search Condition Parser

		void ParseSearchCondition(ICollection<SqlBuilder.Condition> conditions, QuerySource query, ParseInfo parseInfo)
		{
			switch (parseInfo.NodeType)
			{
				case ExpressionType.AndAlso:
					{
						var pi = parseInfo.Convert<BinaryExpression>();
						var e  = parseInfo.Expr as BinaryExpression;

						ParseSearchCondition(conditions, query, pi.Create(e.Left,  pi.Property(Binary.Left)));
						ParseSearchCondition(conditions, query, pi.Create(e.Right, pi.Property(Binary.Right)));

						return;
					}

				case ExpressionType.OrElse:
					{
						var pi = parseInfo.Convert<BinaryExpression>();
						var e  = parseInfo.Expr as BinaryExpression;

						var orCondition = new SqlBuilder.SearchCondition();

						ParseSearchCondition(orCondition.Conditions, query, pi.Create(e.Left,  pi.Property(Binary.Left)));
						ParseSearchCondition(orCondition.Conditions, query, pi.Create(e.Right, pi.Property(Binary.Right)));

						orCondition.Conditions[0].IsOr = true;

						conditions.Add(new SqlBuilder.Condition(false, orCondition));

						return;
					}

				case ExpressionType.Not:
					{
						var pi = parseInfo.Convert<UnaryExpression>();
						var e  = parseInfo.Expr as UnaryExpression;

						var notCondition = new SqlBuilder.SearchCondition();

						ParseSearchCondition(notCondition.Conditions, query, pi.Create(e.Operand, pi.Property(Unary.Operand)));

						if (notCondition.Conditions.Count == 1 && notCondition.Conditions[0].Predicate is SqlBuilder.Predicate.NotExprBase)
						{
							var p = notCondition.Conditions[0].Predicate as SqlBuilder.Predicate.NotExprBase;
							p.IsNot = !p.IsNot;
							conditions.Add(notCondition.Conditions[0]);
						}
						else
							conditions.Add(new SqlBuilder.Condition(true, notCondition));

						return;
					}
			}

			var predicate = ParsePredicate(query, parseInfo);
			conditions.Add(new SqlBuilder.Condition(false, predicate));
		}

		#endregion
	}
}
