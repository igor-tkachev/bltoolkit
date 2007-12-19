namespace BLToolkit.ComponentModel.Design
{
	partial class TypePicker
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TypePicker));
			this.treeView = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.addNewLinkLabel = new NewLink();
			this.addNewPanel = new System.Windows.Forms.Panel();
			this.addNewSplitPanel = new System.Windows.Forms.Panel();
			this.addNewPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// treeView
			// 
			this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView.HotTracking = true;
			this.treeView.ImageIndex = 0;
			this.treeView.ImageList = this.imageList;
			this.treeView.Location = new System.Drawing.Point(0, 0);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = 0;
			this.treeView.Size = new System.Drawing.Size(251, 243);
			this.treeView.TabIndex = 0;
			this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
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
			// addNewLinkLabel
			// 
			this.addNewLinkLabel.AutoSize = true;
			this.addNewLinkLabel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.addNewLinkLabel.ImageList = this.imageList;
			this.addNewLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.addNewLinkLabel.Location = new System.Drawing.Point(0, 3);
			this.addNewLinkLabel.Name = "addNewLinkLabel";
			this.addNewLinkLabel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.addNewLinkLabel.Size = new System.Drawing.Size(103, 13);
			this.addNewLinkLabel.TabIndex = 1;
			this.addNewLinkLabel.TabStop = true;
			this.addNewLinkLabel.Text = "Add Project Type...";
			this.addNewLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.addNewLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.addNewLinkLabel_LinkClicked);
			// 
			// addNewPanel
			// 
			this.addNewPanel.BackColor = System.Drawing.SystemColors.Window;
			this.addNewPanel.Controls.Add(this.addNewLinkLabel);
			this.addNewPanel.Controls.Add(this.addNewSplitPanel);
			this.addNewPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.addNewPanel.Location = new System.Drawing.Point(0, 243);
			this.addNewPanel.Name = "addNewPanel";
			this.addNewPanel.Size = new System.Drawing.Size(251, 20);
			this.addNewPanel.TabIndex = 2;
			// 
			// addNewSplitPanel
			// 
			this.addNewSplitPanel.BackColor = System.Drawing.SystemColors.ControlDark;
			this.addNewSplitPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.addNewSplitPanel.Location = new System.Drawing.Point(0, 0);
			this.addNewSplitPanel.Name = "addNewSplitPanel";
			this.addNewSplitPanel.Size = new System.Drawing.Size(251, 1);
			this.addNewSplitPanel.TabIndex = 2;
			// 
			// TypePicker
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.treeView);
			this.Controls.Add(this.addNewPanel);
			this.Name = "TypePicker";
			this.Size = new System.Drawing.Size(251, 263);
			this.Resize += new System.EventHandler(this.TypePicker_Resize);
			this.addNewPanel.ResumeLayout(false);
			this.addNewPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView treeView;
		private NewLink addNewLinkLabel;
		private System.Windows.Forms.Panel addNewPanel;
		private System.Windows.Forms.Panel addNewSplitPanel;
		private System.Windows.Forms.ImageList imageList;
	}
}
