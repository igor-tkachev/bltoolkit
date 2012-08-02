using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

using BLToolkit.Linq;

namespace BLToolkit.Data.Linq.Builder
{
	class ExpressionTestGenerator
	{
		readonly StringBuilder _exprBuilder = new StringBuilder();

		string _indent = "\t\t\t\t";

		void PushIndent() { _indent += '\t'; }
		void PopIndent () { _indent = _indent.Substring(1); }

		readonly HashSet<Expression> _visitedExprs = new HashSet<Expression>();

		bool BuildExpression(Expression expr)
		{
			switch (expr.NodeType)
			{
				case ExpressionType.Call :
					{
						var ex = (MethodCallExpression)expr;
						var mi = ex.Method;

						var attrs = mi.GetCustomAttributes(typeof(ExtensionAttribute), false);

						if (attrs.Length != 0)
						{
							ex.Arguments[0].Visit(new Func<Expression,bool>(BuildExpression));
							PushIndent();
							_exprBuilder.AppendLine().Append(_indent);
						}
						else if (ex.Object != null)
							ex.Visit(new Func<Expression,bool>(BuildExpression));
						else
							_exprBuilder.Append(GetTypeName(mi.DeclaringType));

						_exprBuilder.Append(".").Append(mi.Name);

						if (mi.IsGenericMethod && mi.GetGenericArguments().Select(GetTypeName).All(t => t != null))
						{
							_exprBuilder
								.Append("<")
								.Append(GetTypeNames(mi.GetGenericArguments(), ","))
								.Append(">");
						}

						_exprBuilder.Append("(");

						PushIndent();

						var n = attrs.Length != 0 ? 1 : 0;

						for (var i = n; i < ex.Arguments.Count; i++)
						{
							if (i != n)
								_exprBuilder.Append(",");

							_exprBuilder.AppendLine().Append(_indent);

							ex.Arguments[i].Visit(new Func<Expression,bool>(BuildExpression));
						}

						PopIndent();

						_exprBuilder.Append(")");

						if (attrs.Length != 0)
						{
							PopIndent();
						}

						return false;
					}

				case ExpressionType.Constant:
					{
						var c = (ConstantExpression)expr;

						if (c.Value is IQueryable)
						{
							var e = ((IQueryable)c.Value).Expression;

							if (_visitedExprs.Add(e))
							{
								e.Visit(new Func<Expression,bool>(BuildExpression));
								return false;
							}
						}

						_exprBuilder.Append(expr);

						return true;
					}

				case ExpressionType.Lambda:
					{
						var le = (LambdaExpression)expr;
						var ps = le.Parameters
							.Select(p => (GetTypeName(p.Type) + " " + p.Name).TrimStart())
							.Aggregate((p1, p2) => p1 + ", " + p2);

						_exprBuilder.Append("(").Append(ps).Append(") => ");

						le.Body.Visit(new Func<Expression,bool>(BuildExpression));
						return false;
					}

				case ExpressionType.New:
					{
						var ne = (NewExpression)expr;

						

						return false;
					}

				case ExpressionType.Quote :
					return true;

				default:
					_exprBuilder.AppendLine("// Unknown expression.").Append(_indent).Append(expr);
					return false;
			}
		}

		readonly Dictionary<Type,string> _typeNames = new Dictionary<Type,string>
		{
			{ typeof(object), "object" },
			{ typeof(bool),   "bool"   },
			{ typeof(int),    "int"    },
			{ typeof(string), "string" },
		};

		readonly StringBuilder _typeBuilder = new StringBuilder();

