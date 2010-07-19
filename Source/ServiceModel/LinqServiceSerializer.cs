using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

using BLToolkit.Data.Sql;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.Mapping;

namespace BLToolkit.ServiceModel
{
	class LinqServiceSerializer: XmlObjectSerializer
	{
		#region Overrides

		public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
		{
			writer.WriteStartElement("Query");
		}

		public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
		{
			new Serializer(writer).Serialize((LinqServiceQuery)graph);
		}

		public override void WriteEndObject(XmlDictionaryWriter writer)
		{
			writer.WriteEndElement();
		}

		public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
		{
			return new Deserializer(reader).Deserialize();
		}

		public override bool IsStartObject(XmlDictionaryReader reader)
		{
			return reader.Name == "Query";
		}

		#endregion

		const int _typeIndex  = -1;
		const int _paramIndex = -2;

		class Serializer
		{
			public Serializer(XmlDictionaryWriter writer)
			{
				_writer = writer;
			}

			readonly XmlDictionaryWriter    _writer;
			readonly StringBuilder          _sb    = new StringBuilder();
			readonly Dictionary<object,int> _dic   = new Dictionary<object,int>();
			private  int                    _index;

			public void Serialize(LinqServiceQuery query)
			{
				var visitor = new QueryVisitor();

				visitor.Visit(query.Query, Visit);

				foreach (var parameter in query.Parameters)
					if (!_dic.ContainsKey(parameter))
						Visit(parameter);

				_sb
					.Append(++_index)
					.Append(' ')
					.Append(_paramIndex);

				Append(query.Parameters.Length);

				foreach (var parameter in query.Parameters)
					Append(parameter);

				_sb.AppendLine();

				_writer.WriteCData(_sb.ToString());
			}

