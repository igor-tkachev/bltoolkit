using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;
	using Reflection;

	// This class implements double functionality (scalar and member type selects)
	// and could be implemented as two different classes.
	// But the class means to have a lot of inheritors, and functionality of the inheritors
	// will be doubled as well. So lets double it once here.
	//
	public class SelectContext : IParseContext
	{
		#region Init

		public IParseContext[]  Sequence { get; set; }
		public LambdaExpression Lambda   { get; set; }
		public Expression       Body     { get; set; }
		public ExpressionParser Parser   { get; private set; }
		public SqlQuery         SqlQuery { get; set; }
		public IParseContext    Parent   { get; set; }
		public bool             IsScalar { get; private set; }

		Expression IParseContext.Expression { get { return Lambda; } }

		readonly Dictionary<MemberInfo,Expression> _members = new Dictionary<MemberInfo,Expression>();

		public SelectContext(LambdaExpression lambda, params IParseContext[] sequences)
		{
			Sequence = sequences;
			Parser   = sequences[0].Parser;
			Lambda   = lambda;
			Body     = lambda.Body.Unwrap();
			SqlQuery = sequences[0].SqlQuery;

			foreach (var context in Sequence)
				context.Parent = this;

			switch (Body.NodeType)
			{
				// .Select(p => new { ... })
				//
				case ExpressionType.New        :
					{
						var expr = (NewExpression)Body;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
						if (expr.Members == null)
							return;
// ReSharper restore HeuristicUnreachableCode
// ReSharper restore ConditionIsAlwaysTrueOrFalse

						for (var i = 0; i < expr.Members.Count; i++)
						{
							var member = expr.Members[i];

							_members.Add(member, expr.Arguments[i]);

							if (member is MethodInfo)
								_members.Add(TypeHelper.GetPropertyByMethod((MethodInfo)member), expr.Arguments[i]);
						}

						break;
					}

				// .Select(p => new MyObject { ... })
				//
				case ExpressionType.MemberInit :
					{
						var expr = (MemberInitExpression)Body;

						foreach (var binding in expr.Bindings)
						{
							if (binding is MemberAssignment)
							{
								var ma = (MemberAssignment)binding;

								_members.Add(binding.Member, ma.Expression);

								if (binding.Member is MethodInfo)
									_members.Add(TypeHelper.GetPropertyByMethod((MethodInfo)binding.Member), ma.Expression);
							}
							else
								throw new InvalidOperationException();
						}

						break;
					}

				// .Select(p => everything else)
				//
				default                        :
					IsScalar = true;
					break;
			}
		}

		#endregion

		#region BuildQuery

		public virtual void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
		{
			var expr = BuildExpression(null, 0);

			var mapper = Expression.Lambda<Func<QueryContext,IDataContext,IDataReader,Expression,object[],T>>(
				expr, new []
				{
					ExpressionParser.ContextParam,
					ExpressionParser.DataContextParam,
					ExpressionParser.DataReaderParam,
					ExpressionParser.ExpressionParam,
					ExpressionParser.ParametersParam,
				});

			query.SetQuery(mapper.Compile());
		}

		#endregion

		#region BuildExpression

		public virtual Expression BuildExpression(Expression expression, int level)
		{
			if (expression == null)
				return Parser.BuildExpression(this, Body);

			var levelExpression = expression.GetLevelExpression(level);

			if (IsScalar)
			{
				if (Body.NodeType == ExpressionType.Parameter)
				{
					if (level == 0)
					{
						var sequence = GetSequence(Body, 0);

						return expression == Body
						       	? sequence.BuildExpression(null, 0)
						       	: sequence.BuildExpression(expression, 1);
					}

					levelExpression = expression.GetLevelExpression(level - 1);

					var parseExpression = GetParseExpression(expression, levelExpression, Body);

					return BuildExpression(parseExpression, 0);
				}

				if (level == 0)
				{
					if (levelExpression != expression)
						return GetSequence(expression, level).BuildExpression(expression, level + 1);

					if (IsSubQuery() && IsExpression(null, 0, RequestFor.Expression))
					{
						var idx = ConvertToIndex(expression, level, ConvertFlags.Field).Single();

						idx = Parent == null ? idx : Parent.ConvertToParentIndex(idx, this);

						return Parser.BuildSql(expression.Type, idx);
					}

					return GetSequence(expression, level).BuildExpression(null, 0);
				}

				var root = Body.GetRootObject();

				if (root.NodeType == ExpressionType.Parameter)
				{
					levelExpression = expression.GetLevelExpression(level - 1);
					var parseExpression = GetParseExpression(expression, levelExpression, Body);

					return BuildExpression(parseExpression, 0);
				}

				//if (levelExpression != expression)
				//	return GetSequence(expression, level).BuildExpression(expression, level + 1);
			}
			else
			{
				var sequence  = GetSequence(expression, level);
				var parameter = Lambda.Parameters[Sequence.Length == 0 ? 0 : Array.IndexOf(Sequence, sequence)];

				if (level == 0)
					return levelExpression == expression ?
						sequence.BuildExpression(null,       0) :
						sequence.BuildExpression(expression, level + 1);

				switch (levelExpression.NodeType)
				{
					case ExpressionType.MemberAccess :
						{
							var memberExpression = _members[((MemberExpression)levelExpression).Member];

							if (levelExpression == expression)
							{
								if (IsSubQuery() &&
									!sequence.IsExpression(memberExpression, 0, RequestFor.Query) &&
									!sequence.IsExpression(memberExpression, 0, RequestFor.Field))
								{
									var idx = ConvertToIndex(expression, level, ConvertFlags.Field).Single();

									idx = Parent == null ? idx : Parent.ConvertToParentIndex(idx, this);

									return Parser.BuildSql(expression.Type, idx);
								}

								return Parser.BuildExpression(this, memberExpression);
							}

							if (memberExpression == parameter)
								return sequence.BuildExpression(expression, level + 1);

							var expr = expression.Convert(ex => ex == levelExpression ? memberExpression : ex);

							return sequence.BuildExpression(expr, 1);
						}

					case ExpressionType.Parameter :

						//if (levelExpression == expression)
							break;
						//return Sequence.BuildExpression(expression, level + 1);
				}
			}

			throw new NotImplementedException();
		}

		#endregion

		#region ConvertToSql

		readonly Dictionary<MemberInfo,ISqlExpression[]> _sql = new Dictionary<MemberInfo,ISqlExpression[]>();

		public virtual ISqlExpression[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
		{
			if (IsScalar)
			{
				switch (flags)
				{
					case ConvertFlags.Field :
					case ConvertFlags.Key   :
					case ConvertFlags.All   :
						{
							if (expression == null)
								return ConvertToSql(Body, 0, flags);

							if (Body.NodeType == ExpressionType.Parameter)
							{
								if (level == 0)
								{
									var sequence = GetSequence(Body, 0);

									return expression == Body ?
										sequence.ConvertToSql(null,       0, flags) :
										sequence.ConvertToSql(expression, 1, flags);
								}

								var levelExpression = expression.GetLevelExpression(level - 1);
								var parseExpression = GetParseExpression(expression, levelExpression, Body);

								return ConvertToSql(parseExpression, 0, flags);
							}

							if (level == 0)
							{
								var levelExpression = expression.GetLevelExpression(level);

								if (levelExpression != expression)
									return GetSequence(expression, level).ConvertToSql(expression, level + 1, flags);

								switch (Body.NodeType)
								{
									case ExpressionType.MemberAccess :
									case ExpressionType.Call         :
										break;
										//return GetSequence(expression, level).IsExpression(Body, 1, requestFlag);
									default                          : return new[] { Parser.ParseExpression(this, expression) };
								}
							}
							else
							{
								var root = Body.GetRootObject();

								if (root.NodeType == ExpressionType.Parameter)
								{
									var levelExpression = expression.GetLevelExpression(level - 1);
									var parseExpression = GetParseExpression(expression, levelExpression, Body);

									return ConvertToSql(parseExpression, 0, flags);
								}
							}

							break;
						}

						/*
						if (Body.NodeType == ExpressionType.Parameter)
						{
							if (expression == null)
								return GetSequence(Body, 0).ConvertToSql(null, 0, flags);

							var levelExpression = expression.GetLevelExpression(level);

							if (levelExpression == expression)
								return GetSequence(expression, level).ConvertToSql(null, 0, flags);
						}
						else
						{
							if (expression == null)
								return new[] { Parser.ParseExpression(this, Body) };

							if (level == 0)
								return GetSequence(expression, level).ConvertToSql(expression, level + 1, flags);
						}

						break;
						*/
				}
			}
			else
			{
				switch (flags)
				{
					case ConvertFlags.All   :
					case ConvertFlags.Key   :
					case ConvertFlags.Field :
						{
							if (level != 0)
							{
								var levelExpression = expression.GetLevelExpression(level);

								switch (levelExpression.NodeType)
								{
									case ExpressionType.MemberAccess :
										{
											var member = ((MemberExpression)levelExpression).Member;

											if (levelExpression == expression)
											{
												ISqlExpression[] sql;

												if (!_sql.TryGetValue(member, out sql))
												{
													sql = Parser.ParseExpressions(this, _members[member], flags);
													_sql.Add(member, sql);
												}

												return sql;
											}

											var memberExpression = _members[member];
											var parseExpression  = GetParseExpression(expression, levelExpression, memberExpression);

											return Parser.ParseExpressions(this, parseExpression, flags);
										}

									case ExpressionType.Parameter:

										if (levelExpression != expression)
											return GetSequence(expression, level).ConvertToSql(expression, level + 1, flags);
										break;
								}
							}

							if (level == 0)
							{
								if (expression == null)
								{
									if (flags != ConvertFlags.Field)
									{
										Func<Expression,ISqlExpression[]> func = e => ConvertToSql(e, 0, flags);

										if (flags != ConvertFlags.Field)
											func = e => ConvertToSql(e, 0, IsExpression(e, 0, RequestFor.Field) ? ConvertFlags.Field : flags);

										var q =
											from m in _members.Values.Distinct()
											select func(m) into mm
											from m in mm
											select m;

										return q.ToArray();
									}
								}
								else
								{
									if (expression.NodeType == ExpressionType.Parameter)
									{
										var levelExpression = expression.GetLevelExpression(level);

										if (levelExpression == expression)
											return GetSequence(expression, level).ConvertToSql(null, 0, flags);
									}
									else
									{
										return GetSequence(expression, level).ConvertToSql(expression, level + 1, flags);
									}
								}
							}

							break;
						}
				}
			}

			throw new NotImplementedException();
		}

		#endregion

		#region ConvertToIndex

		readonly Dictionary<Tuple<MemberInfo,ConvertFlags>,int[]> _memberIndex = new Dictionary<Tuple<MemberInfo,ConvertFlags>,int[]>();

		public virtual int[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
		{
			if (IsScalar)
			{
				if (expression == null)
					return ConvertToSql(expression, level, flags).Select(_ => SqlQuery.Select.Add(_)).ToArray();

				switch (flags)
				{
					case ConvertFlags.Field :
					case ConvertFlags.All   : return GetSequence(expression, level).ConvertToIndex(expression, level + 1, flags);
				}
			}
			else
			{
				switch (flags)
				{
					case ConvertFlags.All   :
					case ConvertFlags.Key   :
					case ConvertFlags.Field :
						{
							if (expression == null)
							{
								throw new NotImplementedException();
							}

							if (level == 0)
							{
								throw new NotImplementedException();
								//return Sequence.ConvertToIndex(expression, level + 1, flags);
							}

							var levelExpression = expression.GetLevelExpression(level);

							switch (levelExpression.NodeType)
							{
								case ExpressionType.MemberAccess :
									{
										if (levelExpression == expression)
										{
											var member = Tuple.Create(((MemberExpression)levelExpression).Member, flags);

											int[] idx;

											if (!_memberIndex.TryGetValue(member, out idx))
											{
												var sql = ConvertToSql(expression, level, flags);

												if (flags == ConvertFlags.Field && sql.Length != 1)
													throw new InvalidOperationException();

												idx = sql.Select(s => SqlQuery.Select.Add(s)).ToArray();

												_memberIndex.Add(member, idx);
											}

											return idx;
										}

										return GetSequence(expression, level).ConvertToIndex(expression, level + 1, flags);
									}

								case ExpressionType.Parameter:

									if (levelExpression == expression)
										break;
									return GetSequence(expression, level).ConvertToIndex(expression, level + 1, flags);
							}

							break;
						}
				}
			}

			throw new NotImplementedException();
		}

		#endregion

		#region IsExpression

		public virtual bool IsExpression(Expression expression, int level, RequestFor requestFlag)
		{
			switch (requestFlag)
			{
				case RequestFor.SubQuery    : return false;
				case RequestFor.Root        :
					return Sequence.Length == 1 ?
						expression == Lambda.Parameters[0] :
						Lambda.Parameters.Any(p => p == expression);
			}

			if (IsScalar)
			{
				switch (requestFlag)
				{
					case RequestFor.Association :
					case RequestFor.Field       :
					case RequestFor.Expression  :
					case RequestFor.Query       :
						{
							if (expression == null)
								return IsExpression(Body, 0, requestFlag);

							if (Body.NodeType == ExpressionType.Parameter)
							{
								if (level == 0)
								{
									var sequence = GetSequence(Body, 0);

									return expression == Body ?
										sequence.IsExpression(null,       0, requestFlag) :
										sequence.IsExpression(expression, 1, requestFlag);
								}

								var levelExpression = expression.GetLevelExpression(level - 1);
								var parseExpression = GetParseExpression(expression, levelExpression, Body);

								return IsExpression(parseExpression, 0, requestFlag);
							}

							if (level == 0)
							{
								var levelExpression = expression.GetLevelExpression(level);

								if (levelExpression != expression)
									return GetSequence(expression, level).IsExpression(expression, level + 1, requestFlag);

								switch (Body.NodeType)
								{
									case ExpressionType.MemberAccess :
									case ExpressionType.Call         :
										break;
										//return GetSequence(expression, level).IsExpression(Body, 1, requestFlag);
									default                          : return requestFlag == RequestFor.Expression;
								}
							}
							else
							{
								var root = Body.GetRootObject();

								if (root.NodeType == ExpressionType.Parameter)
								{
									var levelExpression = expression.GetLevelExpression(level - 1);
									var parseExpression = GetParseExpression(expression, levelExpression, Body);

									return IsExpression(parseExpression, 0, requestFlag);
								}
							}

							break;
						}
				}
			}
			else
			{
				switch (requestFlag)
				{
					case RequestFor.Association :
					case RequestFor.Field       :
					case RequestFor.Expression  :
					case RequestFor.Query       :
						{
							if (expression == null)
							{
								if (requestFlag == RequestFor.Expression)
									return _members.Values.Any(member => IsExpression(member, 0, requestFlag));

								return false;
							}

							var levelExpression = expression.GetLevelExpression(level);

							switch (levelExpression.NodeType)
							{
								case ExpressionType.MemberAccess :
									{
										var memberExpression = _members[((MemberExpression)levelExpression).Member];
										var parseExpression  = GetParseExpression(expression, levelExpression, memberExpression);

										var sequence  = GetSequence(expression, level);
										var parameter = Lambda.Parameters[Sequence.Length == 0 ? 0 : Array.IndexOf(Sequence, sequence)];

										if (memberExpression == parameter && levelExpression == expression)
											return sequence.IsExpression(null, 0, requestFlag);

										switch (memberExpression.NodeType)
										{
											case ExpressionType.MemberAccess :
											case ExpressionType.Parameter    :
											case ExpressionType.Call         : return sequence.IsExpression(parseExpression, 1, requestFlag);
											default                          : return requestFlag == RequestFor.Expression;
										}
									}

								case ExpressionType.Parameter :
									{
										var sequence  = GetSequence(expression, level);
										var parameter = Lambda.Parameters[Sequence.Length == 0 ? 0 : Array.IndexOf(Sequence, sequence)];

										if (levelExpression == expression)
										{
											if (levelExpression == parameter)
												return sequence.IsExpression(null, 0, requestFlag);
										}
										else if (level == 0)
											return sequence.IsExpression(expression, 1, requestFlag);

										break;
									}

								default : return requestFlag == RequestFor.Expression;
							}

							break;
						}
				}
			}

			throw new NotImplementedException();
		}

		#endregion

		#region GetContext

		public virtual IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
		{
			return GetSequence(expression, level).GetContext(expression, level + 1, currentSql);
		}

		#endregion

		#region ConvertToParentIndex

		public virtual int ConvertToParentIndex(int index, IParseContext context)
		{
			return Parent == null ? index : Parent.ConvertToParentIndex(index, this);
		}

		#endregion

		#region SetAlias

		public virtual void SetAlias(string alias)
		{
		}

		#endregion

		#region Helpers

		protected bool IsSubQuery()
		{
			for (var p = Parent; p != null; p = p.Parent)
				if (p.IsExpression(null, 0, RequestFor.SubQuery))
					return true;
			return false;
		}

		IParseContext GetSequence(Expression expression, int level)
		{
			if (Sequence.Length == 1)
				return Sequence[0];

			var levelExpression = expression.GetLevelExpression(level);

			if (IsScalar)
			{
				var root =  Body.GetRootObject();

				if (root.NodeType == ExpressionType.Parameter)
					for (int i = 0; i < Lambda.Parameters.Count; i++)
						if (root == Lambda.Parameters[i])
							return Sequence[i];
			}
			else
			{
				switch (levelExpression.NodeType)
				{
					case ExpressionType.MemberAccess :
						{
							var memberExpression = _members[((MemberExpression)levelExpression).Member];
							var root             =  memberExpression.GetRootObject();

							for (int i = 0; i < Lambda.Parameters.Count; i++)
								if (root == Lambda.Parameters[i])
									return Sequence[i];

							break;
						}

					case ExpressionType.Parameter :
						{
							var root =  expression.GetRootObject();

							if (levelExpression == root)
							{
								for (int i = 0; i < Lambda.Parameters.Count; i++)
									if (levelExpression == Lambda.Parameters[i])
										return Sequence[i];
							}

							break;
						}
				}
			}

			throw new NotImplementedException();
		}

		static Expression GetParseExpression(Expression expression, Expression levelExpression, Expression memberExpression)
		{
			return levelExpression != expression ?
				expression.Convert(ex => ex == levelExpression ? memberExpression : ex) :
				memberExpression;
		}

		#endregion
	}
}