		void BuildType(Type type)
		{
			if (type.Namespace != null && type.Namespace.StartsWith("System") ||
				type.Name.StartsWith("<>f__AnonymousType")                    ||
				type.Assembly == GetType().Assembly                           ||
				type.IsGenericType && type.GetGenericTypeDefinition() != type)
				return;

			var name = type.Name;//.Replace('<', '_').Replace('>', '_').Replace('`', '_').Replace("__f__", "");

			var idx = name.LastIndexOf("`");

			if (idx > 0)
				name = name.Substring(0, idx);

			if (type.IsGenericType)
				type = type.GetGenericTypeDefinition();

			var baseClasses = new[] { type.BaseType }
				.Where(t => t != null && t != typeof(object))
				.Concat(type.GetInterfaces()).ToArray();

			var ctors = type.GetConstructors().Select(c =>
			{
#if SILVERLIGHT
				var attrs = c.GetCustomAttributes(false).ToList();
#else
				var attrs = c.GetCustomAttributesData();
#endif
				var ps    = c.GetParameters().Select(p => GetTypeName(p.ParameterType) + " " + p.Name).ToArray();
				return string.Format(
					"{0}\n\t\tpublic {1}({2})\n\t\t{{\n\t\t\tthrow new NotImplementedException();\n\t\t}}",
					attrs.Count > 0 ? attrs.Select(a => "\n\t\t" + a.ToString()).Aggregate((a1,a2) => a1 + a2) : "",
					name,
					ps.Length == 0 ? "" : ps.Aggregate((s,t) => s + ", " + t));
			}).ToList();

			if (ctors.Count == 1 && ctors[0].IndexOf("()") >= 0)
				ctors.Clear();

			var members = type.GetFields().Intersect(_usedMembers.OfType<FieldInfo>()).Select(f =>
			{
#if SILVERLIGHT
				var attrs = f.GetCustomAttributes(false).ToList();
#else
				var attrs = f.GetCustomAttributesData();
#endif
				return string.Format(
					"{0}\n\t\tpublic {1} {2};",
					attrs.Count > 0 ? attrs.Select(a => "\n\t\t" + a.ToString()).Aggregate((a1,a2) => a1 + a2) : "",
					GetTypeName(f.FieldType),
					f.Name);
			})
			.Concat(
				type.GetProperties().Intersect(_usedMembers.OfType<PropertyInfo>()).Select(p =>
				{
#if SILVERLIGHT
					var attrs = p.GetCustomAttributes(false).ToList();
#else
					var attrs = p.GetCustomAttributesData();
#endif
					return string.Format(
						"{0}\n\t\t{3}{1} {2} {{ get; set; }}",
						attrs.Count > 0 ? attrs.Select(a => "\n\t\t" + a.ToString()).Aggregate((a1,a2) => a1 + a2) : "",
						GetTypeName(p.PropertyType),
						p.Name,
						type.IsInterface ? "" : "public ");
				}))
			.Concat(
				type.GetMethods().Intersect(_usedMembers.OfType<MethodInfo>()).Select(m =>
				{
#if SILVERLIGHT
					var attrs = m.GetCustomAttributes(false).ToList();
#else
					var attrs = m.GetCustomAttributesData();
#endif
					var ps    = m.GetParameters().Select(p => GetTypeName(p.ParameterType) + " " + p.Name).ToArray();
					return string.Format(
						"{0}\n\t\t{5}{4}{1} {2}({3})\n\t\t{{\n\t\t\tthrow new NotImplementedException();\n\t\t}}",
						attrs.Count > 0 ? attrs.Select(a => "\n\t\t" + a.ToString()).Aggregate((a1,a2) => a1 + a2) : "",
						GetTypeName(m.ReturnType),
						m.Name,
						ps.Length == 0 ? "" : ps.Aggregate((s,t) => s + ", " + t),
						m.IsStatic   ? "static "   :
						m.IsVirtual  ? "virtual "  :
						m.IsAbstract ? "abstract " :
						               "",
						type.IsInterface ? "" : "public ");
				}))
			.ToArray();

			{
#if SILVERLIGHT
				var attrs = type.GetCustomAttributes(false).ToList();
#else
				var attrs = type.GetCustomAttributesData();
#endif

				_typeBuilder.AppendFormat(
					type.IsGenericType ?
@"
namespace {0}
{{{8}
	{6}{7}{1} {2}<{3}>{5}
	{{{4}{9}
	}}
}}
"
:
@"
namespace {0}
{{{8}
	{6}{7}{1} {2}{5}
	{{{4}{9}
	}}
}}
",
					type.Namespace,
					type.IsInterface ? "interface" : type.IsClass ? "class" : "struct",
					name,
					type.IsGenericType ? GetTypeNames(type.GetGenericArguments(), ",") : null,
					ctors.Count == 0 ? "" : ctors.Aggregate((s,t) => s + "\n" + t),
					baseClasses.Length == 0 ? "" : " : " + GetTypeNames(baseClasses),
					type.IsPublic ? "public " : "",
					type.IsAbstract && !type.IsInterface ? "abstract " : "",
					attrs.Count > 0 ? attrs.Select(a => "\n\t" + a.ToString()).Aggregate((a1,a2) => a1 + a2) : "",
					members.Length > 0 ?
						(ctors.Count != 0 ? "\n" : "") + members.Aggregate((f1,f2) => f1 + "\n" + f2) :
						""
					);
			}
		}

		string GetTypeNames(IEnumerable<Type> types, string separator = ", ")
		{
			return types.Select(GetTypeName).Aggregate((t1,t2) => t1 + separator + t2);
		}