			void Visit(IQueryElement e)
			{
				switch (e.ElementType)
				{
					case QueryElementType.SqlField :
						{
							var fld = (SqlField)e;

							if (fld == fld.Table.All)
								return;

							GetType(fld.SystemType);

							if (fld.MemberMapper != null)
								GetType(fld.MemberMapper.MemberAccessor.TypeAccessor.OriginalType);

							break;
						}

					case QueryElementType.SqlParameter :
						{
							var p = (SqlParameter)e;

							GetType(p.SystemType);

							if (p.EnumTypes != null)
								foreach (var type in p.EnumTypes)
									GetType(type);

							break;
						}

					case QueryElementType.SqlFunction         : GetType(((SqlFunction)        e).SystemType); break;
					case QueryElementType.SqlExpression       : GetType(((SqlExpression)      e).SystemType); break;
					case QueryElementType.SqlBinaryExpression : GetType(((SqlBinaryExpression)e).SystemType); break;
					case QueryElementType.SqlDataType         : GetType(((SqlDataType)        e).Type);       break;
					case QueryElementType.SqlValue            : GetType(((SqlValue)           e).SystemType); break;
					case QueryElementType.SqlTable            : GetType(((SqlTable)           e).ObjectType); break;
				}

				_dic.Add(e, ++_index);

				_sb
					.Append(_index)
					.Append(' ')
					.Append((int)e.ElementType);

				switch (e.ElementType)
				{
					case QueryElementType.SqlField :
						{
							var elem = (SqlField)e;

							Append(elem.SystemType);
							Append(elem.Name);
							Append(elem.PhysicalName);
							Append(elem.Nullable);
							Append(elem.PrimaryKeyOrder);
							Append(elem.IsIdentity);
							Append(elem.IsUpdatable);
							Append(elem.IsInsertable);
							Append(elem.MemberMapper == null ? null : elem.MemberMapper.MemberAccessor.TypeAccessor.OriginalType);
							Append(elem.MemberMapper == null ? null : elem.MemberMapper.Name);

							break;
						}

					case QueryElementType.SqlFunction :
						{
							var elem = (SqlFunction)e;

							Append(elem.SystemType);
							Append(elem.Name);
							Append(elem.Precedence);
							Append(elem.Parameters);

							break;
						}

					case QueryElementType.SqlParameter :
						{
							var elem = (SqlParameter)e;

							Append(elem.SystemType);
							Append(elem.Name);
							Append(elem.IsQueryParameter);
							Append(elem.SystemType, elem.Value);

							if (elem.EnumTypes == null)
								_sb.Append(" -");
							else
							{
								Append(elem.EnumTypes.Count);

								foreach (var type in elem.EnumTypes)
									Append(type);
							}

							if (elem.TakeValues == null)
								_sb.Append(" -");
							else
							{
								Append(elem.TakeValues.Count);

								foreach (var type in elem.TakeValues)
									Append(type);
							}

							Append(elem.LikeStart);
							Append(elem.LikeEnd);

							break;
						}

					case QueryElementType.SqlExpression :
						{
							var elem = (SqlExpression)e;

							Append(elem.SystemType);
							Append(elem.Expr);
							Append(elem.Precedence);
							Append(elem.Parameters);

							break;
						}

					case QueryElementType.SqlBinaryExpression :
						{
							var elem = (SqlBinaryExpression)e;

							Append(elem.SystemType);
							Append(elem.Expr1);
							Append(elem.Operation);
							Append(elem.Expr2);
							Append(elem.Precedence);

							break;
						}

					case QueryElementType.SqlValue :
						{
							var elem = (SqlValue)e;
							Append(elem.SystemType, elem.Value);
							break;
						}

					case QueryElementType.SqlDataType :
						{
							var elem = (SqlDataType)e;

							Append((int)elem.DbType);
							Append(elem.Type);
							Append(elem.Length);
							Append(elem.Precision);
							Append(elem.Scale);

							break;
						}

					case QueryElementType.SqlTable :
						{
							var elem = (SqlTable)e;

							Append(elem.SourceID);
							Append(elem.Name);
							Append(elem.Alias);
							Append(elem.Database);
							Append(elem.Owner);
							Append(elem.PhysicalName);
							Append(elem.ObjectType);

							if (elem.SequenceAttributes == null)
								_sb.Append(" -");
							else
							{
								Append(elem.SequenceAttributes.Length);

								foreach (var a in elem.SequenceAttributes)
								{
									Append(a.ProviderName);
									Append(a.SequenceName);
								}
							}

							Append(elem.Fields.Count);

							foreach (var field in elem.Fields)
								Append(_dic[field.Value]);

							break;
						}

					case QueryElementType.ExprPredicate :
						{
							var elem = (SqlQuery.Predicate.Expr)e;

							Append(elem.Expr1);
							Append(elem.Precedence);

							break;
						}

					case QueryElementType.NotExprPredicate :
						{
							var elem = (SqlQuery.Predicate.NotExpr)e;

							Append(elem.Expr1);
							Append(elem.IsNot);
							Append(elem.Precedence);

							break;
						}

					case QueryElementType.ExprExprPredicate :
						{
							var elem = (SqlQuery.Predicate.ExprExpr)e;

							Append(elem.Expr1);
							Append((int)elem.Operator);
							Append(elem.Expr2);

							break;
						}

					case QueryElementType.LikePredicate :
						{
							var elem = (SqlQuery.Predicate.Like)e;

							Append(elem.Expr1);
							Append(elem.IsNot);
							Append(elem.Expr2);
							Append(elem.Escape);

							break;
						}

					case QueryElementType.BetweenPredicate :
						{
							var elem = (SqlQuery.Predicate.Between)e;

							Append(elem.Expr1);
							Append(elem.IsNot);
							Append(elem.Expr2);
							Append(elem.Expr3);

							break;
						}

					case QueryElementType.IsNullPredicate :
						{
							var elem = (SqlQuery.Predicate.IsNull)e;

							Append(elem.Expr1);
							Append(elem.IsNot);

							break;
						}

					case QueryElementType.InSubQueryPredicate :
						{
							var elem = (SqlQuery.Predicate.InSubQuery)e;

							Append(elem.Expr1);
							Append(elem.IsNot);
							Append(elem.SubQuery);

							break;
						}

					case QueryElementType.InListPredicate :
						{
							var elem = (SqlQuery.Predicate.InList)e;

							Append(elem.Expr1);
							Append(elem.IsNot);
							Append(elem.Values);

							break;
						}

					case QueryElementType.FuncLikePredicate :
						{
							var elem = (SqlQuery.Predicate.FuncLike)e;
							Append(elem.Function);
							break;
						}

					case QueryElementType.SqlQuery :
						{
							var elem = (SqlQuery)e;

							Append(elem.SourceID);
							Append((int) elem.QueryType);

							if (elem.QueryType == QueryType.Update || elem.QueryType == QueryType.Insert)
								Append(elem.Set);
							else
								Append(elem.Select);

							Append(elem.From);
							Append(elem.Where);
							Append(elem.GroupBy);
							Append(elem.Having);
							Append(elem.OrderBy);
							Append(elem.ParentSql == null ? 0 : elem.ParentSql.SourceID);
							Append(elem.ParameterDependent);

							if (!elem.HasUnion)
								_sb.Append(" -");
							else
								Append(elem.Unions);

							Append(elem.Parameters);

							break;
						}

					case QueryElementType.Column :
						{
							var elem = (SqlQuery.Column) e;

							Append(elem.Parent.SourceID);
							Append(elem.Expression);
							Append(elem._alias);

							break;
						}

					case QueryElementType.SearchCondition :
							Append(((SqlQuery.SearchCondition)e).Conditions);
							break;

					case QueryElementType.Condition :
						{
							var elem = (SqlQuery.Condition)e;

							Append(elem.IsNot);
							Append(elem.Predicate);
							Append(elem.IsOr);

							break;
						}

					case QueryElementType.TableSource :
						{
							var elem = (SqlQuery.TableSource)e;

							Append(elem.Source);
							Append(elem._alias);
							Append(elem.Joins);

							break;
						}

					case QueryElementType.JoinedTable :
						{
							var elem = (SqlQuery.JoinedTable)e;

							Append((int)elem.JoinType);
							Append(elem.Table);
							Append(elem.IsWeak);
							Append(elem.Condition);

							break;
						}

					case QueryElementType.SelectClause :
						{
							var elem = (SqlQuery.SelectClause)e;

							Append(elem.IsDistinct);
							Append(elem.SkipValue);
							Append(elem.TakeValue);
							Append(elem.Columns);

							break;
						}

					case QueryElementType.SetClause :
						{
							var elem = (SqlQuery.SetClause)e;

							Append(elem.Items);
							Append(elem.Into);
							Append(elem.WithIdentity);

							break;
						}

					case QueryElementType.SetExpression :
						{
							var elem = (SqlQuery.SetExpression)e;

							Append(elem.Column);
							Append(elem.Expression);

							break;
						}

					case QueryElementType.FromClause    : Append(((SqlQuery.FromClause)   e).Tables);          break;
					case QueryElementType.WhereClause   : Append(((SqlQuery.WhereClause)  e).SearchCondition); break;
					case QueryElementType.GroupByClause : Append(((SqlQuery.GroupByClause)e).Items);           break;
					case QueryElementType.OrderByClause : Append(((SqlQuery.OrderByClause)e).Items);           break;

					case QueryElementType.OrderByItem :
						{
							var elem = (SqlQuery.OrderByItem)e;

							Append(elem.Expression);
							Append(elem.IsDescending);

							break;
						}

					case QueryElementType.Union :
						{
							var elem = (SqlQuery.Union)e;

							Append(elem.SqlQuery);
							Append(elem.IsAll);

							break;
						}
				}

				_sb.AppendLine();
			}

