namespace BLToolkit.Demo.Forms
{
	partial class MainForm
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
			this.personGridView = new System.Windows.Forms.DataGridView();
			this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.fullNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.genderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.personBinder = new BLToolkit.ComponentModel.ObjectBinder(this.components);
			((System.ComponentModel.ISupportInitialize)(this.personGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// personGridView
			// 
			this.personGridView.AllowUserToAddRows = false;
			this.personGridView.AllowUserToDeleteRows = false;
			this.personGridView.AllowUserToOrderColumns = true;
			this.personGridView.AutoGenerateColumns = false;
			this.personGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.personGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.personGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.fullNameDataGridViewTextBoxColumn,
            this.genderDataGridViewTextBoxColumn});
			this.personGridView.DataSource = this.personBinder;
			this.personGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.personGridView.Location = new System.Drawing.Point(0, 0);
			this.personGridView.Name = "personGridView";
			this.personGridView.ReadOnly = true;
			this.personGridView.Size = new System.Drawing.Size(646, 347);
			this.personGridView.TabIndex = 0;
			this.personGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.personGridView_CellDoubleClick);
			// 
			// iDDataGridViewTextBoxColumn
			// 
			this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
			this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
			this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
			this.iDDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// fullNameDataGridViewTextBoxColumn
			// 
			this.fullNameDataGridViewTextBoxColumn.DataPropertyName = "FullName";
			this.fullNameDataGridViewTextBoxColumn.HeaderText = "Name";
			this.fullNameDataGridViewTextBoxColumn.Name = "fullNameDataGridViewTextBoxColumn";
			this.fullNameDataGridViewTextBoxColumn.ReadOnly = true;
			this.fullNameDataGridViewTextBoxColumn.Width = 150;
			// 
			// genderDataGridViewTextBoxColumn
			// 
			this.genderDataGridViewTextBoxColumn.DataPropertyName = "Gender";
			this.genderDataGridViewTextBoxColumn.HeaderText = "Gender";
			this.genderDataGridViewTextBoxColumn.Name = "genderDataGridViewTextBoxColumn";
			this.genderDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// personBinder
			// 
			this.personBinder.IsNull = null;
			this.personBinder.ItemType = typeof(BLToolkit.Demo.ObjectModel.Person);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(646, 347);
			this.Controls.Add(this.personGridView);
			this.Name = "MainForm";
			this.Text = "Business Logic Toolkit Demo";
			((System.ComponentModel.ISupportInitialize)(this.personGridView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private BLToolkit.ComponentModel.ObjectBinder personBinder;
		private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn fullNameDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn genderDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridView personGridView;
	}
}