		string GetTypeName(Type type)
		{
			if (type == null || type == typeof(object))
				return null;

			if (type.IsGenericParameter)
				return type.ToString();

			string name;

			if (_typeNames.TryGetValue(type, out name))
				return name;

			if (type.Name.StartsWith("<>f__AnonymousType"))
			{
				_typeNames[type] = null;
				return null;
			}

			if (type.IsGenericType)
			{
				var args = type.GetGenericArguments();

				name = "";

				if (type.Namespace != "System")
					name = type.Namespace + ".";

				name += type.Name;

				var idx = name.LastIndexOf("`");

				if (idx > 0)
					name = name.Substring(0, idx);

				name = string.Format("{0}<{1}>",
					name,
					args.Select(GetTypeName).Aggregate((s,t) => s + "," + t));

				_typeNames[type] = name;

				return name;
			}

			if (type.Namespace == "System")
				return type.Name;

			return type.ToString();
		}

		readonly HashSet<MemberInfo> _usedMembers = new HashSet<MemberInfo>();

		void VisitMembers(Expression expr)
		{
			switch (expr.NodeType)
			{
				case ExpressionType.Call :
					{
						var ex = (MethodCallExpression)expr;
						_usedMembers.Add(ex.Method);

						if (ex.Method.IsGenericMethod)
						{
							var gmd = ex.Method.GetGenericMethodDefinition();

							if (gmd != ex.Method)
								_usedMembers.Add(gmd);
						}

						break;
					}

				case ExpressionType.MemberAccess :
					{
						var ex = (MemberExpression)expr;
						_usedMembers.Add(ex.Member);
						break;
					}

				case ExpressionType.MemberInit :
					{
						var ex = (MemberInitExpression)expr;

						Action<IEnumerable<MemberBinding>> visit = null; visit = bs =>
						{
							foreach (var b in bs)
							{
								_usedMembers.Add(b.Member);

								switch (b.BindingType)
								{
									case MemberBindingType.MemberBinding :
										visit(((MemberMemberBinding)b).Bindings);
										break;
								}}
						};

						visit(ex.Bindings);
						break;
					}
			}
		}

		readonly HashSet<Type> _usedTypes = new HashSet<Type>();

		void AddType(Type type)
		{
			if (type == null || type == typeof(object) || type.IsGenericParameter || _usedTypes.Contains(type))
				return;

			_usedTypes.Add(type);

			if (type.IsGenericType)
				foreach (var arg in type.GetGenericArguments())
					AddType(arg);

			if (type.IsGenericType && type.GetGenericTypeDefinition() != type)
				AddType(type.GetGenericTypeDefinition());

			AddType(type.BaseType);

			foreach (var i in type.GetInterfaces())
				AddType(i);
		}

		void VisitTypes(Expression expr)
		{
			AddType(expr.Type);

			switch (expr.NodeType)
			{
				case ExpressionType.Call :
					{
						var ex = (MethodCallExpression)expr;
						var mi = ex.Method;

						AddType(mi.DeclaringType);
						AddType(mi.ReturnType);

						foreach (var arg in mi.GetGenericArguments())
							AddType(arg);

						break;
					}
			}
		}

		public string GenerateSource(Expression expr)
		{
			string       fileName = null;
			StreamWriter sw       = null;

			try
			{
				var dir = Path.Combine(Path.GetTempPath(), "bltookit\\");

				if (!Directory.Exists(dir))
					Directory.CreateDirectory(dir);

				var number = DateTime.Now.Ticks;

				fileName = Path.Combine(dir, "ExpressionTest." + number  + ".cs");

				expr.Visit(new Action<Expression>(VisitMembers));
				expr.Visit(new Action<Expression>(VisitTypes));

				foreach (var type in _usedTypes.OrderBy(t => t.Namespace).ThenBy(t => t.Name))
					BuildType(type);

				expr.Visit(new Func<Expression,bool>(BuildExpression));

				_exprBuilder.Replace("<>h__TransparentIdentifier", "tp");

				sw = File.CreateText(fileName);

				sw.WriteLine(@"//---------------------------------------------------------------------------------------------------
// This code was generated by BLToolkit.
//---------------------------------------------------------------------------------------------------
using System;
using System.Linq.Expressions;

using NUnit.Framework;
{0}
namespace Data.Linq
{{
	[TestFixture]
	public class UserTest : TestBase
	{{
		[Test]
		public void Test()
		{{
			// {1}
			ForEachProvider(db =>
				{2});
		}}
	}}
}}
",
					_typeBuilder,
					expr,
					_exprBuilder);
			}
			catch(Exception ex)
			{
				if (sw != null)
				{
					sw.WriteLine();
					sw.WriteLine(ex.GetType());
					sw.WriteLine(ex.Message);
					sw.WriteLine(ex.StackTrace);
				}
			}
			finally
			{
				if (sw != null)
					sw.Close();
			}

			return fileName;
		}
	}
}
