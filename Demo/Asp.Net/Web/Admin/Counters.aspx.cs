using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.UI;

using BLToolkit.Aspects;

public partial class Admin_Counters : Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Request["cleanup"] == "1")
		{
			CacheAspect.ClearCache();
			Response.Redirect("Counters.aspx");
		}

		List<MethodCallCounter> counters = new List<MethodCallCounter>();

		lock (CounterAspect.Counters.SyncRoot)
			foreach (MethodCallCounter c in CounterAspect.Counters)
				lock (c.CurrentCalls.SyncRoot)
					if (c.TotalCount > 0 || c.CachedCount > 0 | c.CurrentCalls.Count > 0)
						counters.Add(c);

		counters.Sort(delegate(MethodCallCounter x, MethodCallCounter y)
		{
			int c = string.Compare(
				x.MethodInfo.DeclaringType.Name,
				y.MethodInfo.DeclaringType.Name);

			if (c != 0)
				return c;

			return string.Compare(x.MethodInfo.Name, y.MethodInfo.Name);
		});

		counterRepeater.DataSource = counters;

		DataBind();
	}

	protected string GetName(Type type)
	{
		string name = type.Name;

		if (type.IsGenericType || type.Name.IndexOf('_') > 0 && type.BaseType.IsGenericType)
		{
			if (!type.IsGenericType)
				type = type.BaseType;

			name = type.Name.Split('`')[0] + "&lt;";

			foreach (Type t in type.GetGenericArguments())
				name += GetName(t) + ",";

			name = name.TrimEnd(',') + "&gt;";
		}

		return name;
	}

	protected string GetTime(TimeSpan time)
	{
		if (time == TimeSpan.MinValue || time == TimeSpan.MaxValue)
			return "";

		string s = time.ToString();

		if (s.Length > 12)
			s = s.Substring(0, 12);

		if (time.TotalSeconds <= 1)
			s = string.Format("<font color=gray>{0}</font>", s);

		return s;
	}

	protected string GetCurrent(MethodCallCounter counter)
	{
		List<InterceptCallInfo> info = new List<InterceptCallInfo>();

		lock (counter.CurrentCalls.SyncRoot)
		{
			if (counter.CurrentCalls.Count == 0)
				return "";

			foreach (InterceptCallInfo c in counter.CurrentCalls)
				info.Add(c);
		}

		StringBuilder sb = new StringBuilder();

		sb.Append("<tr><td colspan=8 align=left><table class='grid' cellspacing=0 cellpadding=0 rules=all border=1 style='border-collapse:collapse'>");

		sb.Append("<tr class='gridheader'>");
		sb.Append("<td>Time</td>");
		sb.Append("<td>Login</td>");

		foreach (ParameterInfo pi in counter.MethodInfo.GetParameters())
			sb.AppendFormat("<td>{0}</td>", pi.Name);

		sb.Append("</tr>");

		foreach (InterceptCallInfo c in counter.CurrentCalls)
		{
			sb.AppendFormat("<tr>");

			sb.AppendFormat("<td>{0}</td>", DateTime.Now - c.BeginCallTime);
			sb.AppendFormat("<td>{0}</td>", c.CurrentPrincipal.Identity.Name);

			foreach (object value in c.ParameterValues)
			{
				sb.AppendFormat("<td>");

				sb.Append(
					value == null ? "<null>" :
					value is string ? "\"" + value.ToString().Replace("\n", "<br>\n") + "\"" :
					value is char ? "'" + value + "'" :
					value.ToString());

				sb.AppendFormat("</td>");
			}

			sb.AppendFormat("</tr>");
		}

		sb.Append("</td></tr></table>");

		return sb.ToString();
	}
}
