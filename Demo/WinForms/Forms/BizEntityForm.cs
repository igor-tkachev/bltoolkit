using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

using BLToolkit.Validation;
using BLToolkit.Reflection;

using BLToolkit.Demo.ObjectModel;

namespace BLToolkit.Demo.Forms
{
	public class BizEntityForm<F,T> : Form
		where F : BizEntityForm<F,T>, new()
		where T : BizEntity
	{
		#region Static Members

		public static bool Edit(T entity, Action<T> saveAction)
		{
			T clone = (T)entity.Clone();
			F form  = new F();

			form.SetBizEntity(clone);
			form.Init(clone, delegate
			{
				saveAction(clone);

				clone.CopyTo(entity);
				entity.AcceptChanges();
			});

			return form.ShowDialog() == DialogResult.OK;
		}

		public static T EditNew(Action<T> saveAction)
		{
			T entity = TypeAccessor<T>.CreateInstanceEx();
			F form   = new F();

			form.SetBizEntity(entity);
			form.Init(entity, delegate
			{
				saveAction(entity);
				entity.AcceptChanges();
			});

			return form.ShowDialog() == DialogResult.OK? entity: null;
		}

		private void Init(BizEntity entity, SaveHandler saveHandler)
		{
			if (AcceptButton is Button)
				((Button)AcceptButton).Click += SaveEntity;

			_entity      = entity;
			_saveHandler = saveHandler;
		}

		#endregion

		#region Abstracts

		protected virtual void SetBizEntity(T entity)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region SaveEntity Handler

		private delegate void SaveHandler();

		private SaveHandler _saveHandler;
		private BizEntity   _entity;

		protected void SaveEntity(object sender, EventArgs e)
		{
			if (_entity.IsDirty)
			{
				try
				{
					_entity.Validate();

					UseWaitCursor = true;
					_saveHandler();
					UseWaitCursor = false;

					DialogResult = DialogResult.OK;
					Close();
				}
				catch (Exception ex)
				{
					UseWaitCursor = false;
					MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					DialogResult = DialogResult.None;
				}
			}
			else
			{
				DialogResult = DialogResult.Cancel;
				Close();
			}
		}

		#endregion

		#region Scan Controls

		private static void ForEach(Control control, Hashtable scanedControls, Predicate<Control> controlHandler)
		{
			if (control != null && !scanedControls.ContainsKey(control))
			{
				scanedControls.Add(control, control);

				if (controlHandler(control))
					foreach (Control c in control.Controls)
						ForEach(c, scanedControls, controlHandler);
			}
		}

		protected virtual void ScanControls(Predicate<Control> controlHandler)
		{
			ForEach(this, new Hashtable(), controlHandler);
		}

		protected virtual void ScanControls(Control control, Predicate<Control> controlHandler)
		{
			ForEach(control, new Hashtable(), controlHandler);
		}

		#endregion

		#region OnLoad

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			_toolTip.ToolTipIcon  = ToolTipIcon.Warning;
			_toolTip.ToolTipTitle = "Validation";

			try
			{
				_toolTip.IsBalloon = (int)Registry.GetValue(
					@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
					"EnableBalloonTips", 1) != 0;
			}
			catch
			{
			}

			if (!DesignMode)
			{
				ScanControls(InitBindableControls);
				ValidateForm();
			}
		}

		#endregion

		#region Binding

		class ControlInfo
		{
			public ControlInfo(Control control, bool isValidatable, PropertyDescriptor pd)
			{
				Control            = control;
				IsValidatable      = isValidatable;
				PropertyDescriptor = pd;
			}

			public Control            Control;
			public bool               IsValidatable;
			public PropertyDescriptor PropertyDescriptor;
		}

		ToolTip _toolTip = new ToolTip();

		Dictionary<string, ControlInfo> _keyToControl     = new Dictionary<string,ControlInfo>();
		List<ControlInfo>               _bindableControls = new List<ControlInfo>();

		private bool InitBindableControls(Control control)
		{
			foreach (Binding binding in control.DataBindings)
			{
				BizEntity item = binding.BindingManagerBase.Current as BizEntity;

				if (item != null)
				{
					string key = GetBindingKey(item, binding, control);

					if (_keyToControl.ContainsKey(key))
						continue;

					string[]           str = null;
					PropertyDescriptor pd  = null;

					ITypedList typedList = binding.DataSource as ITypedList;

					if (typedList != null)
					{
						pd = typedList.GetItemProperties(null).Find(
							binding.BindingMemberInfo.BindingField, false);

						if (pd != null)
							str = Validator.GetErrorMessages(item, pd);
					}

					if (str == null)
						str = item.GetErrorMessages(binding.BindingMemberInfo.BindingField);

					if (str.Length > 0)
						Array.Sort(str);

					ControlInfo ci = new ControlInfo(control, str.Length > 0, pd);

					_bindableControls.Add(ci);
					_keyToControl.Add(key, ci);

					if (ci.IsValidatable)
						_toolTip.SetToolTip(control, string.Join("\r\n", str));

					control.LostFocus      += ValidateControl;
					control.Validated      += ValidateControl;
					control.EnabledChanged += ValidateControl;
				}
			}

			return true;
		}

