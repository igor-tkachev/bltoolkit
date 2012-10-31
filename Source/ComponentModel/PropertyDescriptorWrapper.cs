using System;
using System.ComponentModel;

namespace BLToolkit.ComponentModel
{
	[System.Diagnostics.DebuggerStepThrough]
	public abstract class PropertyDescriptorWrapper : PropertyDescriptor
	{
		protected PropertyDescriptorWrapper(PropertyDescriptor propertyDescriptor)
			: base(propertyDescriptor)
		{
			if (propertyDescriptor == null) throw new ArgumentNullException("propertyDescriptor");

			_pd = propertyDescriptor;
		}

		private readonly PropertyDescriptor _pd;

		public override AttributeCollection Attributes      { get { return _pd.Attributes;     } }
		public override string              Category        { get { return _pd.Category;       } }
		public override Type                ComponentType   { get { return _pd.ComponentType;  } }
		public override TypeConverter       Converter       { get { return _pd.Converter;      } }
		public override string              Description     { get { return _pd.Description;    } }
		public override bool                DesignTimeOnly  { get { return _pd.DesignTimeOnly; } }
		public override string              DisplayName     { get { return _pd.DisplayName;    } }
		public override bool                IsBrowsable     { get { return _pd.IsBrowsable;    } }
		public override bool                IsLocalizable   { get { return _pd.IsLocalizable;  } }
		public override bool                IsReadOnly      { get { return _pd.IsReadOnly;     } }
		public override string              Name            { get { return _pd.Name;           } }
		public override Type                PropertyType    { get { return _pd.PropertyType;   } }

		public override bool   Equals       (object obj)          { return _pd.Equals(obj);                }
		public override object GetEditor    (Type editorBaseType) { return _pd.GetEditor(editorBaseType);  }
		public override int    GetHashCode  ()                    { return _pd.GetHashCode();              }
		public override object GetValue     (object component)    { return _pd.GetValue(component);        }
		public override string ToString     ()                    { return _pd.ToString();                 }
		public override bool   CanResetValue(object component)    { return _pd.CanResetValue(component);   }

		public override void ResetValue(object component)                { _pd.ResetValue(component);      }
		public override void SetValue  (object component, object value)  { _pd.SetValue(component, value); }

		public override void AddValueChanged(object component, EventHandler handler)
		{
			_pd.AddValueChanged(component, handler);
		}

		public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
		{
			return _pd.GetChildProperties(instance, filter);
		}

		public override bool ShouldSerializeValue(object component)
		{
			return _pd.ShouldSerializeValue(component);
		}

		public override void RemoveValueChanged(object component, EventHandler handler)
		{
			_pd.RemoveValueChanged(component, handler);
		}
	}
}
