using System;
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
			_typeAccessor = TypeAccessor.GetAccessor(itemType);
		}

		private readonly Type         _itemType;
		private readonly TypeAccessor _typeAccessor;

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
					Type itemType = TypeHelper.GetListItemType(
						listAccessors[listAccessors.Length - 1].PropertyType);

					if (itemType == typeof(object))
					{
						TypeAccessor parentAccessor = _typeAccessor;

						foreach (PropertyDescriptor pd in listAccessors)
						{
							// We have to create an instance of the list to determine its item type
							//

							// Create an instance of the parent.
							//
							object parentObject = parentAccessor.CreateInstanceEx();

							// Create an instance of the list.
							//
							object listObject   = pd.GetValue(parentObject);

							if (listObject == null)
							{
								// We failed. Item type can not be determined.
								//
								itemType = null;

								break;
							}

							itemType = TypeHelper.GetListItemType(listObject);

							// Still bad.
							//
							if (itemType == typeof(object))
								break;

							parentAccessor = TypeAccessor.GetAccessor(itemType);
						}
					}

					if (itemType != null && itemType != typeof(object))
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
