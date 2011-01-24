using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;
	using Reflection;

	class SelectParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			return methodCall.IsQueryable("Select");
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			var selector = (LambdaExpression)methodCall.Arguments[1].Unwrap();

			if (selector.Parameters.Count != 1)
				return null;

			var sequence = parser.ParseSequence(methodCall.Arguments[0], sqlQuery);

			sequence.SetAlias(selector.Parameters[0].Name);

			var body = selector.Body.Unwrap();

			// .Select(p => p)
			//
			if (body == selector.Parameters[0])
				return sequence;

			if (sequence.SqlQuery.Select.IsDistinct)
				sequence = new SubQueryContext(sequence);

			switch (body.NodeType)
			{
				// .Select(p => new { ... })
				//
				case ExpressionType.New        : return new SelectContext(sequence, selector, (NewExpression)       body);

				// .Select(p => new MyObject { ... })
				//
				case ExpressionType.MemberInit : return new SelectContext(sequence, selector, (MemberInitExpression)body);

				// .Select(p => everything else)
				//
				default                        : return new ScalarContext(sequence, selector);
			}
		}

		class ScalarContext : SequenceContextBase
		{
			public ScalarContext(IParseContext sequence, LambdaExpression expression)
				: base(sequence, expression)
			{
			}

			public override Expression BuildQuery()
			{
				return Parser.BuildExpression(this, Lambda.Body.Unwrap());
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				if (level == 0)
					return Sequence.BuildExpression(expression, level + 1);

				throw new NotImplementedException();
			}

			public override ISqlExpression[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				switch (flags)
				{
					case ConvertFlags.Field :
					case ConvertFlags.All   :
						if (level == 0)
							return Sequence.ConvertToSql(expression, level + 1, flags);
						break;
				}

				throw new NotImplementedException();
			}

			public override int[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				switch (flags)
				{
					case ConvertFlags.Field :
					case ConvertFlags.All   : return Sequence.ConvertToIndex(expression, level + 1, flags);
				}

				throw new NotImplementedException();
			}

			public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				switch (requestFlag)
				{
					case RequestFor.Association      : return Sequence.IsExpression(expression, level + 1, requestFlag);
					case RequestFor.Root             : return expression == Lambda.Parameters[0];
					case RequestFor.ScalarExpression :
						{
							var body = Lambda.Body.Unwrap();

							if (body.NodeType == ExpressionType.MemberAccess)
								return !Sequence.IsExpression(body, 1, RequestFor.Field);

							return true;
						}
				}

				throw new NotImplementedException();
			}

			public override IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
			{
				return Sequence.GetContext(expression, level + 1, currentSql);
			}
		}

		class SelectContext : ScalarContext
		{
			public SelectContext(IParseContext sequence, LambdaExpression expression, NewExpression body)
				: base(sequence, expression)
			{
				if (body.Members == null)
					return;

				for (var i = 0; i < body.Members.Count; i++)
				{
					var member = body.Members[i];

					_members.Add(member, body.Arguments[i]);

					if (member is MethodInfo)
						_members.Add(TypeHelper.GetPropertyByMethod((MethodInfo)member), body.Arguments[i]);
				}
			}

			public SelectContext(IParseContext sequence, LambdaExpression expression, MemberInitExpression body)
				: base(sequence, expression)
			{
				foreach (var binding in body.Bindings)
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
			}

			readonly Dictionary<MemberInfo,Expression> _members = new Dictionary<MemberInfo,Expression>();

			public override Expression BuildExpression(Expression expression, int level)
			{
				var levelExpression = expression.GetLevelExpression(level);

				switch (levelExpression.NodeType)
				{
					case ExpressionType.MemberAccess :
						{
							var memberExpression = _members[((MemberExpression)levelExpression).Member];

							if (memberExpression == Lambda.Parameters[0])
							{
								if (levelExpression == expression)
									return Sequence.BuildExpression(memberExpression, 0);
								return Sequence.BuildExpression(expression, level + 1);
							}

							var root = Root;

							Expression curex = null;

							_convertToIndex = (ex,l,f) =>
							{
								if (l == 0)
								{
									curex = ex;
									return root.ConvertToIndex(expression, 0, f);
								}

								return Sequence.ConvertToIndex(curex, 1, f);
							};

							Root = this;

							var parseExpression = GetParseExpression(expression, levelExpression, memberExpression);

							var bex = Parser.BuildExpression(this, parseExpression);

							Root = root;
							_convertToIndex = null;

							return bex;
						}

					case ExpressionType.Parameter :
						{
							if (levelExpression == Lambda.Parameters[0])
							{
								if (levelExpression == expression)
									return Sequence.BuildExpression(expression, 0);
								//return Sequence.BuildExpression(expression, level + 1);
							}

							break;
						}
				}

				if (level == 0)
					return Sequence.BuildExpression(expression, level + 1);

				throw new NotImplementedException();
			}

			Func<Expression,int,ConvertFlags,int[]> _convertToIndex;

			readonly Dictionary<MemberInfo,int> _index = new Dictionary<MemberInfo,int>();

			public override int[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				if (_convertToIndex != null)
					return _convertToIndex(expression, level, flags);

				switch (flags)
				{
					case ConvertFlags.Field :
						{
							if (level == 0)
								return Sequence.ConvertToIndex(expression, level + 1, flags);

							var levelExpression = expression.GetLevelExpression(level);

							switch (levelExpression.NodeType)
							{
								case ExpressionType.MemberAccess :
									{
										if (levelExpression == expression)
										{
											var member = ((MemberExpression)levelExpression).Member;

											int idx;

											if (!_index.TryGetValue(member, out idx))
											{
												var sql = ConvertToSql(expression, level, flags).Single();

												idx = SqlQuery.Select.Add(sql);

												_index.Add(member, idx);
											}

											return new[] { idx };
										}

										return Sequence.ConvertToIndex(expression, level + 1, flags);
									}

								case ExpressionType.Parameter:

									if (levelExpression == expression)
										break;
									return Sequence.ConvertToIndex(expression, level + 1, flags);
							}

							break;
						}

					case ConvertFlags.All   :
						break;
				}

				throw new NotImplementedException();
			}

			readonly Dictionary<MemberInfo,ISqlExpression> _sql = new Dictionary<MemberInfo,ISqlExpression>();

			public override ISqlExpression[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				switch (flags)
				{
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
												ISqlExpression sql;

												if (!_sql.TryGetValue(member, out sql))
												{
													sql = Parser.ParseExpression(this, _members[member]);
													_sql.Add(member, sql);
												}

												return new[] { sql };
											}

											var memberExpression = _members[member];
											var parseExpression  = GetParseExpression(expression, levelExpression, memberExpression);

											return new[] { Parser.ParseExpression(this, parseExpression) };
										}

									case ExpressionType.Parameter:

										if (levelExpression != expression)
											return Sequence.ConvertToSql(expression, level + 1, flags);
										break;
								}
							}

							if (level == 0)
								return Sequence.ConvertToSql(expression, level + 1, flags);

							break;
						}

					case ConvertFlags.All   :
						break;
				}

				throw new NotImplementedException();


			}


			public override bool IsExpression(Expression expression, int level, RequestFor testFlag)
			{
				switch (testFlag)
				{
					case RequestFor.ScalarExpression : return false;
					case RequestFor.Field            :
						{
							var levelExpression = expression.GetLevelExpression(level);

							switch (levelExpression.NodeType)
							{
								case ExpressionType.MemberAccess :
									{
										var memberExpression = _members[((MemberExpression)levelExpression).Member];
										var parseExpression  = GetParseExpression(expression, levelExpression, memberExpression);

										return Sequence.IsExpression(parseExpression, 1, testFlag);
									}

								case ExpressionType.Parameter:

									if (levelExpression == expression)
										return false;
									return Sequence.IsExpression(expression, level + 1, testFlag);

								default : return false;
							}
						}
				}

				return base.IsExpression(expression, level, testFlag);
			}

			static Expression GetParseExpression(Expression expression, Expression levelExpression, Expression memberExpression)
			{
				return levelExpression != expression ?
					expression.Convert(ex => ex == levelExpression ? memberExpression : ex) :
					memberExpression;
			}
		}
	}
}