			#region Helpers

			void Append<T>(ICollection<T> exprs)
				where T : IQueryElement
			{
				if (exprs == null)
					_sb.Append(" -");
				else
				{
					Append(exprs.Count);

					foreach (var e in exprs)
						Append(_dic[e]);
				}
			}

			void Append(Type type, object value)
			{
				Append(type);
				Append(value == null ? null : Common.Convert.ToString(value));
			}

			void Append(int value)
			{
				_sb.Append(' ').Append(value);
			}

			void Append(Type value)
			{
				_sb.Append(' ').Append(value == null ? 0 : GetType(value));
			}

			void Append(bool value)
			{
				_sb.Append(' ').Append(value ? '1' : '0');
			}

			void Append(IQueryElement element)
			{
				_sb.Append(' ').Append(element == null ? 0 : _dic[element]);
			}

			void Append(string str)
			{
				_sb.Append(' ');

				if (str == null)
				{
					_sb.Append('-');
				}
				else if (str.Length == 0)
				{
					_sb.Append('0');
				}
				else
				{
					_sb
						.Append(str.Length)
						.Append(':')
						.Append(str);
				}
			}

			int GetType(Type type)
			{
				if (type == null)
					return 0;

				int idx;

				if (!_dic.TryGetValue(type, out idx))
				{
					_dic.Add(type, idx = ++_index);

					_sb
						.Append(idx)
						.Append(' ')
						.Append(_typeIndex)
						.Append(' ');

					Append(type.FullName);

					_sb.AppendLine();
				}

				return idx;
			}

