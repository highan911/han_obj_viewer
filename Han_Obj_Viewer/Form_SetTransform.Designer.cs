namespace Han_Obj_Viewer
{
    partial class Form_SetTransform
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericRx = new System.Windows.Forms.NumericUpDown();
            this.numericRy = new System.Windows.Forms.NumericUpDown();
            this.numericRz = new System.Windows.Forms.NumericUpDown();
            this.numericS = new System.Windows.Forms.NumericUpDown();
            this.numericX = new System.Windows.Forms.NumericUpDown();
            this.numericY = new System.Windows.Forms.NumericUpDown();
            this.numericZ = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericRx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericZ)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rotation X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Rotation Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Rotation Z";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Scale";
            // 
            // numericRx
            // 
            this.numericRx.DecimalPlaces = 1;
            this.numericRx.Location = new System.Drawing.Point(150, 7);
            this.numericRx.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericRx.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericRx.Name = "numericRx";
            this.numericRx.Size = new System.Drawing.Size(120, 25);
            this.numericRx.TabIndex = 4;
            // 
            // numericRy
            // 
            this.numericRy.DecimalPlaces = 1;
            this.numericRy.Location = new System.Drawing.Point(150, 38);
            this.numericRy.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericRy.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericRy.Name = "numericRy";
            this.numericRy.Size = new System.Drawing.Size(120, 25);
            this.numericRy.TabIndex = 5;
            // 
            // numericRz
            // 
            this.numericRz.DecimalPlaces = 1;
            this.numericRz.Location = new System.Drawing.Point(150, 69);
            this.numericRz.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericRz.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericRz.Name = "numericRz";
            this.numericRz.Size = new System.Drawing.Size(120, 25);
            this.numericRz.TabIndex = 6;
            // 
            // numericS
            // 
            this.numericS.DecimalPlaces = 1;
            this.numericS.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericS.Location = new System.Drawing.Point(150, 100);
            this.numericS.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericS.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.numericS.Name = "numericS";
            this.numericS.Size = new System.Drawing.Size(120, 25);
            this.numericS.TabIndex = 7;
            this.numericS.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericX
            // 
            this.numericX.DecimalPlaces = 1;
            this.numericX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericX.Location = new System.Drawing.Point(150, 131);
            this.numericX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericX.Name = "numericX";
            this.numericX.Size = new System.Drawing.Size(120, 25);
            this.numericX.TabIndex = 8;
            // 
            // numericY
            // 
            this.numericY.DecimalPlaces = 1;
            this.numericY.Location = new System.Drawing.Point(150, 162);
            this.numericY.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericY.Name = "numericY";
            this.numericY.Size = new System.Drawing.Size(120, 25);
            this.numericY.TabIndex = 9;
            // 
            // numericZ
            // 
            this.numericZ.DecimalPlaces = 1;
            this.numericZ.Location = new System.Drawing.Point(150, 193);
            this.numericZ.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericZ.Name = "numericZ";
            this.numericZ.Size = new System.Drawing.Size(120, 25);
            this.numericZ.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 164);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 195);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 15);
            this.label7.TabIndex = 13;
            this.label7.Text = "Z";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(195, 234);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 14;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // Form_SetTransform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 276);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericZ);
            this.Controls.Add(this.numericY);
            this.Controls.Add(this.numericX);
            this.Controls.Add(this.numericS);
            this.Controls.Add(this.numericRz);
            this.Controls.Add(this.numericRy);
            this.Controls.Add(this.numericRx);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_SetTransform";
            this.Text = "Do Transform";
            ((System.ComponentModel.ISupportInitialize)(this.numericRx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericZ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericRx;
        private System.Windows.Forms.NumericUpDown numericRy;
        private System.Windows.Forms.NumericUpDown numericRz;
        private System.Windows.Forms.NumericUpDown numericS;
        private System.Windows.Forms.NumericUpDown numericX;
        private System.Windows.Forms.NumericUpDown numericY;
        private System.Windows.Forms.NumericUpDown numericZ;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonOK;
    }
}