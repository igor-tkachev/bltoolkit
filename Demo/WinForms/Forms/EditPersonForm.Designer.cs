namespace BLToolkit.Demo.Forms
{
	partial class EditPersonForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.firstNameTextBox = new System.Windows.Forms.TextBox();
			this.personBinder = new BLToolkit.ComponentModel.ObjectBinder(this.components);
			this.firstNameLabel = new System.Windows.Forms.Label();
			this.middleNameLabel = new System.Windows.Forms.Label();
			this.middleNameTextBox = new System.Windows.Forms.TextBox();
			this.lastNameLabel = new System.Windows.Forms.Label();
			this.lastNameTextBox = new System.Windows.Forms.TextBox();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.genderSelector = new BLToolkit.Demo.Controls.EnumSelector(this.components);
			this.SuspendLayout();
			// 
			// firstNameTextBox
			// 
			this.firstNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.personBinder, "FirstName", true));
			this.firstNameTextBox.Location = new System.Drawing.Point(90, 12);
			this.firstNameTextBox.Name = "firstNameTextBox";
			this.firstNameTextBox.Size = new System.Drawing.Size(160, 20);
			this.firstNameTextBox.TabIndex = 2;
			// 
			// personBinder
			// 
			this.personBinder.IsNull = null;
			this.personBinder.ItemType = typeof(BLToolkit.Demo.ObjectModel.Person);
			this.personBinder.ObjectViewType = typeof(BLToolkit.Demo.Forms.ObjectViews.PersonView);
			// 
			// firstNameLabel
			// 
			this.firstNameLabel.AutoSize = true;
			this.firstNameLabel.Location = new System.Drawing.Point(12, 15);
			this.firstNameLabel.Name = "firstNameLabel";
			this.firstNameLabel.Size = new System.Drawing.Size(60, 13);
			this.firstNameLabel.TabIndex = 1;
			this.firstNameLabel.Text = "First Name:";
			// 
			// middleNameLabel
			// 
			this.middleNameLabel.AutoSize = true;
			this.middleNameLabel.Location = new System.Drawing.Point(12, 47);
			this.middleNameLabel.Name = "middleNameLabel";
			this.middleNameLabel.Size = new System.Drawing.Size(72, 13);
			this.middleNameLabel.TabIndex = 3;
			this.middleNameLabel.Text = "Middle Name:";
			// 
			// middleNameTextBox
			// 
			this.middleNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.personBinder, "MiddleName", true));
			this.middleNameTextBox.Location = new System.Drawing.Point(90, 44);
			this.middleNameTextBox.Name = "middleNameTextBox";
			this.middleNameTextBox.Size = new System.Drawing.Size(160, 20);
			this.middleNameTextBox.TabIndex = 4;
			// 
			// lastNameLabel
			// 
			this.lastNameLabel.AutoSize = true;
			this.lastNameLabel.Location = new System.Drawing.Point(12, 80);
			this.lastNameLabel.Name = "lastNameLabel";
			this.lastNameLabel.Size = new System.Drawing.Size(61, 13);
			this.lastNameLabel.TabIndex = 5;
			this.lastNameLabel.Text = "Last Name:";
			// 
			// lastNameTextBox
			// 
			this.lastNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.personBinder, "LastName", true));
			this.lastNameTextBox.Location = new System.Drawing.Point(90, 77);
			this.lastNameTextBox.Name = "lastNameTextBox";
			this.lastNameTextBox.Size = new System.Drawing.Size(160, 20);
			this.lastNameTextBox.TabIndex = 6;
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(33, 235);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 100;
			this.okButton.Text = "Save";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(157, 235);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 101;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// genderSelector
			// 
			this.genderSelector.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.personBinder, "Gender", true));
			this.genderSelector.Location = new System.Drawing.Point(90, 103);
			this.genderSelector.Name = "genderSelector";
			this.genderSelector.Size = new System.Drawing.Size(102, 113);
			this.genderSelector.TabIndex = 7;
			this.genderSelector.TabStop = false;
			this.genderSelector.Text = "Gender";
			this.genderSelector.Value = -1;
			this.genderSelector.ValueType = typeof(BLToolkit.Demo.ObjectModel.Gender);
			// 
			// EditPersonForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(269, 268);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.genderSelector);
			this.Controls.Add(this.lastNameLabel);
			this.Controls.Add(this.middleNameLabel);
			this.Controls.Add(this.firstNameLabel);
			this.Controls.Add(this.lastNameTextBox);
			this.Controls.Add(this.middleNameTextBox);
			this.Controls.Add(this.firstNameTextBox);
			this.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.personBinder, "FormTitle", true));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EditPersonForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Person";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private BLToolkit.ComponentModel.ObjectBinder personBinder;
		private System.Windows.Forms.TextBox firstNameTextBox;
		private System.Windows.Forms.Label firstNameLabel;
		private System.Windows.Forms.Label middleNameLabel;
		private System.Windows.Forms.TextBox middleNameTextBox;
		private System.Windows.Forms.Label lastNameLabel;
		private System.Windows.Forms.TextBox lastNameTextBox;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private BLToolkit.Demo.Controls.EnumSelector genderSelector;
	}
}