using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;

using BLToolkit.ComponentModel;
using System.Reflection;

namespace BLToolkit.Demo.Controls
{
	public partial class EnumSelector : GroupBox
	{
		public EnumSelector()
		{
			InitializeComponent();
		}

		public EnumSelector(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

		[RefreshProperties(RefreshProperties.Repaint)]
		[Category("Data")]
		[Bindable(true)]
		public  object  Value
		{
			get
			{
				foreach (RadioButton r in _controls.Keys)
					if (r.Checked)
						return Enum.Parse(_valueType, (string)_controls[r]);


				return -1;
			}
			set
			{
				if (value != null)
				{
					string s = value.ToString();

					foreach (DictionaryEntry de in _controls)
					{
						RadioButton r = (RadioButton)de.Key;

						if (r.Checked != (de.Value.ToString() == s))
							r.Checked = !r.Checked;
					}
				}
			}
		}

		const FieldAttributes EnumField = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal;

		private Hashtable _controls = new Hashtable();

		private Type _valueType;
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(null)]
		[Category("Data")]
		[TypeConverter(typeof(TypeTypeConverter))]
		[Editor(typeof(EnumEditor), typeof(UITypeEditor))]
		public  Type  ValueType
		{
			get { return _valueType; }
			set
			{
				_valueType = value;

				foreach (Control c in _controls.Keys)
					Controls.Remove(c);

				FieldInfo[] fields = _valueType.GetFields();

				foreach (FieldInfo fi in fields)
				{
					if ((fi.Attributes & EnumField) == EnumField)
					{
						RadioButton r = new RadioButton();

						Controls.Add(r);
						_controls.Add(r, fi.Name);

						r.Text     = fi.Name;
						r.Name     = fi.Name + "RadioButton";
						r.TabIndex = _controls.Count;
						r.AutoSize = true;
						r.Location = new System.Drawing.Point(10, 23 * _controls.Count - 4);
						r.UseVisualStyleBackColor = true;
					}
				}
			}
		}

		class EnumEditor : ComponentModel.Design.TypeEditor
		{
			protected override bool FilterTypeList(Type type)
			{
				return type.IsPublic && type.IsEnum;
			}
		}
	}
}
