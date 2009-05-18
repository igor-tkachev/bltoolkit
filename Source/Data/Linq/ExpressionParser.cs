using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BLToolkit.Data.Linq
{
	using DataProvider;
	using Mapping;
	using Reflection;
	using Data.Sql;
	using Data.Sql.SqlProvider;

	using IExpr = Data.Sql.ISqlExpression;

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
				pi => pi.IsLambda(0, body => { BuildScalarSelect(body); return true; }),
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

		QuerySource ParseSequence(ParseInfo info)
		{
			QuerySource select = null;

			if (TypeHelper.IsSameOrParent(typeof(IQueryable), info.Expr.Type))
				if (info.NodeType == ExpressionType.MemberAccess)
					info = GetIQueriable(info);

			if (info.IsConstant<IQueryable>((value,expr) =>
				{
					select = new QuerySource.Table(_info.MappingSchema, _info.SqlBuilder, new LambdaInfo(expr));
					return true;
				}))
				return select;

			if (info.NodeType != ExpressionType.Call)
				throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'.", info.Expr), "info");

			info.ConvertTo<MethodCallExpression>().Match
			(
				pi => pi.IsQueryableMethod("Select", seq => select = ParseSequence(seq), l => select = ParseSelect(l, select)),
				pi => pi.IsQueryableMethod("Where",      seq => select = ParseSequence(seq),  l      => select = ParseWhere     (select, l)),
				pi => pi.IsQueryableMethod("SelectMany", seq => select = ParseSequence(seq), (l1,l2) => select = ParseSelectMany(select, l1, l2)),
				pi => { throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'.", pi.Expr), "info"); }
			);

			return select;
		}

		ParseInfo GetIQueriable(ParseInfo info)
		{
			if (info.NodeType == ExpressionType.MemberAccess)
			{
				var p    = Expression.Parameter(typeof(Expression), "expr");
				var expr = ReplaceParameter(ParseInfo.CreateRoot(info.Expr, p), _ => {});
				var l    = Expression.Lambda<Func<Expression,IQueryable>>(Expression.Convert(expr, typeof(IQueryable)), new [] { p });
				var qe   = l.Compile();

				_info.QueryableAccessors.Add(info.Expr, qe);

				return info.Create(
					qe(info).Expression,
					Expression.Call(
						_infoParam,
						Expressor<ExpressionInfo<T>>.MethodExpressor(a => a.GetIQueryable(null, null)),
						new Expression[] { info.ParamAccessor, _expressionParam }));
			}

			throw new InvalidOperationException();
		}

		#endregion

		#region Parse Select

		QuerySource ParseSelect(LambdaInfo l, params QuerySource[] sources)
		{
			for (int i = 0; i < sources.Length && i < l.Parameters.Length; i++)
				SetAlias(sources[i], l.Parameters[i].Expr.Name);

			switch (l.Body.NodeType)
			{
				case ExpressionType.Parameter   : return sources[0];
				case ExpressionType.New         : return new QuerySource.Expr  (_info.SqlBuilder, l.ConvertTo<NewExpression>(),        sources);
				case ExpressionType.MemberInit  : return new QuerySource.Expr  (_info.SqlBuilder, l.ConvertTo<MemberInitExpression>(), sources);
				default                         : return new QuerySource.Scalar(_info.SqlBuilder, l,                                   sources);
			}
		}

		#endregion

		#region Parse SelectMany

		QuerySource ParseSelectMany(QuerySource select, LambdaInfo lambda1, LambdaInfo lambda2)
		{
			var sqlBuilder        = new SqlBuilder();
			var currentSqlBuilder = _info.SqlBuilder;

			_info.SqlBuilder = sqlBuilder;

			var source = ParseSequence(lambda1.Body);

			_info.SqlBuilder = currentSqlBuilder;
			_info.SqlBuilder.From.Table(sqlBuilder);

			return ParseSelect(lambda2, select, source);
		}

		#endregion

		#region Parse Where

		QuerySource ParseWhere(QuerySource select, LambdaInfo l)
		{
			SetAlias(select, l.Parameters[0].Expr.Name);

			if (CheckForSubQuery(select, l.Body))
			{
				var subQuery = new QuerySource.SubQuery(_info.SqlBuilder, select);

				_info.SqlBuilder = subQuery.SqlBuilder;

				select = subQuery;
			}

			ParseSearchCondition(_info.SqlBuilder.Where.SearchCondition.Conditions, select, l.Body);

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
				table    => { table.Select(this);   _info.SetQuery();  }, // QueryInfo.Table
				expr     => BuildNew(query, expr.Lambda.Body),            // QueryInfo.Expr
				BuildSubQuery,                                            // QueryInfo.SubQuery
				scalar   => BuildNew(query, scalar.Lambda.Body)           // QueryInfo.Scalar
			);
		}

		void BuildNew(QuerySource query, ParseInfo expr)
		{
			var info = BuildNewExpression(query, expr);

			var mapper = Expression.Lambda<Func<ExpressionInfo<T>,IDataReader,MappingSchema,Expression,T>>(
				info, new [] { _infoParam, _dataReaderParam, _mapSchemaParam, _expressionParam });

			_info.SetQuery(mapper.Compile());
		}

		void BuildSubQuery(QuerySource.SubQuery subQuery)
		{
			subQuery.ParentQueries[0].Match
			(
				table  => _info.SetQuery(), // QueryInfo.Table
				expr   => // QueryInfo.Expr
				{
					if (expr.Lambda.Body.Expr is NewExpression)
					{
						ParseInfo newExpr = null;
						var       member  = 0;

						var info = expr.Lambda.Body.Walk(pi =>
						{
							if (newExpr == null && pi.NodeType == ExpressionType.New)
							{
								newExpr = pi;
							}
							else if (newExpr != null)
							{
								var mi = ((NewExpression)newExpr.Expr).Members[member++];

								if (mi is MethodInfo)
									mi = TypeHelper.GetPropertyByMethod((MethodInfo)mi);

								var field = subQuery.Fields[mi];
								var idx   = field.Select(this);

								return BuildField(pi, idx, pi.Expr.Type);
							}

							return pi;
						});

						var mapper = Expression.Lambda<Func<ExpressionInfo<T>,IDataReader,MappingSchema,Expression,T>>(
							info, new [] { _infoParam, _dataReaderParam, _mapSchemaParam, _expressionParam });

						_info.SetQuery(mapper.Compile());
					}
					else
						throw new NotImplementedException();
				}, 
				sub    => { throw new NotImplementedException(); }, // QueryInfo.SubQuery
				scalar => { throw new NotImplementedException(); }  // QueryInfo.Scalar
			);
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

							if (IsServerSideOnly(pi))
								return BuildField(query, ma);

							var ex = pi.Create(ma.Expr.Expression, pi.Property(Member.Expression));

							if (query.ParentQueries.Length > 0)
							{
								QueryField field = null;

								foreach (var pq in query.ParentQueries)
								{
									field = pq.GetField(ma);
									if (field != null)
										break;
								}

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
							QueryField field = null;

							foreach (var pq in query.ParentQueries)
							{
								field = pq.GetField(pi.Expr);
								if (field != null)
									break;
							}

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
									return BuildNewExpression(ma, ma.Lambda.Body);
								}

								throw new InvalidOperationException();
							}

							break;
						}

					case ExpressionType.Constant:
						{
							if (query.ParentQueries.Length > 0)
							{
								QueryField field = null;

								foreach (var pq in query.ParentQueries)
								{
									field = pq.GetField(pi);
									if (field != null)
										break;
								}

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
						return BuildField(query.ParentQueries[0], pi);

					case ExpressionType.Call:
						{
							if (IsServerSideOnly(pi))
								return BuildField(query, pi);
						}

						break;

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

		void BuildScalarSelect(ParseInfo parseInfo)
		{
			switch (parseInfo.NodeType)
			{
				case ExpressionType.New:
				case ExpressionType.MemberInit:
					BuildNew(ParseSelect(new LambdaInfo(parseInfo)), parseInfo);
					return;
			}

			var expr = ParseExpression(null, parseInfo);

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
		readonly Dictionary<Expression,Expression>                  _accessors  = new Dictionary<Expression, Expression>();

		ExpressionInfo<T>.Parameter BuildParameter(ParseInfo expr)
		{
			ExpressionInfo<T>.Parameter p;

			if (_parameters.TryGetValue(expr.Expr, out p))
				return p;

			string name = null;

			var newExpr = ReplaceParameter(expr, nm => name = nm);
			var mapper  = Expression.Lambda<Func<Expression,object>>(Expression.Convert(newExpr, typeof(object)), new [] { _expressionParam });

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

		ParseInfo ReplaceParameter(ParseInfo expr, Action<string> setName)
		{
			return expr.Walk(pi =>
			{
				if (pi.NodeType == ExpressionType.MemberAccess)
				{
					Expression accessor;

					if (_accessors.TryGetValue(pi.Expr, out accessor))
					{
						var ma = (MemberExpression)pi.Expr;
						setName(ma.Member.Name);

						return pi.Parent.Replace(pi.Expr, accessor);
					}
				}

				pi.IsConstant(c =>
				{
					if (!TypeHelper.IsScalar(pi.Expr.Type))
					{
						var e = Expression.Convert(c.ParamAccessor, pi.Expr.Type);
						pi = pi.Parent.Replace(e, c.ParamAccessor);

						if (pi.Parent.NodeType == ExpressionType.MemberAccess)
						{
							var ma = (MemberExpression)pi.Parent.Expr;
							setName(ma.Member.Name);
						}
					}

					return true;
				});

				return pi;
			});
		}

		#endregion

		#region Expression Parser

		ISqlProvider _sqlProvider; 
		ISqlProvider  SqlProvider
		{
			get { return _sqlProvider ?? (_sqlProvider = _info.DataProvider.CreateSqlProvider()); }
		}

		ISqlExpression Convert(ISqlExpression expr)
		{
			return SqlProvider.ConvertExpression(expr);
		}

		ISqlPredicate Convert(ISqlPredicate predicate)
		{
			return SqlProvider.ConvertPredicate(predicate);
		}

		public ISqlExpression ParseExpression(QuerySource query, ParseInfo parseInfo)
		{
			if (parseInfo.NodeType == ExpressionType.Parameter && query is QuerySource.Scalar)
			{
				var ma = (QuerySource.Scalar)query;
				return ParseExpression(ma.ParentQueries[0], ma.Lambda.Body);
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
						var t  = e.Left.Type ?? e.Right.Type;

						switch (parseInfo.NodeType)
						{
							case ExpressionType.Add            :
							case ExpressionType.AddChecked     : return Convert(new SqlBinaryExpression(l, "+", r, t, Precedence.Additive));
							case ExpressionType.And            : return Convert(new SqlBinaryExpression(l, "&", r, t, Precedence.Bitwise));
							case ExpressionType.Divide         : return Convert(new SqlBinaryExpression(l, "/", r, t, Precedence.Multiplicative));
							case ExpressionType.ExclusiveOr    : return Convert(new SqlBinaryExpression(l, "^", r, t, Precedence.Bitwise));
							case ExpressionType.Modulo         : return Convert(new SqlBinaryExpression(l, "%", r, t, Precedence.Multiplicative));
							case ExpressionType.Multiply       : return Convert(new SqlBinaryExpression(l, "*", r, t, Precedence.Multiplicative));
							case ExpressionType.Or             : return Convert(new SqlBinaryExpression(l, "|", r, t, Precedence.Bitwise));
							case ExpressionType.Power          : return Convert(new SqlFunction("Power", l, r));
							case ExpressionType.Subtract       :
							case ExpressionType.SubtractChecked: return Convert(new SqlBinaryExpression(l, "-", r, t, Precedence.Subtraction));
							case ExpressionType.Coalesce       :
								{
									if (r is SqlFunction)
									{
										var c = (SqlFunction)r;

										if (c.Name == "Coalesce")
										{
											var parms = new ISqlExpression[c.Parameters.Length + 1];

											parms[0] = l;
											c.Parameters.CopyTo(parms, 1);

											return Convert(new SqlFunction("Coalesce", parms));
										}
									}

									return Convert(new SqlFunction("Coalesce", l, r));
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
						var ma = (MemberExpression)parseInfo.Expr;
						var ef = SqlProvider.ConvertMember(ma.Member);

						if (ef != null)
						{
							var pi = parseInfo.ConvertTo<MemberExpression>();

							var pie = parseInfo.Parent.Replace(ef, null).Walk(wpi =>
							{
								if (wpi.NodeType == ExpressionType.Parameter)
								{
									var expr = ma.Expression;

									if (expr.NodeType == ExpressionType.MemberAccess)
										if (!_accessors.ContainsKey(expr))
											_accessors.Add(expr, pi.Property(Member.Expression));

									return pi.Create(expr, null);
								}

								return wpi;
							});

							return ParseExpression(query, pie);
						}

						var attrs = ma.Member.GetCustomAttributes(typeof(SqlPropertyAttribute), true);

						if (attrs.Length > 0)
						{
							var attr = (SqlPropertyAttribute)attrs[0];
							return Convert(new SqlExpression(attr.Name ?? ma.Member.Name));
						}

						var field = query.GetField(parseInfo.Expr);

						if (field != null)
							return field.GetExpression(this);

						break;
					}

				case ExpressionType.Call:
					{
						var pi = parseInfo.Convert<MethodCallExpression>();
						var e  = parseInfo.Expr as MethodCallExpression;

						var ef = SqlProvider.ConvertMember(e.Method);

						if (ef != null)
						{
							var pie = parseInfo.Parent.Replace(ef, null).Walk(wpi =>
							{
								if (wpi.NodeType == ExpressionType.Parameter)
								{
									Expression       expr;
									Func<Expression> fparam;

									var pe = (ParameterExpression)wpi.Expr;

									if (pe.Name == "obj")
									{
										expr   = e.Object;
										fparam = () => pi.Property(MethodCall.Object);
									}
									else
									{
										var i  = int.Parse(pe.Name.Substring(1));
										expr   = e.Arguments[i];
										fparam = () => pi.Index(e.Arguments, MethodCall.Arguments, i);
									}

									if (expr.NodeType == ExpressionType.MemberAccess)
										if (!_accessors.ContainsKey(expr))
											_accessors.Add(expr, fparam());

									return pi.Create(expr, null);
								}

								return wpi;
							});

							return ParseExpression(query, pie);
						}

						var attrs = e.Method.GetCustomAttributes(typeof(SqlFunctionAttribute), true);

						if (attrs.Length > 0)
						{
							var attr = (SqlFunctionAttribute)attrs[0];

							if (attr is SqlPropertyAttribute)
								return Convert(new SqlExpression(attr.Name ?? e.Method.Name));

							var parms = new List<ISqlExpression>();

							if (e.Object != null)
								parms.Add(ParseExpression(query, pi.Create(e.Object, pi.Property(MethodCall.Object))));

							for (var i = 0; i < e.Arguments.Count; i++)
								parms.Add(ParseExpression(query, pi.Create(e.Arguments[i], pi.Index(e.Arguments, MethodCall.Arguments, i))));

							return Convert(new SqlFunction(attr.Name ?? e.Method.Name, parms.ToArray()));
						}

						break;
					}
			}

			throw new LinqException("'{0}' cannot be converted to SQL.", parseInfo.Expr);
		}

		bool IsServerSideOnly(ParseInfo parseInfo)
		{
			bool isServerSideOnly = false;

			parseInfo.Walk(pi =>
			{
				if (isServerSideOnly)
					return pi;

				switch (pi.NodeType)
				{
					case ExpressionType.MemberAccess:
						{
							var ma = (MemberExpression)pi.Expr;
							var ef = SqlProvider.ConvertMember(ma.Member);

							if (ef != null)
							{
								isServerSideOnly = IsServerSideOnly(pi.Parent.Replace(ef, null));
								break;
							}

							var attrs = ma.Member.GetCustomAttributes(typeof(SqlFunctionAttribute), true);

							isServerSideOnly = attrs.Length > 0 && ((SqlFunctionAttribute)attrs[0]).ServerSideOnly;
						}

						break;

					case ExpressionType.Call:
						{
							var e  = pi.Expr as MethodCallExpression;
							var ef = SqlProvider.ConvertMember(e.Method);

							if (ef != null)
							{
								isServerSideOnly = IsServerSideOnly(pi.Parent.Replace(ef, null));
								break;
							}

							var attrs = e.Method.GetCustomAttributes(typeof(SqlFunctionAttribute), true);

							isServerSideOnly = attrs.Length > 0 && ((SqlFunctionAttribute)attrs[0]).ServerSideOnly;
						}

						break;
				}

				return pi;
			});

			return isServerSideOnly;
		}

		static bool CanBeConstant(ParseInfo expr)
		{
			var canbe = true;

			expr.Walk(pi =>
			{
				var ex = pi.Expr;

				if (ex is BinaryExpression || ex is UnaryExpression || ex.NodeType == ExpressionType.Convert)
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

							if (IsConstant(mc.Method.DeclaringType) || mc.Method.DeclaringType == typeof(object))
								return pi;

							var attrs = mc.Method.GetCustomAttributes(typeof(SqlFunctionAttribute), true);

							if (attrs.Length > 0 && !((SqlFunctionAttribute)attrs[0]).ServerSideOnly)
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

		bool CanBeCompiled(ParseInfo expr)
		{
			var canbe = true;

			expr.Walk(pi =>
			{
				if (canbe)
				{
					canbe = !IsServerSideOnly(pi);

					if (canbe) switch (pi.NodeType)
					{
						case ExpressionType.Parameter:
							canbe = false;
							break;

						case ExpressionType.MemberAccess:
							{
								var ma    = (MemberExpression)pi.Expr;
								var attrs = ma.Member.GetCustomAttributes(typeof(SqlFunctionAttribute), true);

								canbe = attrs.Length == 0  || !((SqlFunctionAttribute)attrs[0]).ServerSideOnly;
								break;
							}

						case ExpressionType.Call:
							{
								var mc    = (MethodCallExpression)pi.Expr;
								var attrs = mc.Method.GetCustomAttributes(typeof(SqlFunctionAttribute), true);

								canbe = attrs.Length == 0  || !((SqlFunctionAttribute)attrs[0]).ServerSideOnly;
								break;
							}
					}
				}

				pi.StopWalking = !canbe;

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

						if (notCondition.Conditions.Count == 1 && notCondition.Conditions[0].Predicate is SqlBuilder.Predicate.NotExpr)
						{
							var p = notCondition.Conditions[0].Predicate as SqlBuilder.Predicate.NotExpr;
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
