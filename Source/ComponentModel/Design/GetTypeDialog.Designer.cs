namespace BLToolkit.ComponentModel.Design
{
	partial class GetTypeDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetTypeDialog));
			System.Windows.Forms.Label labelRebuild;
			this._treeView = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this._systemCheckBox = new System.Windows.Forms.CheckBox();
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			labelRebuild = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// _treeView
			// 
			this._treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._treeView.ImageIndex = 0;
			this._treeView.ImageList = this.imageList;
			this._treeView.Location = new System.Drawing.Point(14, 48);
			this._treeView.Name = "_treeView";
			this._treeView.SelectedImageIndex = 0;
			this._treeView.Size = new System.Drawing.Size(523, 345);
			this._treeView.TabIndex = 0;
			this._treeView.DoubleClick += new System.EventHandler(this.treeView_DoubleClick);
			this._treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Magenta;
			this.imageList.Images.SetKeyName(0, "None.bmp");
			this.imageList.Images.SetKeyName(1, "Assembly.bmp");
			this.imageList.Images.SetKeyName(2, "Namespace.bmp");
			this.imageList.Images.SetKeyName(3, "Object.bmp");
			// 
			// _systemCheckBox
			// 
			this._systemCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._systemCheckBox.AutoSize = true;
			this._systemCheckBox.Checked = true;
			this._systemCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this._systemCheckBox.Location = new System.Drawing.Point(14, 409);
			this._systemCheckBox.Name = "_systemCheckBox";
			this._systemCheckBox.Size = new System.Drawing.Size(330, 20);
			this._systemCheckBox.TabIndex = 1;
			this._systemCheckBox.Text = "Hide assemblies that begin with Microsoft or System.";
			this._systemCheckBox.UseVisualStyleBackColor = true;
			this._systemCheckBox.CheckedChanged += new System.EventHandler(this.systemCheckBox_CheckedChanged);
			// 
			// labelRebuild
			// 
			labelRebuild.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			labelRebuild.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			labelRebuild.Location = new System.Drawing.Point(11, 9);
			labelRebuild.Name = "labelRebuild";
			labelRebuild.Size = new System.Drawing.Size(525, 32);
			labelRebuild.TabIndex = 5;
			labelRebuild.Text = "If your object type does not appear, close the dialog and rebuild the project tha" +
				"t contains your object.\r\n\r\n";
			// 
			// _okButton
			// 
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Location = new System.Drawing.Point(350, 404);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(87, 29);
			this._okButton.TabIndex = 3;
			this._okButton.Text = "OK";
			this._okButton.UseVisualStyleBackColor = true;
			// 
			// _cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new System.Drawing.Point(450, 404);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(87, 29);
			this._cancelButton.TabIndex = 4;
			this._cancelButton.Text = "Cancel";
			this._cancelButton.UseVisualStyleBackColor = true;
			// 
			// GetTypeDialog
			// 
			this.AcceptButton = this._okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(547, 442);
			this.Controls.Add(labelRebuild);
			this.Controls.Add(this._systemCheckBox);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._okButton);
			this.Controls.Add(this._treeView);
			this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(555, 275);
			this.Name = "GetTypeDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select the Type";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.TreeView _treeView;
		private System.Windows.Forms.CheckBox _systemCheckBox;
	}
}