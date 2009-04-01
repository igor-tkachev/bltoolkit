using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace BLToolkit.Data.Linq
{
	using Mapping;
	using Sql;

	class ExpressionParser<T> : ReflectionHelper
	{
		public ExpressionParser()
		{
			_info.SqlBuilder = new SqlBuilder();
		}

		readonly ExpressionInfo<T>   _info            = new ExpressionInfo<T>();
		readonly ParameterExpression _expressionParam = Expression.Parameter(typeof(Expression),    "expr");
		readonly ParameterExpression _dataReaderParam = Expression.Parameter(typeof(IDataReader),   "rd");
		readonly ParameterExpression _mapSchemaParam  = Expression.Parameter(typeof(MappingSchema), "ms");

		public ExpressionInfo<T> Parse(MappingSchema mappingSchema, Expression expression)
		{
			_info.MappingSchema = mappingSchema;
			_info.Expression    = expression;

			ParseInfo.CreateRoot(expression, _expressionParam).Match(
				//
				// db.Table.ToList()
				//
				pi => pi.IsConstant<IQueryable>((value, _) => SimpleQuery(value.ElementType, null)),
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

		QueryInfo ParseSequence(ParseInfo<Expression> info)
		{
			QueryInfo select = null;

			if (info.IsConstant<IQueryable>((value, _) =>
				{
					var table = new SqlTable(_info.MappingSchema, value.ElementType);
					_info.SqlBuilder.From.Table(table);
					select = new QueryInfo.Constant(value.ElementType, table);
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

		#region Parse Select

		static QueryInfo ParseSelect(QueryInfo select, ParseInfo<ParameterExpression> parm, ParseInfo body)
		{
			SetAlias(select, parm.Expr.Name);

			switch (body.NodeType)
			{
				case ExpressionType.Parameter : return select;
				case ExpressionType.New       : return new QueryInfo.New       (select, parm, body.ConvertTo<NewExpression>());
				case ExpressionType.MemberInit: return new QueryInfo.MemberInit(select, parm, body.ConvertTo<MemberInitExpression>());
				default                       : throw  new InvalidOperationException();
			}
		}

		#endregion

		#region Where

		QueryInfo ParseWhere(QueryInfo select, ParseInfo<ParameterExpression> parm, ParseInfo body)
		{
			if (CheckForSubQuery(select, parm, body))
			{
				select = MakeSubQuery(select, parm, body);
			}

			return select;
		}

		private QueryInfo MakeSubQuery(QueryInfo select, ParseInfo<ParameterExpression> parm, ParseInfo info)
		{
			return select;
		}

		bool CheckForSubQuery(QueryInfo query, ParseInfo<ParameterExpression> parm, ParseInfo expr)
		{
			var makeSubquery = false;

			expr.Walk(pi =>
			{
				if (!makeSubquery)
					pi.IsMemberAccess((ma,ex) =>
					{
						if (IsParameter(ma.Expr, parm.Expr))
						{
							// field = p.FieldName
							//
							var member = ma.Expr.Member;

							if (member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Property)
								makeSubquery = CheckFieldForSubQuery(query, ma);

							return true;
						}

						return false;
					});

				return pi;
			});

			return makeSubquery;
		}

		public bool CheckFieldForSubQuery(QueryInfo queryInfo, ParseInfo<MemberExpression> memberExpr)
		{
			bool? makeQuery = null;
			var   member    = memberExpr.Expr.Member;

			FieldWalker(queryInfo, memberExpr,
				field              => makeQuery = false,
				(query,memExpr)    => makeQuery = CheckFieldForSubQuery(query, memExpr),
				(query,expr,parms) => makeQuery = true);

			if (makeQuery == null)
				throw new LinqException("Member '{0}.{1}' is not an SQL column.", member.ReflectedType, member.Name);

			return makeQuery.Value;
		}

		#endregion

		#region Build Select

		void BuildSelect(QueryInfo query)
		{
			query.Match
			(
				constantExpr => // QueryInfo.Constant
				{
					foreach (var field in constantExpr.Table.Fields.Values)
						_info.SqlBuilder.Select.Expr(field);

					_info.SetQuery();
				},
				newExpr    => BuildNew(query, newExpr.   Body, newExpr.   Parameter), // QueryInfo.New
				memberInit => BuildNew(query, memberInit.Body, memberInit.Parameter)  // QueryInfo.MemberInit
			);
		}

		void BuildNew(QueryInfo query, ParseInfo expr, ParseInfo<ParameterExpression> parm)
		{
			var info = BuildNewExpression(query, expr, parm);

			var mapper = Expression.Lambda<Func<IDataReader,MappingSchema,Expression,T>>(
				info, new [] { _dataReaderParam, _mapSchemaParam, _expressionParam });

			_info.SetQuery(mapper.Compile());
		}

		ParseInfo BuildNewExpression(QueryInfo query, ParseInfo expr, ParseInfo<ParameterExpression> parm)
		{
			return expr.Walk(pi =>
			{
				pi.IsMemberAccess((ma,ex) =>
				{
					if (IsParameter(ma.Expr, parm.Expr))
					{
						// field = p.FieldName
						//
						var member = ma.Expr.Member;

						if (member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Property)
						{
							var field = GetField(query, ma);

							if (field != null)
							{
								pi = field;
								return true;
							}
						}
					}
					else if (ex.IsConstant(c =>
					{
						// field = localVariable
						//
						var e = Expression.MakeMemberAccess(Expression.Convert(c.ParamAccessor, ex.Expr.Type), ma.Expr.Member);
						pi = pi.Parent.Replace(e, c.ParamAccessor);
						return true;
					}))
					{
						return true;
					}

					return false;
				});

				return pi;
			});
		}

		public ParseInfo GetField(QueryInfo queryInfo, ParseInfo<MemberExpression> memberExpr)
		{
			ParseInfo pi = null;

			var member = memberExpr.Expr.Member;

			FieldWalker(queryInfo, memberExpr,
				field =>
				{
					var idx = _info.SqlBuilder.Select.Add(field);

					var memberType = member.MemberType == MemberTypes.Field ?
						((FieldInfo)   member).FieldType :
						((PropertyInfo)member).PropertyType;

					MethodInfo mi;

					if (!MapSchema.Converters.TryGetValue(memberType, out mi))
						throw new LinqException("Cannot find converter for the '{0}' type.", memberType.FullName);

					pi = memberExpr.Parent.Replace(
						Expression.Call(_mapSchemaParam, mi,
							Expression.Call(_dataReaderParam, DataReader.GetValue,
								Expression.Constant(idx, typeof(int)))),
						memberExpr.ParamAccessor);
				},
				(query,memExpr)    => pi = GetField(query, memExpr),
				(query,expr,parms) =>
				{
					pi = BuildNewExpression(query,expr,parms);
					pi.IsReplaced = true;
				}
			);

			if (pi == null)
				throw new LinqException("Member '{0}.{1}' is not an SQL column.", member.ReflectedType, member.Name);

			return pi;
		}

		static void SetAlias(QueryInfo query, string alias)
		{
			query.Match
			(
				constantExpr =>
				{
					if (constantExpr.Table.Alias == null)
						constantExpr.Table.Alias = alias;
				},
				_ => {},
				__ => {}
			);
		}

		bool SimpleQuery(Type type, string alias)
		{
			var table = new SqlTable(_info.MappingSchema, type) { Alias = alias };

			_info.SqlBuilder
				.Select
					.Field(table.All)
				.From
					.Table(table);

			_info.SetQuery();

			return true;
		}

		#endregion

		#region Helpers

		public void FieldWalker(
			QueryInfo                   query,
			ParseInfo<MemberExpression> memberExpr,
			Action<SqlField>            processConst,
			Action<QueryInfo,ParseInfo<MemberExpression>> processNextSource,
			Action<QueryInfo,ParseInfo,ParseInfo<ParameterExpression>> processExpression)
		{
			var member = memberExpr.Expr.Member;

			query.Match
			(
				constantExpr =>
				{
					var field = constantExpr.Table[member.Name];
					if (field != null)
						processConst(field);
				},
				newExpr =>
				{
					if (IsParameter(memberExpr.Expr, newExpr.Parameter.Expr))
					{
						processNextSource(newExpr.SourceInfo, memberExpr);
					}
					else if (IsMember(memberExpr.Expr.Expression, newExpr.Body.Expr.Members))
					{
						processNextSource(newExpr.SourceInfo, memberExpr);
					}
					else
					{
						var body = newExpr.Body.Expr;
						var mem  = member;

						if (mem is PropertyInfo)
							mem = ((PropertyInfo)member).GetGetMethod();

						if (body.Members != null)
						{
							for (var i = 0; i < body.Members.Count; i++)
							{
								if (mem == body.Members[i])
								{
									var arg = body.Arguments[i];

									if (arg is MemberExpression)
									{
										processNextSource(
											newExpr.SourceInfo,
											newExpr.Body.Create((MemberExpression)arg, newExpr.Body.Index(body.Arguments, New.Arguments, i)));
									}
									else
									{
										processExpression(
											query,
											newExpr.Body.Create(arg, newExpr.Body.Index(body.Arguments, New.Arguments, i)),
											newExpr.Parameter);
									}

									return;
								}
							}
						}
					}
				},
				memberInit =>
				{
					if (IsParameter(memberExpr.Expr, memberInit.Parameter.Expr))
					{
						processNextSource(memberInit.SourceInfo, memberExpr);
					}
					else if (IsMember(memberExpr.Expr.Expression, memberInit.Members))
					{
						processNextSource(memberInit.SourceInfo, memberExpr);
					}
					else
					{
						var body = memberInit.Body.Expr;

						for (var i = 0; i < body.Bindings.Count; i++)
						{
							var binding = body.Bindings[i];

							if (member == binding.Member)
							{
								if (binding is MemberAssignment)
								{
									var ma = binding as MemberAssignment;

									var piBinding    = memberInit.Body.Create(ma.Expression, memberInit.Body.Index(body.Bindings, MemberInit.Bindings, i));
									var piAssign     = piBinding.      Create(ma.Expression, piBinding.ConvertExpressionTo<MemberAssignment>());
									var piExpression = piAssign.       Create(ma.Expression, piAssign.Property(MemberAssignmentBind.Expression));

									if (ma.Expression is MemberExpression)
									{
										processNextSource(
											memberInit.SourceInfo,
											piExpression.Create(ma.Expression as MemberExpression, piExpression.Convert<MemberExpression>()));
									}
									else
									{
										processExpression(query, piExpression, memberInit.Parameter);
									}

									return;
								}
							}
						}

					}
				}
			);
		}

		static bool IsParameter(MemberExpression ex, ParameterExpression parm)
		{
			if (ex.Expression == parm) return true;

			if (ex.Expression is MemberExpression)
				return IsParameter(ex.Expression as MemberExpression, parm);

			return false;
		}

		static bool IsMember(Expression ex, IEnumerable<MemberInfo> members)
		{
			var me = ex as MemberExpression;

			if (me != null)
			{
				var member = me.Member;
				if (member is PropertyInfo)
					member = ((PropertyInfo)member).GetGetMethod();

				foreach (var m in members)
					if (m == member)
						return true;

				return IsMember(me.Expression, members);
			}

			return false;
		}

		#endregion
	}
}
