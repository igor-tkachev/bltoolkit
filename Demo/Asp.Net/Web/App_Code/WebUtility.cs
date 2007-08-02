using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Configuration;

using PetShop.BusinessLogic;

namespace PetShop.Web
{
	/// <summary>
	/// Collection of utility methods for web tier
	/// </summary>
	public static class WebUtility
	{
		/// <summary>
		/// Method to make sure that user's inputs are not malicious
		/// </summary>
		/// <param name="text">User's Input</param>
		/// <param name="maxLength">Maximum length of input</param>
		/// <returns>The cleaned up version of the input</returns>
		public static string InputText(string text, int maxLength)
		{
			text = text.Trim();

			if (string.IsNullOrEmpty(text))
				return string.Empty;

			text = Regex.Replace(text, "[\\s]{2,}", " ");                             // two or more spaces
			text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n"); // <br>
			text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");     // &nbsp;
			text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);                  // any other tags
			text = text.Replace("'", "''");

			if (text.Length > maxLength)
				text = text.Substring(0, maxLength);

			return text;
		}

		/// <summary>
		/// Method to check whether input has other characters than numbers
		/// </summary>
		public static string CleanNonWord(string text)
		{
			return Regex.Replace(text, "\\W", "");
		}

		/// <summary>
		/// Method to redirect user to search page
		/// </summary>
		/// <param name="key">Search keyword</param> 
		public static void SearchRedirect(string key)
		{
			HttpContext.Current.Response.Redirect(
				string.Format("~/Search.aspx?keywords={0}", InputText(key, 255)));
		}
	}
}
