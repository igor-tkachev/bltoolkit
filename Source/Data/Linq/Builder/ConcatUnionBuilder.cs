using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;
	using Data.Sql;
	using Reflection;

	class ConcatUnionBuilder : MethodCallBuilder
	{
		#region Builder

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

			return new UnionContext(sequence1, sequence2, methodCall);
		}

		protected override SequenceConvertInfo Convert(
			ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo, ParameterExpression param)
		{
			return null;
		}

		#endregion

		#region Context

		sealed class UnionContext : SubQueryContext
		{
			public UnionContext(IBuildContext sequence1, IBuildContext sequence2, MethodCallExpression methodCall)
				: base(sequence1)
			{
				_methodCall = methodCall;

				_isObject =
					sequence1.IsExpression(null, 0, RequestFor.Object).Result ||
					sequence2.IsExpression(null, 0, RequestFor.Object).Result;

				if (_isObject)
				{
					var type = _methodCall.Method.GetGenericArguments()[0];
					_unionParameter = Expression.Parameter(type, "t");
				}

				Init(sequence1, sequence2);
			}

			readonly bool                          _isObject;
			readonly MethodCallExpression          _methodCall;
			readonly ParameterExpression           _unionParameter;
			readonly Dictionary<MemberInfo,Member> _members = new Dictionary<MemberInfo,Member>(new MemberInfoComparer());

			class Member
			{
				public SqlInfo          SequenceInfo;
				public SqlInfo          SqlQueryInfo;
				public MemberExpression MemberExpression;
			}

			void Init(IBuildContext sequence1, IBuildContext sequence2)
			{
				var idx1 = sequence1.ConvertToIndex(null, 0, ConvertFlags.All).OrderBy(_ => _.Index).ToList();

				if (_isObject)
				{
					foreach (var info in idx1)
					{
						if (info.Member == null)
							throw new InvalidOperationException();

						CheckAndAddMember(sequence1, sequence2, info);
					}

					var idx2 = sequence2.ConvertToIndex(null, 0, ConvertFlags.All).OrderBy(_ => _.Index).ToList();

					if (idx1.Count != idx2.Count)
					{
						for (var i = 0; i < idx2.Count; i++)
						{
							if (i < idx1.Count)
							{
								if (idx1[i].Index != idx2[i].Index)
									throw new InvalidOperationException();
							}
							else
							{
								CheckAndAddMember(sequence2, sequence1, idx2[i]);
							}
						}
					}
				}
				else
					sequence2.ConvertToIndex(null, 0, ConvertFlags.All).OrderBy(_ => _.Index).ToList();
			}

			void CheckAndAddMember(IBuildContext sequence1, IBuildContext sequence2, SqlInfo info)
			{
				var member = new Member
				{
					SequenceInfo     = info,
					MemberExpression = Expression.PropertyOrField(_unionParameter, info.Member.Name)
				};

				if (sequence1.IsExpression(member.MemberExpression, 1, RequestFor.Object).Result)
					throw new LinqException("Types in {0} are constructed incompatibly.", _methodCall.Method.Name);

				var idx = sequence2.ConvertToIndex(member.MemberExpression, 1, ConvertFlags.Field);

				if (idx[0].Index != member.SequenceInfo.Index)
					throw new LinqException("Types in {0} are constructed incompatibly.", _methodCall.Method.Name);

				_members.Add(member.MemberExpression.Member, member);
			}

			public override void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				var expr   = BuildExpression(null, 0);
				var mapper = Builder.BuildMapper<T>(expr);

				query.SetQuery(mapper.Compile());
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				if (_isObject)
				{
					if (expression == null)
					{
						var type  = _methodCall.Method.GetGenericArguments()[0];
						var nctor = (NewExpression)Expression.Find(e =>
						{
							if (e.NodeType == ExpressionType.New && e.Type == type)
							{
								var ne = (NewExpression)e;
								return ne.Arguments != null && ne.Arguments.Count > 0;
							}

							return false;
						});

						Expression expr;

						if (nctor != null)
						{
							var members = nctor.Members
								.Select(m => m is MethodInfo ? TypeHelper.GetPropertyByMethod((MethodInfo)m) : m)
								.ToList();

							expr = Expression.New(
								nctor.Constructor,
								members
									.Select(m => Expression.PropertyOrField(_unionParameter, m.Name))
									.Cast<Expression>(),
								members);
						}
						else
						{
							var ta = TypeAccessor.GetAccessor(type);

							expr = Expression.MemberInit(
								Expression.New(ta.Type),
								_members
									.Select(m => Expression.Bind(m.Value.MemberExpression.Member, m.Value.MemberExpression))
									.Cast<MemberBinding>());
						}

						var ex = Builder.BuildExpression(this, expr);

						return ex;
					}

					if (level == 0 || level == 1)
					{
						var levelExpression = expression.GetLevelExpression(1);

						if (expression == levelExpression)
						{
							var idx = ConvertToIndex(expression, level, ConvertFlags.Field);
							var n   = idx[0].Index;

							if (Parent != null)
								n = Parent.ConvertToParentIndex(n, this);

							return Builder.BuildSql(expression.Type, n);
						}
					}

					throw new InvalidOperationException();
				}

				return base.BuildExpression(expression, level);
			}

			public override IsExpressionResult IsExpression(Expression expression, int level, RequestFor testFlag)
			{
				if (testFlag == RequestFor.Root && expression == _unionParameter)
					return IsExpressionResult.True;

				return base.IsExpression(expression, level, testFlag);
			}

			public override SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				if (_isObject)
				{
					return ConvertToSql(expression, level, flags)
						.Select(idx =>
						{
							if (idx.Index < 0)
								idx.Index = SqlQuery.Select.Add(idx.Sql);
							return idx;
						})
						.ToArray();
				}

				return base.ConvertToIndex(expression, level, flags);
			}

			public override SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				if (_isObject)
				{
					switch (flags)
					{
						case ConvertFlags.All   :
						case ConvertFlags.Key   :

							if (expression == null)
							{
								return _members.Values
									.Select(m => ConvertToSql(m.MemberExpression, 0, ConvertFlags.Field)[0])
									.ToArray();
							}

							break;

						case ConvertFlags.Field :

							if (expression != null && (level == 0 || level == 1) && expression.NodeType == ExpressionType.MemberAccess)
							{
								var levelExpression = expression.GetLevelExpression(1);

								if (expression == levelExpression)
								{
									var ma     = (MemberExpression)expression;
									var member = _members[ma.Member];

									if (member.SqlQueryInfo == null)
									{
										member.SqlQueryInfo = new SqlInfo
										{
											Sql    = SubQuery.SqlQuery.Select.Columns[member.SequenceInfo.Index],
											Query  = SqlQuery,
											Member = member.MemberExpression.Member,
										};
									}

									return new[] { member.SqlQueryInfo };
								}
							}

							break;
					}

					throw new InvalidOperationException();
				}

				return base.ConvertToSql(expression, level, flags);
			}
		}

		#endregion
	}
}
