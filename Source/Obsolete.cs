/*
 * File:    MapData.cs
 * Created: 5/1/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data
{
	/// <summary>
	/// The class is obsolete and will be removed with the next version.
	/// </summary>
	[Obsolete("Use Rsdn.Framework.Data.Mapping.Map class instead.")]
	public sealed class MapData : Rsdn.Framework.Data.Mapping.Map
	{
	}

	/// <summary>
	/// The class is obsolete and will be removed with the next version.
	/// </summary>
	[Obsolete("Use Rsdn.Framework.Data.Mapping.RsdnMapException class instead.")]
	[Serializable] 
	public class RsdnMapDataException : Rsdn.Framework.Data.Mapping.RsdnMapException
	{
	}

	/*
	/// <summary>
	/// 
	/// </summary>
	[Obsolete("Use Rsdn.Framework.Data namespace instead.")]
	public sealed class MapDefaultValueAttribute : Rsdn.Framework.Data.Mapping.MapDefaultValueAttribute
	{
	}

	/// <summary>
	/// 
	/// </summary>
	[Obsolete("Use Rsdn.Framework.Data namespace instead.")]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class MapFieldAttribute : Rsdn.Framework.Data.Mapping.MapFieldAttribute
	{
	}

	/// <summary>
	/// 
	/// </summary>
	[Obsolete("Use Rsdn.Framework.Data namespace instead.")]
	public sealed class MapNullValueAttribute : Rsdn.Framework.Data.Mapping.MapNullValueAttribute
	{
	}
	*/
}
