/*
 * File:    ResultInfo.cs
 * Created: 08/16/2005
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;
using System.Collections;

namespace Rsdn.Framework.Data.Mapping
{
	class ResultInfo
	{
		public MapResultSet ResultSet;
		public IList        List;
		public string       IndexID;
		public Hashtable    Hashtable;
	}
}
