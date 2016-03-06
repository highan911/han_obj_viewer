namespace Han_Obj_Viewer
{
    partial class SharpGLForm
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
            this.openGLControl = new SharpGL.OpenGLControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPointLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.currentMeshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.meshCompareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointNeighborToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.faceNeighborToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.faceNormalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openGLControl
            // 
            this.openGLControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openGLControl.DrawFPS = true;
            this.openGLControl.Location = new System.Drawing.Point(0, 28);
            this.openGLControl.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.openGLControl.Name = "openGLControl";
            this.openGLControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openGLControl.RenderContextType = SharpGL.RenderContextType.FBO;
            this.openGLControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControl.Size = new System.Drawing.Size(832, 423);
            this.openGLControl.TabIndex = 0;
            this.openGLControl.OpenGLInitialized += new System.EventHandler(this.openGLControl_OpenGLInitialized);
            this.openGLControl.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl_OpenGLDraw);
            this.openGLControl.Resized += new System.EventHandler(this.openGLControl_Resized);
            this.openGLControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseDown);
            this.openGLControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseMove);
            this.openGLControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseUp);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(832, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.loadLabelToolStripMenuItem,
            this.loadPointLabelToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.loadToolStripMenuItem.Text = "Load Mesh";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // loadLabelToolStripMenuItem
            // 
            this.loadLabelToolStripMenuItem.Name = "loadLabelToolStripMenuItem";
            this.loadLabelToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.loadLabelToolStripMenuItem.Text = "Load Face Label";
            this.loadLabelToolStripMenuItem.Click += new System.EventHandler(this.loadFaceLabelToolStripMenuItem_Click);
            // 
            // loadPointLabelToolStripMenuItem
            // 
            this.loadPointLabelToolStripMenuItem.Name = "loadPointLabelToolStripMenuItem";
            this.loadPointLabelToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.loadPointLabelToolStripMenuItem.Text = "Load Point Label";
            this.loadPointLabelToolStripMenuItem.Click += new System.EventHandler(this.loadPointLabelToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.currentMeshToolStripMenuItem,
            this.meshCompareToolStripMenuItem,
            this.pointNeighborToolStripMenuItem,
            this.faceNeighborToolStripMenuItem,
            this.faceNormalToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.editToolStripMenuItem.Text = "Display";
            // 
            // currentMeshToolStripMenuItem
            // 
            this.currentMeshToolStripMenuItem.Name = "currentMeshToolStripMenuItem";
            this.currentMeshToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
            this.currentMeshToolStripMenuItem.Text = "Current Mesh";
            this.currentMeshToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.currentMeshToolStripMenuItem_DropDownItemClicked);
            // 
            // meshCompareToolStripMenuItem
            // 
            this.meshCompareToolStripMenuItem.Name = "meshCompareToolStripMenuItem";
            this.meshCompareToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
            this.meshCompareToolStripMenuItem.Text = "Mesh Compare";
            // 
            // pointNeighborToolStripMenuItem
            // 
            this.pointNeighborToolStripMenuItem.Name = "pointNeighborToolStripMenuItem";
            this.pointNeighborToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
            this.pointNeighborToolStripMenuItem.Text = "Point Neighbor";
            this.pointNeighborToolStripMenuItem.Click += new System.EventHandler(this.pointNeighborToolStripMenuItem_Click);
            // 
            // faceNeighborToolStripMenuItem
            // 
            this.faceNeighborToolStripMenuItem.Name = "faceNeighborToolStripMenuItem";
            this.faceNeighborToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
            this.faceNeighborToolStripMenuItem.Text = "Face Neighbor";
            this.faceNeighborToolStripMenuItem.Click += new System.EventHandler(this.faceNeighborToolStripMenuItem_Click);
            // 
            // faceNormalToolStripMenuItem
            // 
            this.faceNormalToolStripMenuItem.Name = "faceNormalToolStripMenuItem";
            this.faceNormalToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
            this.faceNormalToolStripMenuItem.Text = "Face Normal";
            this.faceNormalToolStripMenuItem.Click += new System.EventHandler(this.faceNormalToolStripMenuItem_Click);
            // 
            // SharpGLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 451);
            this.Controls.Add(this.openGLControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "SharpGLForm";
            this.Text = "Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SharpGL.OpenGLControl openGLControl;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadLabelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem currentMeshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem meshCompareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pointNeighborToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem faceNeighborToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPointLabelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem faceNormalToolStripMenuItem;
    }
}

