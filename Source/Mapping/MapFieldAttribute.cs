/*
 * File:    MapFieldAttribute.cs
 * Created: 03/17/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// Is applied to any members that should be mapped. 
	/// </summary>
	/// <example>
	/// The following example demonstrates how to use the attribute.
	/// <code>
	/// using System;
	/// 
	/// using Rsdn.Framework.Data;
	/// 
	/// namespace Example
	/// {
	///     public class Category
	///     {
	///         [MapField(Name = "CategoryID")]
	///         public int    ID;
	///         
	///         public string CategoryName;
	///         
	///         [MapField(IsNullable = true)]
	///         public string Description;
	///     }
	///     
	///     class Test
	///     {
	///         static void Main()
	///         {
	///             using (DbManager db = new DbManager())
	///             {
	///                 db.ExecuteList(
	///                     typeof(Category), @"
	///                     SELECT
	///                         CategoryID,
	///                         CategoryName,
	///                         Description
	///                     FROM Categories");
	///             }
	///         }
	///     }
	/// }
	/// </code>
	/// </example>
	[AttributeUsage(
		AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class,
		AllowMultiple = true
	)]
	public class MapFieldAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MapFieldAttribute"/> class.
		/// </summary>
		public MapFieldAttribute()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourceName"></param>
		public MapFieldAttribute(string sourceName)
		{
			_sourceName = sourceName;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourceName"></param>
		/// <param name="targetName"></param>
		public MapFieldAttribute(string sourceName, string targetName)
		{
			_sourceName = sourceName;
			_targetName = targetName;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourceName"></param>
		/// <param name="targetName"></param>
		/// <param name="isNullable"></param>
		public MapFieldAttribute(string sourceName, string targetName, bool isNullable)
		{
			_sourceName = sourceName;
			_targetName = targetName;
			_isNullable = isNullable;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="isNullable"></param>
		public MapFieldAttribute(bool isNullable)
		{
			_isNullable = isNullable;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourceName"></param>
		/// <param name="isNullable"></param>
		public MapFieldAttribute(string sourceName, bool isNullable)
		{
			_sourceName = sourceName;
			_isNullable = isNullable;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourceName"></param>
		/// <param name="isNullable"></param>
		/// <param name="isTrimmable"></param>
		public MapFieldAttribute(string sourceName, bool isNullable, bool isTrimmable)
		{
			_sourceName  = sourceName;
			_isNullable  = isNullable;
			_isTrimmable = isTrimmable;
		}

		private string _sourceName;
		/// <summary>
		/// 
		/// </summary>
		public  string SourceName
		{
			get { return _sourceName;  }
			set { _sourceName = value; }
		}

		private string _targetName;
		/// <summary>
		/// 
		/// </summary>
		public  string TargetName
		{
			get { return _targetName;  }
			set { _targetName = value; }
		}

		private bool _isNullable;
		/// <summary>
		/// 
		/// </summary>
		public  bool IsNullable
		{
			get { return _isNullable;  }
			set { _isNullable = value; }
		}

		private bool _isTrimmable = true;
		/// <summary>
		/// 
		/// </summary>
		/// <include file="Examples.xml" path='examples/mapfield[@name="IsTrimmable"]/*' />
		public  bool IsTrimmable
		{
			get { return _isTrimmable;  }
			set { _isTrimmable = value; }
		}
	}
}
