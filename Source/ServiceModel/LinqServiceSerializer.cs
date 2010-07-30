using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using BLToolkit.Data.Linq;
using BLToolkit.Data.Sql;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace BLToolkit.ServiceModel
{
	class LinqServiceSerializer
	{
		internal class XmlQuerySerializer : XmlObjectSerializer
		{
			#region Overrides

			public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
			{
				writer.WriteStartElement("Query");
			}

			public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
			{
				new QuerySerializer(writer).Serialize((LinqServiceQuery)graph);
			}

			public override void WriteEndObject(XmlDictionaryWriter writer)
			{
				writer.WriteEndElement();
			}

			public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
			{
				return new QueryDeserializer(reader).Deserialize();
			}

			public override bool IsStartObject(XmlDictionaryReader reader)
			{
				return reader.Name == "Query";
			}

			#endregion
		}

		internal class XmlResultSerializer : XmlObjectSerializer
		{
			#region Overrides

			public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
			{
				writer.WriteStartElement("QueryResult");
			}

			public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
			{
				new ResultSerializer(writer).Serialize((LinqServiceResult)graph);
			}

			public override void WriteEndObject(XmlDictionaryWriter writer)
			{
				writer.WriteEndElement();
			}

			public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
			{
				return new ResultDeserializer(reader).Deserialize();
			}

			public override bool IsStartObject(XmlDictionaryReader reader)
			{
				return reader.Name == "QueryResult";
			}

			#endregion
		}

		#region SerializerBase

		const int _paramIndex     = -1;
		const int _typeIndex      = -2;
		const int _typeArrayIndex = -3;

		class SerializerBase
		{
			protected readonly StringBuilder          Builder = new StringBuilder();
			protected readonly Dictionary<object,int> Dic     = new Dictionary<object,int>();
			protected int                             Index;

			protected void Append(Type type, object value)
			{
				Append(type);

				if (value == null)
					Append((string)null);
				else if (!type.IsArray)
					Append(Common.Convert.ToString(value));
				else
				{
					var elementType = type.GetElementType();

					Builder.Append(' ');

					var len = Builder.Length;
					var cnt = 0;

					if (elementType.IsArray)
						foreach (var val in (IEnumerable)value)
						{
							Append(elementType, val);
							cnt++;
						}
					else
						foreach (var val in (IEnumerable)value)
						{
							Append(val == null ? null : Common.Convert.ToString(val));
							cnt++;
						}

					Builder.Insert(len, cnt);
				}
			}

			protected void Append(int value)
			{
				Builder.Append(' ').Append(value);
			}

			protected void Append(Type value)
			{
				Builder.Append(' ').Append(value == null ? 0 : GetType(value));
			}

			protected void Append(bool value)
			{
				Builder.Append(' ').Append(value ? '1' : '0');
			}

			protected void Append(IQueryElement element)
			{
				Builder.Append(' ').Append(element == null ? 0 : Dic[element]);
			}

			protected void Append(string str)
			{
				Builder.Append(' ');

				if (str == null)
				{
					Builder.Append('-');
				}
				else if (str.Length == 0)
				{
					Builder.Append('0');
				}
				else
				{
					Builder
						.Append(str.Length)
						.Append(':')
						.Append(str);
				}
			}

			protected int GetType(Type type)
			{
				if (type == null)
					return 0;

				int idx;

				if (!Dic.TryGetValue(type, out idx))
				{
					if (type.IsArray)
					{
						var elementType = GetType(type.GetElementType());

						Dic.Add(type, idx = ++Index);

						Builder
							.Append(idx)
							.Append(' ')
							.Append(_typeArrayIndex)
							.Append(' ')
							.Append(elementType);
					}
					else
					{
						Dic.Add(type, idx = ++Index);

						Builder
							.Append(idx)
							.Append(' ')
							.Append(_typeIndex);

						Append(type.FullName);
					}

					Builder.AppendLine();
				}

				return idx;
			}
		}

		#endregion

		#region DeserializerBase

		class DeserializerBase
		{
			protected readonly Dictionary<int,object> Dic = new Dictionary<int,object>();

			protected string Str;
			protected int    Pos;

			char Peek()
			{
				return Str[Pos];
			}

			char Next()
			{
				return Str[++Pos];
			}

			bool Get(char c)
			{
				if (Peek() == c)
				{
					Pos++;
					return true;
				}

				return false;
			}

			protected int ReadInt()
			{
				Get(' ');

				var minus = Get('-');
				var value = 0;

				for (var c = Peek(); char.IsDigit(c); c = Next())
					value = value * 10 + (c - '0');

				return minus ? -value : value;
			}

			protected int? ReadCount()
			{
				Get(' ');

				if (Get('-'))
					return null;

				var value = 0;

				for (var c = Peek(); char.IsDigit(c); c = Next())
					value = value * 10 + (c - '0');

				return value;
			}

			protected string ReadString()
			{
				Get(' ');

				var c = Peek();

				if (c == '-')
				{
					Pos++;
					return null;
				}

				if (c == '0')
				{
					Pos++;
					return string.Empty;
				}

				var len   = ReadInt();
				var value = Str.Substring(++Pos, len);

				Pos += len;

				return value;
			}

			protected bool ReadBool()
			{
				Get(' ');

				var value = Peek() == '1';

				Pos++;

				return value;
			}

			protected T Read<T>()
				where T : class
			{
				var idx = ReadInt();
				return idx == 0 ? null : (T)Dic[idx];
			}

			protected T[] ReadArray<T>()
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

			protected List<T> ReadList<T>()
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

			protected void NextLine()
			{
				while (Pos < Str.Length && (Peek() == '\n' || Peek() == '\r'))
					Pos++;
			}

			interface IDeserializerHelper
			{
				object GetArray(DeserializerBase deserializer);
			}

			class DeserializerHelper<T> : IDeserializerHelper
			{
				public object GetArray(DeserializerBase deserializer)
				{
					var count = deserializer.ReadInt();
					var arr   = new T[count];
					var type  = typeof(T);

					for (var i = 0; i < count; i++)
						arr[i] = (T)deserializer.ReadValue(type);

					return arr;
				}
			}

			static readonly Dictionary<Type,Func<DeserializerBase,object>> _arrayDeserializers =
				new Dictionary<Type,Func<DeserializerBase,object>>();

			protected object ReadValue(Type type)
			{
				if (type == null)
					return ReadString();

				if (type.IsArray)
				{
					var elem = type.GetElementType();

					Func<DeserializerBase, object > deserializer;

					lock (_arrayDeserializers)
					{
						if (!_arrayDeserializers.TryGetValue(elem, out deserializer))
						{
							var helper = (IDeserializerHelper)Activator.CreateInstance(typeof(DeserializerHelper<>).MakeGenericType(elem));
							_arrayDeserializers.Add(elem, deserializer = helper.GetArray);
						}
					}

					return deserializer(this);
				}

				var str = ReadString();
				return Common.Convert.ChangeTypeFromString(str, type);
			}

			protected readonly List<string> UnresolvedTypes = new List<string>();

			protected Type ResolveType(string str)
			{
				if (str == null)
					return null;

				var type = Type.GetType(str, false);

				if (type == null)
				{
					type = LinqService.TypeResolver(str);

					if (type == null)
					{
						UnresolvedTypes.Add(str);

						Debug.WriteLine(
							string.Format("Type '{0}' cannot be resolved. Use LinqService.TypeResolver to resolve unknown types.", str),
							"LinqServiceSerializer");
					}
				}

				return type;
			}
		}

		#endregion

		#region QuerySerializer

		class QuerySerializer : SerializerBase
		{
			public QuerySerializer(XmlDictionaryWriter writer)
			{
				_writer = writer;
			}

			readonly XmlDictionaryWriter _writer;

			public void Serialize(LinqServiceQuery query)
			{
				var visitor = new QueryVisitor();

				visitor.Visit(query.Query, Visit);

				foreach (var parameter in query.Parameters)
					if (!Dic.ContainsKey(parameter))
						Visit(parameter);

				Builder
					.Append(++Index)
					.Append(' ')
					.Append(_paramIndex);

				Append(query.Parameters.Length);

				foreach (var parameter in query.Parameters)
					Append(parameter);

				Builder.AppendLine();

				_writer.WriteCData(Builder.ToString());
			}

			void Visit(IQueryElement e)
			{
				switch (e.ElementType)
				{
					case QueryElementType.SqlField :
						{
							var fld = (SqlField)e;

							if (fld != fld.Table.All)
							{
								GetType(fld.SystemType);

								if (fld.MemberMapper != null)
									GetType(fld.MemberMapper.MemberAccessor.TypeAccessor.OriginalType);
							}

							break;
						}

					case QueryElementType.SqlParameter :
						{
							var p = (SqlParameter)e;

							if (p.Value == null || p.SystemType.IsArray || !(p.Value is IEnumerable))
							{
								GetType(p.SystemType);
							}
							else
							{
								var elemType = TypeHelper.GetElementType(p.Value.GetType());
								GetType(GetArrayType(elemType));
							}

							//if (p.EnumTypes != null)
							//	foreach (var type in p.EnumTypes)
							//		GetType(type);

							break;
						}

					case QueryElementType.SqlFunction         : GetType(((SqlFunction)        e).SystemType); break;
					case QueryElementType.SqlExpression       : GetType(((SqlExpression)      e).SystemType); break;
					case QueryElementType.SqlBinaryExpression : GetType(((SqlBinaryExpression)e).SystemType); break;
					case QueryElementType.SqlDataType         : GetType(((SqlDataType)        e).Type);       break;
					case QueryElementType.SqlValue            : GetType(((SqlValue)           e).SystemType); break;
					case QueryElementType.SqlTable            : GetType(((SqlTable)           e).ObjectType); break;
				}

				Dic.Add(e, ++Index);

				Builder
					.Append(Index)
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

							Append(elem.Name);
							Append(elem.IsQueryParameter);

							if (elem.Value == null || elem.SystemType.IsArray || !(elem.Value is IEnumerable))
							{
								Append(elem.SystemType, elem.Value);
							}
							else
							{
								var elemType = TypeHelper.GetElementType(elem.Value.GetType());
								var value    = ConvertIEnumerableToArray(elem.Value, elemType);

								Append(GetArrayType(elemType), value);
							}

							/*
							if (elem.EnumTypes == null)
								Builder.Append(" -");
							else
							{
								Append(elem.EnumTypes.Count);

								foreach (var type in elem.EnumTypes)
									Append(type);
							}

							if (elem.TakeValues == null)
								Builder.Append(" -");
							else
							{
								Append(elem.TakeValues.Count);

								foreach (var type in elem.TakeValues)
									Append(type);
							}

							Append(elem.LikeStart);
							Append(elem.LikeEnd);
							*/

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
								Builder.Append(" -");
							else
							{
								Append(elem.SequenceAttributes.Length);

								foreach (var a in elem.SequenceAttributes)
								{
									Append(a.ProviderName);
									Append(a.SequenceName);
								}
							}

							Append(Dic[elem.All]);
							Append(elem.Fields.Count);

							foreach (var field in elem.Fields)
								Append(Dic[field.Value]);

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
								Builder.Append(" -");
							else
								Append(elem.Unions);

							Append(elem.Parameters);

							if (Dic.ContainsKey(elem.All))
								Append(Dic[elem.All]);
							else
								Builder.Append(" -");

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

				Builder.AppendLine();
			}

			void Append<T>(ICollection<T> exprs)
				where T : IQueryElement
			{
				if (exprs == null)
					Builder.Append(" -");
				else
				{
					Append(exprs.Count);

					foreach (var e in exprs)
						Append(Dic[e]);
				}
			}
		}

		#endregion

		#region QueryDeserializer

		class QueryDeserializer : DeserializerBase
		{
			public QueryDeserializer(XmlDictionaryReader reader)
			{
				_reader = reader;
			}

			readonly XmlDictionaryReader _reader;

			SqlQuery       _query;
			SqlParameter[] _parameters;

			readonly Dictionary<int,SqlQuery> _queries = new Dictionary<int,SqlQuery>();
			readonly List<Action>             _actions = new List<Action>();

			public LinqServiceQuery Deserialize()
			{
				Str = _reader.ReadString();

				while (Parse()) {}

				foreach (var action in _actions)
					action();

				var query = new LinqServiceQuery { Query = _query, Parameters = _parameters};

				return query;
			}

			bool Parse()
			{
				NextLine();

				if (Pos >= Str.Length)
					return false;

				var obj  = null as object;
				var idx  = ReadInt(); Pos++;
				var type = ReadInt(); Pos++;

				switch (type)
				{
					case _paramIndex     : obj = _parameters = ReadArray<SqlParameter>(); break;
					case _typeIndex      : obj = ResolveType(ReadString());               break;
					case _typeArrayIndex : obj = GetArrayType(Read<Type>());              break;

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

					case (int)QueryElementType.SqlParameter :
						{
							var name             = ReadString();
							var isQueryParameter = ReadBool();
							var systemType       = Read<Type>();
							var value            = ReadValue(systemType);
							//var enumTypes        = ReadList<Type>();
							//var takeValues       = null as List<int>;

							/*
							var count = ReadCount();

							if (count != null)
							{
								takeValues = new List<int>(count.Value);

								for (var i = 0; i < count; i++)
									takeValues.Add(ReadInt());
							}

							var likeStart = ReadString();
							var likeEnd   = ReadString();
							*/

							obj = new SqlParameter(systemType, name, value)
							{
								IsQueryParameter = isQueryParameter,
								//EnumTypes        = enumTypes,
								//TakeValues       = takeValues,
								//LikeStart        = likeStart,
								//LikeEnd          = likeEnd,
							};

							/*
							if (enumTypes != null && UnresolvedTypes.Count > 0)
								foreach (var et in enumTypes)
									if (et == null)
										throw new LinqException(
											"Query cannot be deserialized. The possible reason is that the deserializer could not resolve the following types: {0}. Use LinqService.TypeResolver to resolve types.",
											string.Join(", ", UnresolvedTypes.Select(_ => "'" + _ + "'").ToArray()));
							*/

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

							var all    = Read<SqlField>();
							var fields = ReadArray<SqlField>();
							var flds   = new SqlField[fields.Length + 1];

							flds[0] = all;
							Array.Copy(fields, 0, flds, 1, fields.Length);

							obj = new SqlTable(sourceID, name, alias, database, owner, physicalName, objectType, sequenceAttributes, flds);

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

							query.All = Read<SqlField>();

							obj = query;

							break;
						}

					case (int)QueryElementType.Column :
						{
							var sid         = ReadInt();
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

					case (int)QueryElementType.SetExpression : obj = new SqlQuery.SetExpression(Read<ISqlExpression>(), Read<ISqlExpression>()); break;
					case (int)QueryElementType.FromClause    : obj = new SqlQuery.FromClause(ReadArray<SqlQuery.TableSource>());                 break;
					case (int)QueryElementType.WhereClause   : obj = new SqlQuery.WhereClause(Read<SqlQuery.SearchCondition>());                 break;
					case (int)QueryElementType.GroupByClause : obj = new SqlQuery.GroupByClause(ReadArray<ISqlExpression>());                    break;
					case (int)QueryElementType.OrderByClause : obj = new SqlQuery.OrderByClause(ReadArray<SqlQuery.OrderByItem>());              break;

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

				Dic.Add(idx, obj);

				return true;
			}
		}

		#endregion

		#region ResultSerializer

		class ResultSerializer : SerializerBase
		{
			public ResultSerializer(XmlDictionaryWriter writer)
			{
				_writer = writer;
			}

			readonly XmlDictionaryWriter _writer;

			public void Serialize(LinqServiceResult result)
			{
				Append(result.FieldCount);
				Append(result.RowCount);
				Append(result.QueryID.ToString());

				Builder.AppendLine();

				foreach (var name in result.FieldNames)
				{
					Append(name);
					Builder.AppendLine();
				}

				foreach (var type in result.FieldTypes)
				{
					Append(type.FullName);
					Builder.AppendLine();
				}

				foreach (var data in result.Data)
				{
					foreach (var str in data)
						Append(str);

					Builder.AppendLine();
				}

				_writer.WriteCData(Builder.ToString());
			}
		}

		#endregion

		#region ResultDeserializer

		class ResultDeserializer : DeserializerBase
		{
			public ResultDeserializer(XmlDictionaryReader reader)
			{
				_reader = reader;
			}

			readonly XmlDictionaryReader _reader;

			public LinqServiceResult Deserialize()
			{
				Str = _reader.ReadString();

				var fieldCount = ReadInt();

				var result = new LinqServiceResult
				{
					FieldCount = fieldCount,
					RowCount   = ReadInt(),
					QueryID    = new Guid(ReadString()),
					FieldNames = new string[fieldCount],
					FieldTypes = new Type  [fieldCount],
					Data       = new List<string[]>(),
				};

				NextLine();

				for (var i = 0; i < fieldCount; i++) { result.FieldNames[i] = ReadString();              NextLine(); }
				for (var i = 0; i < fieldCount; i++) { result.FieldTypes[i] = ResolveType(ReadString()); NextLine(); }

				for (var n = 0; n < result.RowCount; n++)
				{
					var data = new string[fieldCount];

					for (var i = 0; i < fieldCount; i++)
						data[i] = ReadString();

					result.Data.Add(data);

					NextLine();
				}

				return result;
			}
		}

		#endregion

		#region Helpers

		interface IArrayHelper
		{
			Type   GetArrayType();
			object ConvertToArray(object list);
		}

		class ArrayHelper<T> : IArrayHelper
		{
			public Type GetArrayType()
			{
				return typeof(T[]);
			}

			public object ConvertToArray(object list)
			{
				return ((IEnumerable<T>)list).ToArray();
			}
		}

		static readonly Dictionary<Type,Type>                 _arrayTypes      = new Dictionary<Type,Type>();
		static readonly Dictionary<Type,Func<object,object >> _arrayConverters = new Dictionary<Type,Func<object,object>>();

		static Type GetArrayType(Type elementType)
		{
			Type arrayType;

			lock (_arrayTypes)
			{
				if (!_arrayTypes.TryGetValue(elementType, out arrayType))
				{
					var helper = (IArrayHelper)Activator.CreateInstance(typeof(ArrayHelper<>).MakeGenericType(elementType));
					_arrayTypes.Add(elementType, arrayType = helper.GetArrayType());
				}
			}

			return arrayType;
		}

		static object ConvertIEnumerableToArray(object list, Type elementType)
		{
			Func<object,object> converter;

			lock (_arrayConverters)
			{
				if (!_arrayConverters.TryGetValue(elementType, out converter))
				{
					var helper = (IArrayHelper)Activator.CreateInstance(typeof(ArrayHelper<>).MakeGenericType(elementType));
					_arrayConverters.Add(elementType, converter = helper.ConvertToArray);
				}
			}

			return converter(list);
		}

		#endregion
	}
}
