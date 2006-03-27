using System;

using BLToolkit.Demo.ObjectModel;

namespace BLToolkit.Demo.Forms
{
	public interface IBizEntityForm<T>
		where T : BizEntity
	{
		void SetBizEntity(T t);
	}
}
