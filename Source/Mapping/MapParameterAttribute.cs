/*
 * File:    MapParameterAttribute.cs
 * Created: 12/03/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	/// <include file="Examples.xml" path='examples/mapparam[@name="ctor(object)"]/*' />
	[AttributeUsage(AttributeTargets.Property)]
	public class MapParameterAttribute : Attribute
	{
		/// <summary>
		/// 
		/// </summary>
		/// <include file="Examples.xml" path='examples/mapparam[@name="ctor(object)"]/*' />
		protected MapParameterAttribute()
		{
			SetParameters();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameter1"></param>
		/// <include file="Examples.xml" path='examples/mapparam[@name="ctor(object)"]/*' />
		public MapParameterAttribute(object parameter1)
		{
			SetParameters(parameter1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameter1"></param>
		/// <param name="parameter2"></param>
		/// <include file="Examples.xml" path='examples/mapparam[@name="ctor(object)"]/*' />
		public MapParameterAttribute(
			object parameter1,
			object parameter2)
		{
			SetParameters(parameter1, parameter2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameter1"></param>
		/// <param name="parameter2"></param>
		/// <param name="parameter3"></param>
		/// <include file="Examples.xml" path='examples/mapparam[@name="ctor(object)"]/*' />
		public MapParameterAttribute(
			object parameter1,
			object parameter2,
			object parameter3)
		{
			SetParameters(parameter1, parameter2, parameter3);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameter1"></param>
		/// <param name="parameter2"></param>
		/// <param name="parameter3"></param>
		/// <param name="parameter4"></param>
		/// <include file="Examples.xml" path='examples/mapparam[@name="ctor(object)"]/*' />
		public MapParameterAttribute(
			object parameter1,
			object parameter2,
			object parameter3,
			object parameter4)
		{
			SetParameters(parameter1, parameter2, parameter3, parameter4);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameter1"></param>
		/// <param name="parameter2"></param>
		/// <param name="parameter3"></param>
		/// <param name="parameter4"></param>
		/// <param name="parameter5"></param>
		/// <include file="Examples.xml" path='examples/mapparam[@name="ctor(object)"]/*' />
		public MapParameterAttribute(
			object parameter1,
			object parameter2,
			object parameter3,
			object parameter4,
			object parameter5)
		{
			SetParameters(parameter1, parameter2, parameter3, parameter4, parameter5);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="parameters"></param>
		protected void SetParameters(params object[] parameters)
		{
			_parameters = parameters;
		}

		private object[] _parameters;
		/// <summary>
		/// 
		/// </summary>
		public  object[]  Parameters
		{
			get { return _parameters;  }
		}
	}
}
