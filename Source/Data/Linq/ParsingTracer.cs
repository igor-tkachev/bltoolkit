using System;
using System.Diagnostics;
using System.Linq.Expressions;

using BLToolkit.Data.Sql;

namespace BLToolkit.Data.Linq
{
	static class ParsingTracer
	{
		[Conditional("TRACE_PARSING")]
		public static void IncIndentLevel()
		{
			Debug.IndentLevel++;
		}

		[Conditional("TRACE_PARSING")]
		public static void DecIndentLevel()
		{
			Debug.IndentLevel--;
		}

		[Conditional("TRACE_PARSING")]
		static void WriteLineInternal()
		{
			Debug.WriteLine("", GetMethodName());
		}

		[Conditional("TRACE_PARSING")]
		static void WriteLineInternal(string format, params object[] args)
		{
			Debug.WriteLine(string.Format(format, args), GetMethodName());
		}

		[Conditional("TRACE_PARSING")]
		static void WriteLineInternal(string prefix, Expression expr)
		{
			if (expr != null)
				Debug.WriteLine(prefix + " - " + expr + " : Expression." + expr.GetType().Name, GetMethodName());
		}

		[Conditional("TRACE_PARSING")]
		static void WriteLineInternal(string prefix, QueryField field)
		{
			if (field != null)
				Debug.WriteLine(prefix + " - " + field + " : QueryField." + field.GetType().Name, GetMethodName());
		}

		[Conditional("TRACE_PARSING")]
		static void WriteLineInternal(string prefix, params QuerySource[] source)
		{
			if (source.Length != 0)
				for (var i = 0; i < source.Length; i++)
					Debug.WriteLine(
						prefix + (source.Length == 1 ? "" : i.ToString()) + " - " + source[i] + " : QuerySource." + source[i].GetType().Name,
						GetMethodName());
		}

		[Conditional("TRACE_PARSING")]
		static void WriteLineInternal(string prefix, LambdaInfo lambda)
		{
			if (lambda != null)
				Debug.WriteLine(prefix + " - " + lambda.Body + " : Expression." + lambda.Body.GetType().Name, GetMethodName());
		}

		[Conditional("TRACE_PARSING")]
		static void WriteLineInternal(string prefix, SqlQuery sql)
		{
			if (sql != null)
			{
				var str = sql.ToString().Replace('\t', ' ').Replace('\n', ' ');

				for (var len = str.Length; len != (str = str.Replace("  ", " ")).Length; len = str.Length)
				{
				}

				Debug.WriteLine(prefix + " - " + str, GetMethodName());
			}
		}

		[Conditional("TRACE_PARSING")] public static void WriteLine() { WriteLineInternal(); }
		[JetBrains.Annotations.StringFormatMethod("format")]
		[Conditional("TRACE_PARSING")] public static void WriteLine(string format, params object[] args) { WriteLineInternal(format, args);     }

		[Conditional("TRACE_PARSING")] public static void WriteLine(               Expression    expr)   { WriteLineInternal("expr",   expr);   }
		[Conditional("TRACE_PARSING")] public static void WriteLine(               QueryField    field)  { WriteLineInternal("field",  field);  }
		[Conditional("TRACE_PARSING")] public static void WriteLine(params         QuerySource[] source) { WriteLineInternal("query",  source); }
		[Conditional("TRACE_PARSING")] public static void WriteLine(               SqlQuery      sql)    { WriteLineInternal("sql",    sql);    }
		[Conditional("TRACE_PARSING")] public static void WriteLine(               LambdaInfo    lambda) { WriteLineInternal("lambda", lambda); }

		[Conditional("TRACE_PARSING")] public static void WriteLine(string prefix, Expression    expr)   { WriteLineInternal(prefix,   expr);   }
		[Conditional("TRACE_PARSING")] public static void WriteLine(string prefix, QueryField    field)  { WriteLineInternal(prefix,   field);  }
		[Conditional("TRACE_PARSING")] public static void WriteLine(string prefix, QuerySource   source) { WriteLineInternal(prefix,   source); }
		[Conditional("TRACE_PARSING")] public static void WriteLine(string prefix, LambdaInfo    lambda) { WriteLineInternal(prefix,   lambda); }
		[Conditional("TRACE_PARSING")] public static void WriteLine(string prefix, SqlQuery      sql)    { WriteLineInternal(prefix,   sql);    }

		static string GetMethodName()
		{
#if TRACE_PARSING
			var method = new StackTrace().GetFrame(3).GetMethod();
			var name   = method.Name;

			if (method.IsConstructor)
			{
				name = method.DeclaringType.Name;

				if (method.DeclaringType.IsNested)
				{
					name = method.DeclaringType.DeclaringType.Name + "." + name;
				}
			}

			return name;
#else
			return "";
#endif
		}
	}
}
