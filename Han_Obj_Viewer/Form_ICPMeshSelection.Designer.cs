﻿namespace Han_Obj_Viewer
{
    partial class Form_ICPMeshSelection
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
            this.comboBoxTarget = new System.Windows.Forms.ComboBox();
            this.comboBoxSource = new System.Windows.Forms.ComboBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxTarget
            // 
            this.comboBoxTarget.FormattingEnabled = true;
            this.comboBoxTarget.Location = new System.Drawing.Point(104, 54);
            this.comboBoxTarget.Name = "comboBoxTarget";
            this.comboBoxTarget.Size = new System.Drawing.Size(166, 23);
            this.comboBoxTarget.TabIndex = 0;
            this.comboBoxTarget.SelectedIndexChanged += new System.EventHandler(this.comboBoxTarget_SelectedIndexChanged);
            // 
            // comboBoxSource
            // 
            this.comboBoxSource.FormattingEnabled = true;
            this.comboBoxSource.Location = new System.Drawing.Point(104, 12);
            this.comboBoxSource.Name = "comboBoxSource";
            this.comboBoxSource.Size = new System.Drawing.Size(166, 23);
            this.comboBoxSource.TabIndex = 1;
            this.comboBoxSource.SelectedIndexChanged += new System.EventHandler(this.comboBoxSource_SelectedIndexChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(195, 97);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Source";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Target";
            // 
            // Form_ICPMeshSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 137);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.comboBoxSource);
            this.Controls.Add(this.comboBoxTarget);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_ICPMeshSelection";
            this.Text = "Mesh Selection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxTarget;
        private System.Windows.Forms.ComboBox comboBoxSource;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}