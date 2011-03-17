using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;

	class SelectManyParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, ParseInfo parseInfo)
		{
			return
				methodCall.IsQueryable("SelectMany") &&
				methodCall.Arguments.Count == 3      &&
				((LambdaExpression)methodCall.Arguments[1].Unwrap()).Parameters.Count == 1;
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, ParseInfo parseInfo)
		{
			var sequence           = parser.ParseSequence(new ParseInfo(parseInfo, methodCall.Arguments[0]));
			var collectionSelector = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			var resultSelector     = (LambdaExpression)methodCall.Arguments[2].Unwrap();

			if (!sequence.SqlQuery.GroupBy.IsEmpty)
			{
				sequence = new SubQueryContext(sequence);
			}

			var context        = new SelectManyContext(parseInfo.Parent, collectionSelector, sequence);
			var expr           = collectionSelector.Body.Unwrap();

			var collectionInfo = new ParseInfo(context, expr, new SqlQuery());
			var collection     = parser.ParseSequence(collectionInfo);
			var leftJoin       = collection is DefaultIfEmptyParser.DefaultIfEmptyContext;
			var sql            = collection.SqlQuery;

			var sequenceTable  = sequence.SqlQuery.From.Tables[0].Source;
			var newQuery       = null != new QueryVisitor().Find(sql, e => e == collectionInfo.SqlQuery);
			var crossApply     = null != new QueryVisitor().Find(sql, e =>
				e == sequenceTable ||
				e.ElementType == QueryElementType.SqlField && sequenceTable == ((SqlField)e).Table ||
				e.ElementType == QueryElementType.Column   && sequenceTable == ((SqlQuery.Column)e).Parent);

			if (!newQuery)
			{
				context.Collection = new SubQueryContext(collection, sequence.SqlQuery, false);
				return new SelectContext(parseInfo.Parent, resultSelector, sequence, context);
			}

			if (!crossApply)
			{
				if (!leftJoin)
				{
					context.Collection = new SubQueryContext(collection, sequence.SqlQuery, true);
					return new SelectContext(parseInfo.Parent, resultSelector, sequence, context);
				}
				else
				{
					var join = SqlQuery.OuterApply(sql);
					sequence.SqlQuery.From.Tables[0].Joins.Add(join.JoinedTable);
					context.Collection = new SubQueryContext(collection, sequence.SqlQuery, false);

					return new SelectContext(parseInfo.Parent, resultSelector, sequence, context);
				}
			}

			if (collection is TableParser.TableContext)
			{
				var join = leftJoin ? SqlQuery.LeftJoin(sql) : SqlQuery.InnerJoin(sql);

				join.JoinedTable.Condition.Conditions.AddRange(sql.Where.SearchCondition.Conditions);

				sql.Where.SearchCondition.Conditions.Clear();

				var collectionParent = collection.Parent as TableParser.TableContext;

				// Association.
				//
				if (collectionParent != null && collectionInfo.IsAssociationParsed)
				{
					var ts = (SqlQuery.TableSource)new QueryVisitor().Find(sequence.SqlQuery.From, e =>
					{
						if (e.ElementType == QueryElementType.TableSource)
						{
							var t = (SqlQuery.TableSource)e;
							return t.Source == collectionParent.SqlTable;
						}

						return false;
					});

					ts.Joins.Add(join.JoinedTable);
				}
				else
				{
					sequence.SqlQuery.From.Tables[0].Joins.Add(join.JoinedTable);
				}

				context.Collection = new SubQueryContext(collection, sequence.SqlQuery, false);
				return new SelectContext(parseInfo.Parent, resultSelector, sequence, context);
			}
			else
			{
				var join = leftJoin ? SqlQuery.OuterApply(sql) : SqlQuery.CrossApply(sql);
				sequence.SqlQuery.From.Tables[0].Joins.Add(join.JoinedTable);

				context.Collection = new SubQueryContext(collection, sequence.SqlQuery, false);
				return new SelectContext(parseInfo.Parent, resultSelector, sequence, context);
			}
		}

		public class SelectManyContext : SelectContext
		{
			public SelectManyContext(IParseContext parent, LambdaExpression lambda, IParseContext sequence)
				: base(parent, lambda, sequence)
			{
			}

			private IParseContext _collection;
			public  IParseContext  Collection
			{
				get { return _collection; }
				set
				{
					_collection = value;
					_collection.Parent = this;
				}
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				if (expression == null)
					return Collection.BuildExpression(expression, level);

				var root = expression.GetRootObject();

				if (root == Lambda.Parameters[0])
					return base.BuildExpression(expression, level);

				return Collection.BuildExpression(expression, level);
			}

			public override void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				if (Collection == null)
					base.BuildQuery(query, queryParameter);

				throw new NotImplementedException();
			}

			public override SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				if (Collection != null)
				{
					if (expression == null)
						return Collection.ConvertToIndex(expression, level, flags);

					var root = expression.GetRootObject();

					if (root != Lambda.Parameters[0])
						return Collection.ConvertToIndex(expression, level, flags);
				}

				return base.ConvertToIndex(expression, level, flags);
			}

			/*
			public override int ConvertToParentIndex(int index, IParseContext context)
			{
				if (Collection == null)
					return base.ConvertToParentIndex(index, context);

				throw new NotImplementedException();
			}
			*/

			public override SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				if (Collection != null)
				{
					if (expression == null)
						return Collection.ConvertToSql(expression, level, flags);

					var root = expression.GetRootObject();

					if (root != Lambda.Parameters[0])
						return Collection.ConvertToSql(expression, level, flags);
				}

				return base.ConvertToSql(expression, level, flags);
			}

			public override IParseContext GetContext(Expression expression, int level, ParseInfo parseInfo)
			{
				if (Collection != null)
				{
					if (expression == null)
						return Collection.GetContext(expression, level, parseInfo);

					var root = expression.GetRootObject();

					if (root != Lambda.Parameters[0])
						return Collection.GetContext(expression, level, parseInfo);
				}

				return base.GetContext(expression, level, parseInfo);
			}

			public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				if (Collection != null)
				{
					if (expression == null)
						return Collection.IsExpression(expression, level, requestFlag);

					var root = expression.GetRootObject();

					if (root != Lambda.Parameters[0])
						return Collection.IsExpression(expression, level, requestFlag);
				}

				return base.IsExpression(expression, level, requestFlag);
			}
		}
	}
}
