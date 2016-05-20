namespace Han_Obj_Viewer
{
    partial class Form_ICP
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
            this.labelMin = new System.Windows.Forms.Label();
            this.labelVal = new System.Windows.Forms.Label();
            this.labelTotalIter = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.Location = new System.Drawing.Point(12, 61);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(31, 15);
            this.labelMin.TabIndex = 5;
            this.labelMin.Text = "Min";
            // 
            // labelVal
            // 
            this.labelVal.AutoSize = true;
            this.labelVal.Location = new System.Drawing.Point(12, 35);
            this.labelVal.Name = "labelVal";
            this.labelVal.Size = new System.Drawing.Size(47, 15);
            this.labelVal.TabIndex = 6;
            this.labelVal.Text = "Value";
            // 
            // labelTotalIter
            // 
            this.labelTotalIter.AutoSize = true;
            this.labelTotalIter.Location = new System.Drawing.Point(12, 9);
            this.labelTotalIter.Name = "labelTotalIter";
            this.labelTotalIter.Size = new System.Drawing.Size(87, 15);
            this.labelTotalIter.TabIndex = 7;
            this.labelTotalIter.Text = "Total Iter";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(195, 99);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 8;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // Form_ICP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 147);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.labelTotalIter);
            this.Controls.Add(this.labelVal);
            this.Controls.Add(this.labelMin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_ICP";
            this.Text = "Form_SA";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_ICP_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelMin;
        private System.Windows.Forms.Label labelVal;
        private System.Windows.Forms.Label labelTotalIter;
        private System.Windows.Forms.Button buttonStart;
    }
}