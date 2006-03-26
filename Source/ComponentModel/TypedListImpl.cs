using System;
using System.Collections;
using System.ComponentModel;

using BLToolkit.Reflection;

namespace BLToolkit.ComponentModel
{
	public class TypedListImpl : ITypedList
	{
		public TypedListImpl(Type itemType)
		{
			if (itemType == null) throw new ArgumentNullException("itemType");

			_itemType     = itemType;

//			try
//			{
//				if (_itemType != typeof(object))
					_typeAccessor = TypeAccessor.GetAccessor(itemType);
//			}
//			catch
//			{
//			}
		}

		private Type         _itemType;
		private TypeAccessor _typeAccessor;

		private NullValueProvider _getNullValue;
		public  NullValueProvider  GetNullValue
		{
			get
			{
				if (_getNullValue == null)
					_getNullValue = TypeAccessor.GetNullValue;

				return _getNullValue;
			}

			set { _getNullValue = value; }
		}

		#region ITypedList Members

		private PropertyDescriptorCollection _pdc;

		public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			return GetItemProperties(listAccessors, null, null, true);
		}

		public PropertyDescriptorCollection GetItemProperties(
			PropertyDescriptor[] listAccessors,
			Type                 objectViewType,
			IsNullHandler        isNull,
			bool                 cache)
		{
			PropertyDescriptorCollection pdc = null;

			if (listAccessors == null || listAccessors.Length == 0)
			{
				if (_pdc == null)
				{
					_pdc = _typeAccessor != null?
						_typeAccessor.CreateExtendedPropertyDescriptors(objectViewType, isNull):
						new PropertyDescriptorCollection(null);
				}

				pdc = _pdc;

				if (!cache)
					_pdc = null;
			}
			else
			{
				try
				{
					// Lets try to pick out the item type from the list type.
					//
					Type itemType = TypeHelper.GetListItemType(listAccessors[0].PropertyType);

					if (itemType == typeof(object))
					{
						// We have to create an instance of the list to determine its item type.
						//
						object obj = _typeAccessor.CreateInstanceEx();
						object o   = listAccessors[0].GetValue(obj);

						if (o != null)
							itemType = TypeHelper.GetListItemType(o);
					}

					if (itemType != typeof(object))
					{
						TypeAccessor ta = TypeAccessor.GetAccessor(itemType);

						pdc = ta.CreateExtendedPropertyDescriptors(null, isNull);
					}
				}
				catch
				{
				}

				if (pdc == null)
					pdc = new PropertyDescriptorCollection(null);
			}

			return pdc;
		}

		public string GetListName(PropertyDescriptor[] listAccessors)
		{
			string name = _itemType.Name;

			if (listAccessors != null)
				foreach (PropertyDescriptor pd in listAccessors)
					name += "_" + pd.Name;

			return name;
		}

		#endregion
	}
}