			#endregion
		}

		class Deserializer
		{
			public Deserializer(XmlDictionaryReader reader)
			{
				_reader = reader;
			}

			readonly XmlDictionaryReader    _reader;
			readonly Dictionary<int,object> _dic = new Dictionary<int,object>();

			SqlQuery       _query;
			SqlParameter[] _parameters;
			string         _str;
			int            _pos;

			readonly Dictionary<int,SqlQuery> _queries = new Dictionary<int,SqlQuery>();
			readonly List<Action>             _actions = new List<Action>();

			public LinqServiceQuery Deserialize()
			{
				_str = _reader.ReadString();

				while (Parse()) {}

				foreach (var action in _actions)
					action();

				var query = new LinqServiceQuery { Query = _query, Parameters = _parameters};

				return query;
			}

			bool Parse()
			{
				while (_pos < _str.Length && (Peek() == '\n' || Peek() == '\r'))
					_pos++;

				if (_pos >= _str.Length)
					return false;

				var obj  = null as object;
				var idx  = ReadInt(); _pos++;
				var type = ReadInt(); _pos++;

				switch (type)
				{
					case _typeIndex:
						{
							obj = Type.GetType(ReadString(), false);
							break;
						}

					case _paramIndex:
						{
							obj = _parameters = ReadArray<SqlParameter>();
							break;
						}

					case (int)QueryElementType.SqlField :
						{
							var systemType       = Read<Type>();
							var name             = ReadString();
							var physicalName     = ReadString();
							var nullable         = ReadBool();
							var primaryKeyOrder  = ReadInt();
							var isIdentity       = ReadBool();
							var isUpdatable      = ReadBool();
							var isInsertable     = ReadBool();
							var memberMapperType = Read<Type>();
							var memberMapperName = ReadString();
							var memberMapper     = memberMapperType == null ? null : Map.GetObjectMapper(memberMapperType)[memberMapperName];

							obj = new SqlField(
								systemType,
								name,
								physicalName,
								nullable,
								primaryKeyOrder,
								isIdentity
									? new DataAccess.IdentityAttribute()
									: isInsertable || isUpdatable
									  	? new DataAccess.NonUpdatableAttribute(isInsertable, isUpdatable, false)
									  	: null,
								memberMapper);

							break;
						}

					case (int)QueryElementType.SqlFunction :
						{
							var systemType = Read<Type>();
							var name       = ReadString();
							var precedence = ReadInt();
							var parameters = ReadArray<ISqlExpression>();

							obj = new SqlFunction(systemType, name, precedence, parameters);

							break;
						}

					case (int)QueryElementType.SqlParameter:
						{
							var systemType       = Read<Type>();
							var name             = ReadString();
							var isQueryParameter = ReadBool();
							var value            = ReadValue(systemType);
							var enumTypes        = ReadList<Type>();
							var takeValues       = null as List<int>;

							var count = ReadCount();

							if (count != null)
							{
								takeValues = new List<int>(count.Value);

								for (var i = 0; i < count; i++)
									takeValues.Add(ReadInt());
							}

							var likeStart = ReadString();
							var likeEnd   = ReadString();

							obj = new SqlParameter(systemType, name, value)
							{
								IsQueryParameter = isQueryParameter,
								EnumTypes        = enumTypes,
								TakeValues       = takeValues,
								LikeStart        = likeStart,
								LikeEnd          = likeEnd,
							};

							break;
						}

					case (int)QueryElementType.SqlExpression :
						{
							var systemType = Read<Type>();
							var expr       = ReadString();
							var precedence = ReadInt();
							var parameters = ReadArray<ISqlExpression>();

							obj = new SqlExpression(systemType, expr, precedence, parameters);

							break;
						}

					case (int)QueryElementType.SqlBinaryExpression :
						{
							var systemType = Read<Type>();
							var expr1      = Read<ISqlExpression>();
							var operation  = ReadString();
							var expr2      = Read<ISqlExpression>();
							var precedence = ReadInt();

							obj = new SqlBinaryExpression(systemType, expr1, operation, expr2, precedence);

							break;
						}

					case (int)QueryElementType.SqlValue :
						{
							var systemType = Read<Type>();
							var value      = ReadValue(systemType);

							obj = new SqlValue(systemType, value);

							break;
						}

					case (int)QueryElementType.SqlDataType :
						{
							var dbType     = (SqlDbType)ReadInt();
							var systemType = Read<Type>();
							var length     = ReadInt();
							var precision  = ReadInt();
							var scale      = ReadInt();

							obj = new SqlDataType(dbType, systemType, length, precision, scale);

							break;
						}

					case (int)QueryElementType.SqlTable :
						{
							var sourceID           = ReadInt();
							var name               = ReadString();
							var alias              = ReadString();
							var database           = ReadString();
							var owner              = ReadString();
							var physicalName       = ReadString();
							var objectType         = Read<Type>();
							var sequenceAttributes = null as SequenceNameAttribute[];

							var count = ReadCount();

							if (count != null)
							{
								sequenceAttributes = new SequenceNameAttribute[count.Value];

								for (var i = 0; i < count.Value; i++)
									sequenceAttributes[i] = new SequenceNameAttribute(ReadString(), ReadString());
							}

							var fields = ReadArray<SqlField>();

							obj = new SqlTable(sourceID, name, alias, database, owner, physicalName, objectType, sequenceAttributes, fields);

							break;
						}

					case (int)QueryElementType.ExprPredicate :
						{
							var expr1      = Read<ISqlExpression>();
							var precedence = ReadInt();

							obj = new SqlQuery.Predicate.Expr(expr1, precedence);

							break;
						}

					case (int)QueryElementType.NotExprPredicate :
						{
							var expr1      = Read<ISqlExpression>();
							var isNot      = ReadBool();
							var precedence = ReadInt();

							obj = new SqlQuery.Predicate.NotExpr(expr1, isNot, precedence);

							break;
						}

					case (int)QueryElementType.ExprExprPredicate :
						{
							var expr1     = Read<ISqlExpression>();
							var @operator = (SqlQuery.Predicate.Operator)ReadInt();
							var expr2     = Read<ISqlExpression>();

							obj = new SqlQuery.Predicate.ExprExpr(expr1, @operator, expr2);

							break;
						}

					case (int)QueryElementType.LikePredicate :
						{
							var expr1  = Read<ISqlExpression>();
							var isNot  = ReadBool();
							var expr2  = Read<ISqlExpression>();
							var escape = Read<ISqlExpression>();

							obj = new SqlQuery.Predicate.Like(expr1, isNot, expr2, escape);

							break;
						}

					case (int)QueryElementType.BetweenPredicate :
						{
							var expr1 = Read<ISqlExpression>();
							var isNot = ReadBool();
							var expr2 = Read<ISqlExpression>();
							var expr3 = Read<ISqlExpression>();

							obj = new SqlQuery.Predicate.Between(expr1, isNot, expr2, expr3);

							break;
						}

					case (int)QueryElementType.IsNullPredicate :
						{
							var expr1 = Read<ISqlExpression>();
							var isNot = ReadBool();

							obj = new SqlQuery.Predicate.IsNull(expr1, isNot);

							break;
						}

					case (int)QueryElementType.InSubQueryPredicate :
						{
							var expr1    = Read<ISqlExpression>();
							var isNot    = ReadBool();
							var subQuery = Read<SqlQuery>();

							obj = new SqlQuery.Predicate.InSubQuery(expr1, isNot, subQuery);

							break;
						}

					case (int)QueryElementType.InListPredicate :
						{
							var expr1  = Read<ISqlExpression>();
							var isNot  = ReadBool();
							var values = ReadList<ISqlExpression>();

							obj = new SqlQuery.Predicate.InList(expr1, isNot, values);

							break;
						}

					case (int)QueryElementType.FuncLikePredicate :
						{
							var func = Read<SqlFunction>();
							obj = new SqlQuery.Predicate.FuncLike(func);
							break;
						}

					case (int)QueryElementType.SqlQuery :
						{
							var sid                = ReadInt();
							var queryType          = (QueryType)ReadInt();
							var readSet            = queryType == QueryType.Update || queryType == QueryType.Insert;
							var set                = readSet ? Read<SqlQuery.SetClause>() : null;
							var select             = readSet ? new SqlQuery.SelectClause(null) : Read<SqlQuery.SelectClause>();
							var from               = Read<SqlQuery.FromClause>();
							var where              = Read<SqlQuery.WhereClause>();
							var groupBy            = Read<SqlQuery.GroupByClause>();
							var having             = Read<SqlQuery.WhereClause>();
							var orderBy            = Read<SqlQuery.OrderByClause>();
							var parentSql          = ReadInt();
							var parameterDependent = ReadBool();
							var unions             = ReadArray<SqlQuery.Union>();
							var parameters         = ReadArray<SqlParameter>();

							var query = _query = new SqlQuery(sid) { QueryType = queryType };

							query.Init(
								set,
								select,
								from,
								where,
								groupBy,
								having,
								orderBy,
								unions == null ? null : unions.ToList(),
								null,
								parameterDependent,
								parameters.ToList());

							_queries.Add(sid, _query);

							if (parentSql != 0)
								_actions.Add(() => query.ParentSql = _queries[parentSql]);

							break;
						}

					case (int)QueryElementType.Column :
						{
							var sid        = ReadInt();
							var expression = Read<ISqlExpression>();
							var alias      = ReadString();

							var col = new SqlQuery.Column(null, expression, alias);

							_actions.Add(() => col.Parent = _queries[sid]);

							obj = col;

							break;
						}

					case (int)QueryElementType.SearchCondition :
						obj = new SqlQuery.SearchCondition(ReadArray<SqlQuery.Condition>());
						break;

					case (int)QueryElementType.Condition :
						obj = new SqlQuery.Condition(ReadBool(), Read<ISqlPredicate>(), ReadBool());
						break;

					case (int)QueryElementType.TableSource :
						{
							var source = Read<ISqlTableSource>();
							var alias  = ReadString();
							var joins  = ReadArray<SqlQuery.JoinedTable>();

							obj = new SqlQuery.TableSource(source, alias, joins);

							break;
						}

					case (int)QueryElementType.JoinedTable :
						{
							var joinType  = (SqlQuery.JoinType)ReadInt();
							var table     = Read<SqlQuery.TableSource>();
							var isWeak    = ReadBool();
							var condition = Read<SqlQuery.SearchCondition>();

							obj = new SqlQuery.JoinedTable(joinType, table, isWeak, condition);

							break;
						}

					case (int)QueryElementType.SelectClause :
						{
							var isDistinct = ReadBool();
							var skipValue  = Read<ISqlExpression>();
							var takeValue  = Read<ISqlExpression>();
							var columns    = ReadArray<SqlQuery.Column>();

							obj = new SqlQuery.SelectClause(isDistinct, takeValue, skipValue, columns);

							break;
						}

					case (int)QueryElementType.SetClause :
						{
							var items = ReadArray<SqlQuery.SetExpression>();
							var into  = Read<SqlTable>();
							var wid   = ReadBool();

							var c = new SqlQuery.SetClause { Into = into, WithIdentity = wid };

							c.Items.AddRange(items);
							obj = c;

							break;
						}

					case (int)QueryElementType.SetExpression :
						obj = new SqlQuery.SetExpression(Read<ISqlExpression>(), Read<ISqlExpression>());
						break;

					case (int)QueryElementType.FromClause :
						obj = new SqlQuery.FromClause(ReadArray<SqlQuery.TableSource>());
						break;

					case (int)QueryElementType.WhereClause :
						obj = new SqlQuery.WhereClause(Read<SqlQuery.SearchCondition>());
						break;

					case (int)QueryElementType.GroupByClause :
						obj = new SqlQuery.GroupByClause(ReadArray<ISqlExpression>());
						break;

					case (int)QueryElementType.OrderByClause :
						obj = new SqlQuery.OrderByClause(ReadArray<SqlQuery.OrderByItem>());
						break;

					case (int)QueryElementType.OrderByItem :
						{
							var expression   = Read<ISqlExpression>();
							var isDescending = ReadBool();

							obj = new SqlQuery.OrderByItem(expression, isDescending);

							break;
						}

					case (int)QueryElementType.Union :
						{
							var sqlQuery = Read<SqlQuery>();
							var isAll    = ReadBool();

							obj = new SqlQuery.Union(sqlQuery, isAll);

							break;
						}
				}

				_dic.Add(idx, obj);

				return true;
			}

