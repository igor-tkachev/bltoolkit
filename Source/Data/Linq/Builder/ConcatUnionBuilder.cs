using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;
	using Data.Sql;

	class ConcatUnionBuilder : MethodCallBuilder
	{
		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.Arguments.Count == 2 && methodCall.IsQueryable("Concat", "Union");
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var sequence1 = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));
			var sequence2 = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[1], new SqlQuery()));
			var union     = new SqlQuery.Union(sequence2.SqlQuery, methodCall.Method.Name == "Concat");

			sequence1.SqlQuery.Unions.Add(union);

			return new UnionContext(sequence1, sequence2);
		}

		protected override SequenceConvertInfo Convert(
			ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo, ParameterExpression param)
		{
			return null;
		}

		sealed class UnionContext : SubQueryContext
		{
			public UnionContext(IBuildContext sequence1, IBuildContext sequence2)
				: base(sequence1)
			{
				_union = sequence2;
			}

			readonly IBuildContext _union;
			private  bool          _checkUnion;

			public override void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				var expr = BuildExpression(null, 0);

				var mapper = Expression.Lambda<Func<QueryContext,IDataContext,IDataReader,Expression,object[],T>>(
					expr, new []
					{
						ExpressionBuilder.ContextParam,
						ExpressionBuilder.DataContextParam,
						ExpressionBuilder.DataReaderParam,
						ExpressionBuilder.ExpressionParam,
						ExpressionBuilder.ParametersParam,
					});

				query.SetQuery(mapper.Compile());
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				/*
				if (SubQuery.IsExpression(expression, level, RequestFor.Object) ||
					Union.   IsExpression(expression, level, RequestFor.Object) )
				{
					var sidx   = SubQuery.ConvertToIndex(expression, level, ConvertFlags.All);
					var uidx   = Union.   ConvertToIndex(expression, level, ConvertFlags.All);
					var lambda = (LambdaExpression)Expression;
					var type   = lambda.Body.Type;
					var parm   = Expression.Parameter(type, "p");

					var nctor = (NewExpression)Expression.Find(e =>
					{
						if (e.NodeType == ExpressionType.New && e.Type == type)
						{
							var ne = (NewExpression)e;
							return ne.Arguments != null && ne.Arguments.Count > 0;
						}

						return false;
					});

					var expr = nctor != null ?
						Expression.New(
							nctor.Constructor,
							nctor.Members.Select(m => Expression.PropertyOrField(parm, m.Name)),
							nctor.Members) as Expression:
						Expression.MemberInit(
							Expression.New(type),
							sidx.Union(uidx)
								.Select(i => i.Member.Name)
								.Distinct()
								.Select(n =>
								{
									var m = Expression.PropertyOrField(parm, n);
									return Expression.Bind(m.Member, m);
								}));

					return Builder.BuildExpression(this, expr);
				}
				*/

				return base.BuildExpression(expression, level);
			}

			protected override int GetIndex(SqlQuery.Column column)
			{
				int idx;

				if (!ColumnIndexes.TryGetValue(column, out idx))
				{
					if (!_checkUnion)
					{
						_checkUnion = true;

						var subSql   = SubQuery.ConvertToIndex(null, 0, ConvertFlags.All).OrderBy(_ => _.Index).ToList();
						var unionSql = _union.  ConvertToIndex(null, 0, ConvertFlags.All).OrderBy(_ => _.Index).ToList();
						var sub      = SubQuery.SqlQuery.Select.Columns;
						var union    = _union.  SqlQuery.Select.Columns;

						for (var i = 0; i < sub.Count; i++)
						{
							if (i >= subSql.Count || subSql[i].Index != i)
							{
								if (i < subSql.Count && subSql[i].Index < i)
									throw new InvalidOperationException();
								subSql.Insert(i, new SqlInfo { Index = i, Sql = sub[i].Expression });
							}
						}

						for (var i = 0; i < union.Count; i++)
						{
							if (i >= unionSql.Count || unionSql[i].Index != i)
							{
								if (i < unionSql.Count && unionSql[i].Index < i)
									throw new InvalidOperationException();
								unionSql.Insert(i, new SqlInfo { Index = i, Sql = union[i].Expression });
							}
						}

						var reorder = false;

						for (var i = 0; i < subSql.Count && i < unionSql.Count; i++)
						{
							if (subSql[i].Member != unionSql[i].Member)
							{
								reorder = true;

								var sm = subSql[i].Member;

								if (sm != null)
								{
									var um = unionSql.Select((s,n) => new { s, n }).Where(_ => _.s.Member == sm).FirstOrDefault();

									if (um != null)
									{
										unionSql.RemoveAt(um.n);
										unionSql.Insert(i, um.s);
									}
									else
									{
										if (unionSql[i].Member != null)
											unionSql.Insert(i, new SqlInfo());
									}
								}
								else
								{
									if (unionSql[i].Member != null)
										unionSql.Insert(i, new SqlInfo());
								}
							}
						}

						if (reorder)
						{
							var cols = union.ToList();

							union.Clear();

							foreach (var info in unionSql)
							{
								if (info.Index < 0)
									union.Add(new SqlQuery.Column(_union.SqlQuery, new SqlValue(null)));
								else
									union.Add(cols[info.Index]);
							}
						}

						while (sub.Count < union.Count)
						{
							var type = (Type)null;//union[sub.Count].SystemType;

							var func = type == null ?
								new SqlValue(null) :
								Builder.Convert(
								this,
								new SqlFunction(type, "Convert", SqlDataType.GetDataType(type), new SqlValue(null)));

							sub.  Add(new SqlQuery.Column(SubQuery.SqlQuery, func));
						}

						while (union.Count < sub.Count)
						{
							var type = (Type)null;//sub[union.Count].SystemType;

							var func = type == null ?
								new SqlValue(null) :
								Builder.Convert(
								this,
								new SqlFunction(type, "Convert", SqlDataType.GetDataType(type), new SqlValue(null)));

							union.Add(new SqlQuery.Column(_union.   SqlQuery, func));
						}
					}

					idx = SqlQuery.Select.Add(column);
					ColumnIndexes.Add(column, idx);

					while (SubQuery.SqlQuery.Select.Columns.Count < _union.SqlQuery.Select.Columns.Count)
						_union.SqlQuery.Select.Columns.Add(new SqlQuery.Column(_union.SqlQuery, new SqlValue(null)));
				}

				return idx;
			}
		}
	}
}
