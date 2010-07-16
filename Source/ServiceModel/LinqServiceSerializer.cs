using System;
using System.Collections.Generic;
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
					case QueryElementType.SqlField:
						{
							var fld = (SqlField) e;

							if (fld == fld.Table.All)
								return;

							GetType(fld.SystemType);

							if (fld.MemberMapper != null)
								GetType(fld.MemberMapper.MemberAccessor.TypeAccessor.OriginalType);

							break;
						}

					case QueryElementType.SqlTable:
						GetType(((SqlTable) e).ObjectType);
						break;
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

					case QueryElementType.SqlValue :
						{
							var elem = (SqlValue)e;
							Append(elem.SystemType, elem.Value);
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

					case QueryElementType.SqlQuery :
						{
							var elem = (SqlQuery) e;

							Append(elem.SourceID);
							Append((int)elem.QueryType);

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
							{
								Append(elem.Unions.Count);

								foreach (var item in elem.Unions)
									Append(_dic[item]);
							}

							Append(elem.Parameters.Count);

							foreach (var item in elem.Parameters)
								Append(_dic[item]);

							break;
						}

					case QueryElementType.Column :
						{
							var elem = (SqlQuery.Column)e;

							Append(elem.Parent.SourceID);
							Append(elem.Expression);
							Append(elem._alias);

							break;
						}

					case QueryElementType.SearchCondition :
						{
							var elem = (SqlQuery.SearchCondition)e;

							Append(elem.Conditions.Count);

							foreach (var item in elem.Conditions)
								Append(_dic[item]);

							break;
						}

					case QueryElementType.Condition :
						{
							var elem = (SqlQuery.Condition)e;

							Append(elem.IsNot);
							Append(elem.Predicate);
							Append(elem.IsOr);

							break;
						}

					case QueryElementType.SetExpression :
						{
							var elem = (SqlQuery.SetExpression)e;

							Append(elem.Column);
							Append(elem.Expression);

							break;
						}

					case QueryElementType.SetClause :
						{
							var elem = (SqlQuery.SetClause)e;

							Append(elem.Items.Count);

							foreach (var item in elem.Items)
								Append(item);

							Append(elem.Into);
							Append(elem.WithIdentity);

							break;
						}

					case QueryElementType.FromClause :
						{
							var elem = (SqlQuery.FromClause)e;

							Append(elem.Tables.Count);

							foreach (var item in elem.Tables)
								Append(item);

							break;
						}

					case QueryElementType.WhereClause :
						{
							var elem = (SqlQuery.WhereClause)e;
							Append(elem.SearchCondition);
							break;
						}

					case QueryElementType.GroupByClause :
						{
							var elem = (SqlQuery.GroupByClause)e;

							Append(elem.Items.Count);

							foreach (var item in elem.Items)
								Append(item);

							break;
						}

					case QueryElementType.OrderByClause :
						{
							var elem = (SqlQuery.OrderByClause)e;

							Append(elem.Items.Count);

							foreach (var item in elem.Items)
								Append(item);

							break;
						}
				}

				_sb.AppendLine();
			}

			#region Helpers

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

					case (int) QueryElementType.SqlField:
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

					case (int) QueryElementType.SqlValue:
						{
							var systemType = Read<Type>();
							var value = ReadValue(systemType);

							obj = new SqlValue(systemType, value);

							break;
						}

					case (int) QueryElementType.SqlTable:
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

					case (int) QueryElementType.Condition:
						obj = new SqlQuery.Condition(ReadBool(), Read<ISqlPredicate>(), ReadBool());
						break;

					case (int) QueryElementType.SetExpression:
						obj = new SqlQuery.SetExpression(Read<ISqlExpression>(), Read<ISqlExpression>());
						break;

					case (int) QueryElementType.SetClause:
						{
							var items = ReadArray<SqlQuery.SetExpression>();
							var into  = Read<SqlTable>();
							var wid   = ReadBool();

							var c = new SqlQuery.SetClause {Into = into, WithIdentity = wid};

							c.Items.AddRange(items);
							obj = c;

							break;
						}

					case (int) QueryElementType.FromClause:
						obj = new SqlQuery.FromClause(ReadArray<SqlQuery.TableSource>());
						break;

					case (int) QueryElementType.WhereClause:
						obj = new SqlQuery.WhereClause(Read<SqlQuery.SearchCondition>());
						break;

					case (int)QueryElementType.GroupByClause :
						obj = new SqlQuery.GroupByClause(ReadArray<ISqlExpression>());
						break;

					case (int)QueryElementType.OrderByClause :
						obj = new SqlQuery.OrderByClause(ReadArray<SqlQuery.OrderByItem>());
						break;
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

				for (var i = 0; i < items.Length; i++)
					items[i] = Read<T>();

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