			#region Helpers

			char Peek()
			{
				return _str[_pos];
			}

			char Next()
			{
				return _str[++_pos];
			}

			bool Get(char c)
			{
				if (Peek() == c)
				{
					_pos++;
					return true;
				}

				return false;
			}

			int ReadInt()
			{
				Get(' ');

				var minus = Get('-');
				var value = 0;

				for (var c = Peek(); char.IsDigit(c); c = Next())
					value = value * 10 + (c - '0');

				return minus ? -value : value;
			}

			int? ReadCount()
			{
				Get(' ');

				if (Get('-'))
					return null;

				var value = 0;

				for (var c = Peek(); char.IsDigit(c); c = Next())
					value = value * 10 + (c - '0');

				return value;
			}

			string ReadString()
			{
				Get(' ');

				var c = Peek();

				if (c == '-')
				{
					_pos++;
					return null;
				}

				if (c == '0')
				{
					_pos++;
					return string.Empty;
				}

				var len   = ReadInt();
				var value = _str.Substring(++_pos, len);

				_pos += len;

				return value;
			}

			bool ReadBool()
			{
				Get(' ');

				var value = Peek() == '1';

				_pos++;

				return value;
			}

			T Read<T>()
				where T : class
			{
				var idx = ReadInt();
				return idx == 0 ? null : (T)_dic[idx];
			}