		private void ValidateControl(object sender, EventArgs e)
		{
			Validate((Control)sender, true);
		}

		private static string GetBindingKey(BizEntity entity, Binding binding, Control control)
		{
			return string.Format("{0}.{1}.{2}",
				entity.GetHashCode(), binding.BindingMemberInfo.BindingField, control.Name);
		}

		protected bool Validate(Control control, bool validateCombines)
		{
			bool result = true;

			foreach (Binding binding in control.DataBindings)
			{
				if (binding.BindingManagerBase == null || binding.BindingManagerBase.Count == 0)
					continue;

				BizEntity item = binding.BindingManagerBase.Current as BizEntity;

				if (item != null)
				{
					string key = GetBindingKey(item, binding, control);

					if (!_keyToControl.ContainsKey(key))
						continue;

					ControlInfo ci        = _keyToControl[key];
					string      fieldName = binding.BindingMemberInfo.BindingField;

					bool isValid = ci.IsValidatable?
						ci.PropertyDescriptor != null?
							Validator.IsValid(item, ci.PropertyDescriptor):
							item.IsValid(fieldName):
						true;

					if (isValid)
					{
						if (item.IsDirtyMember(fieldName))
							SetDirty(control);
						else
							ResetControl(control);
					}
					else
					{
						SetInvalid(control);

						result = false;
					}

					/*
					if (validateCombines)
					{
						PropertyInfo pi = 
							item.GetType().GetProperty(binding.BindingMemberInfo.BindingField);

						if (pi != null)
						{
							object[] attrs = pi.GetCustomAttributes(typeof(CombineAttribute), true);

							foreach (CombineAttribute a in attrs)
							{
								string key = GetBindingKey(item, binding, control);
								string key = item.GetHashCode() + "." + a.Name;

								ControlInfo ci = (ControlInfo)nameToControl[key];

								if (ci != null)
									result = Validate(ci.Control, false) && result;
							}
						}
					}
					 */
				}
			}

			return result;
		}

		Dictionary<Control, Control> _modifiedControls = new Dictionary<Control,Control>();
		Dictionary<Control, Color>   _originalColors   = new Dictionary<Control,Color>();

		protected virtual void SetInvalid(Control control)
		{
			if (control.Enabled == false)
				return;

			if (_modifiedControls.ContainsKey(control) == false)
				_modifiedControls.Add(control, control);

			if (_originalColors.ContainsKey(control) == false)
				_originalColors.Add(control, control.BackColor);

			Color color = Modify((Color)_originalColors[control], 45, 0, 0);

			if (color != control.BackColor)
				control.BackColor = color;
		}

		protected virtual void SetDirty(Control control)
		{
			if (control.Enabled == false)
				return;

			if (_modifiedControls.ContainsKey(control) == false)
				_modifiedControls.Add(control, control);

			if (_originalColors.ContainsKey(control) == false)
				_originalColors.Add(control, control.BackColor);

			Color color = Modify((Color)_originalColors[control], 50,  50, -15);

			if (color != control.BackColor)
				control.BackColor = color;
		}

		protected virtual void ResetControl(Control control)
		{
			if (_modifiedControls.ContainsKey(control))
			{
				_modifiedControls.Remove(control);

				if (_originalColors.ContainsKey(control))
				{
					control.BackColor = control.Enabled?
						(Color)_originalColors[control]:
						Color.FromKnownColor(KnownColor.Control);
				}
			}
		}

		public virtual bool ValidateForm()
		{
			bool isValid = true;

			foreach (ControlInfo ci in _bindableControls)
				isValid = Validate(ci.Control, false) && isValid;

			return isValid;
		}

		public static Color Modify(Color original, int dr, int dg, int db)
		{
			int r = original.R + dr;
			int g = original.G + dg;
			int b = original.B + db;

			if (r > 255 || g > 255 || b > 255)
			{
				int d = Math.Max(r, Math.Max(g, b)) - 255;

				r -= d;
				g -= d;
				b -= d;
			}

			if (r < 0) r = 0;
			if (g < 0) g = 0;
			if (b < 0) b = 0;

			return Color.FromArgb(r, g, b);
		}

		#endregion
	}
}
