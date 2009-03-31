using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using BLToolkit.Mapping;

namespace BLToolkit.Data.Linq
{
	using Sql;

	class ExpressionParser<T>
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
				pi => pi.IsQueryableMethod("Where",  seq => select = ParseSequence(seq), (p, b) => ParseWhere(select, p, b)),

				pi => { throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'.", pi.Expr), "info"); }
			);

			return select;
		}

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

		void ParseWhere(QueryInfo select, ParseInfo<ParameterExpression> parm, ParseInfo<Expression> body)
		{
			throw  new NotImplementedException();
		}

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

		static bool IsParameter(MemberExpression ex, ParameterExpression parm)
		{
			if (ex.Expression == parm) return true;

			if (ex.Expression is MemberExpression)
				return IsParameter(ex.Expression as MemberExpression, parm);

			return false;
		}

		void BuildNew(QueryInfo query, ParseInfo expr, ParseInfo<ParameterExpression> parm)
		{
			var info = expr.Walk(pi =>
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

			var mapper = Expression.Lambda<Func<IDataReader,MappingSchema,Expression,T>>(
				info, new [] { _dataReaderParam, _mapSchemaParam, _expressionParam });

			_info.SetQuery(mapper.Compile());
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

		public ParseInfo GetField(QueryInfo query, ParseInfo<MemberExpression> memberExpr)
		{
			ParseInfo pi = null;

			var member = memberExpr.Expr.Member;

			query.Match
			(
				constantExpr =>
				{
					var field = constantExpr.Table[member.Name];

					if (field != null)
					{
						var idx = _info.SqlBuilder.Select.Add(field);

						var memberType = member.MemberType == MemberTypes.Field ?
							((FieldInfo)   member).FieldType :
							((PropertyInfo)member).PropertyType;

						MethodInfo mi;

						if (!ParseInfo.MappingSchema.Converters.TryGetValue(memberType, out mi))
							throw new LinqException("Cannot find converter for the '{0}' type.", memberType.FullName);

						pi = memberExpr.Parent.Replace(
							Expression.Call(_mapSchemaParam, mi,
								Expression.Call(_dataReaderParam, ParseInfo.DataReader.GetValue,
									Expression.Constant(idx, typeof(int)))),
							memberExpr.ParamAccessor);
					}
				},
				newExpr =>
				{
					if (IsParameter(memberExpr.Expr, newExpr.Parameter.Expr))
					{
						pi = GetField(newExpr.SourceInfo, memberExpr);
					}
					else if (IsMember(memberExpr.Expr.Expression, newExpr.Body.Expr.Members))
					{
						pi = GetField(newExpr.SourceInfo, memberExpr);
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
										pi = GetField(
											newExpr.SourceInfo,
											newExpr.Body.Create((MemberExpression)arg, newExpr.Body.Index(body.Arguments, ParseInfo.New.Arguments, i)));
										return;
									}

									throw new InvalidOperationException();
								}
							}
						}
					}
				},
				memberInit =>
				{
					if (IsParameter(memberExpr.Expr, memberInit.Parameter.Expr))
					{
						pi = GetField(memberInit.SourceInfo, memberExpr);
					}
					else if (IsMember(memberExpr.Expr.Expression, memberInit.Members))
					{
						pi = GetField(memberInit.SourceInfo, memberExpr);
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

									if (ma.Expression is MemberExpression)
									{
										var piBinding    = memberInit.Body.Create(ma.Expression, memberInit.Body.Index(body.Bindings, ParseInfo.MemberInit.Bindings, i));
										var piAssign     = piBinding.      Create(ma.Expression, piBinding.ConvertExpressionTo<MemberAssignment>());
										var piExpression = piAssign.       Create(ma.Expression, piAssign.Property(ParseInfo.MemberAssignmentBind.Expression));

										pi = GetField(
											memberInit.SourceInfo,
											piExpression.Create(ma.Expression as MemberExpression, piExpression.Convert<MemberExpression>()));
										return;
									}

									throw new InvalidOperationException();
								}
							}
						}

					}
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
	}
}