			T[] ReadArray<T>()
				where T : class
			{
				var count = ReadCount();

				if (count == null)
					return null;

				var items = new T[count.Value];

				for (var i = 0; i < count; i++)
					items[i] = Read<T>();

				return items;
			}

			List<T> ReadList<T>()
				where T : class
			{
				var count = ReadCount();

				if (count == null)
					return null;

				var items = new List<T>(count.Value);

				for (var i = 0; i < count; i++)
					items.Add(Read<T>());

				return items;
			}

			object ReadValue(Type type)
			{
				var str = ReadString();

				if (str == null)
					return null;

				if (type == typeof(string))
					return str;

				var underlyingType = type;
				var isNullable     = false;

				if (underlyingType.IsGenericType && underlyingType.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					underlyingType = underlyingType.GetGenericArguments()[0];
					isNullable     = true;
				}

				if (underlyingType.IsEnum)
					return Enum.Parse(type, str);

				if (isNullable)
				{
					switch (Type.GetTypeCode(underlyingType))
					{
						case TypeCode.Boolean  : return Common.Convert.ToNullableBoolean (str);
						case TypeCode.Char     : return Common.Convert.ToNullableChar    (str);
						case TypeCode.SByte    : return Common.Convert.ToNullableSByte   (str);
						case TypeCode.Byte     : return Common.Convert.ToNullableByte    (str);
						case TypeCode.Int16    : return Common.Convert.ToNullableInt16   (str);
						case TypeCode.UInt16   : return Common.Convert.ToNullableUInt16  (str);
						case TypeCode.Int32    : return Common.Convert.ToNullableInt32   (str);
						case TypeCode.UInt32   : return Common.Convert.ToNullableUInt32  (str);
						case TypeCode.Int64    : return Common.Convert.ToNullableInt64   (str);
						case TypeCode.UInt64   : return Common.Convert.ToNullableUInt64  (str);
						case TypeCode.Single   : return Common.Convert.ToNullableSingle  (str);
						case TypeCode.Double   : return Common.Convert.ToNullableDouble  (str);
						case TypeCode.Decimal  : return Common.Convert.ToNullableDecimal (str);
						case TypeCode.DateTime : return Common.Convert.ToNullableDateTime(str);
						case TypeCode.Object   :
							if (type == typeof(Guid))           return Common.Convert.ToNullableGuid          (str);
							if (type == typeof(DateTimeOffset)) return Common.Convert.ToNullableDateTimeOffset(str);
							if (type == typeof(TimeSpan))       return Common.Convert.ToNullableTimeSpan      (str);
							break;
						default                : break;
					}
				}
				else
				{
					switch (Type.GetTypeCode(underlyingType))
					{
						case TypeCode.Boolean  : return Common.Convert.ToBoolean(str);
						case TypeCode.Char     : return Common.Convert.ToChar    (str);
						case TypeCode.SByte    : return Common.Convert.ToSByte   (str);
						case TypeCode.Byte     : return Common.Convert.ToByte    (str);
						case TypeCode.Int16    : return Common.Convert.ToInt16   (str);
						case TypeCode.UInt16   : return Common.Convert.ToUInt16  (str);
						case TypeCode.Int32    : return Common.Convert.ToInt32   (str);
						case TypeCode.UInt32   : return Common.Convert.ToUInt32  (str);
						case TypeCode.Int64    : return Common.Convert.ToInt64   (str);
						case TypeCode.UInt64   : return Common.Convert.ToUInt64  (str);
						case TypeCode.Single   : return Common.Convert.ToSingle  (str);
						case TypeCode.Double   : return Common.Convert.ToDouble  (str);
						case TypeCode.Decimal  : return Common.Convert.ToDecimal (str);
						case TypeCode.DateTime : return Common.Convert.ToDateTime(str);
						case TypeCode.Object   :
							if (type == typeof(Guid))           return Common.Convert.ToGuid          (str);
							if (type == typeof(DateTimeOffset)) return Common.Convert.ToDateTimeOffset(str);
							if (type == typeof(TimeSpan))       return Common.Convert.ToTimeSpan      (str);
							if (type == typeof(Binary))         return Common.Convert.ToLinqBinary    (str);
							break;
						default                : break;
					}
				}

				if (type == typeof(SqlByte))     return Common.Convert.ToSqlByte    (str);
				if (type == typeof(SqlInt16))    return Common.Convert.ToSqlInt16   (str);
				if (type == typeof(SqlInt32))    return Common.Convert.ToSqlInt32   (str);
				if (type == typeof(SqlInt64))    return Common.Convert.ToSqlInt64   (str);
				if (type == typeof(SqlSingle))   return Common.Convert.ToSqlSingle  (str);
				if (type == typeof(SqlBoolean))  return Common.Convert.ToSqlBoolean (str);
				if (type == typeof(SqlDouble))   return Common.Convert.ToSqlDouble  (str);
				if (type == typeof(SqlDateTime)) return Common.Convert.ToSqlDateTime(str);
				if (type == typeof(SqlDecimal))  return Common.Convert.ToSqlDecimal (str);
				if (type == typeof(SqlMoney))    return Common.Convert.ToSqlMoney   (str);
				if (type == typeof(SqlString))   return Common.Convert.ToSqlString  (str);
				if (type == typeof(SqlGuid))     return Common.Convert.ToSqlGuid    (str);

				throw new InvalidOperationException();
			}

			#endregion
		}
	}
}
