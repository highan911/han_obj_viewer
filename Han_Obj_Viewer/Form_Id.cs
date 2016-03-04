using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Han_Obj_Viewer
{
    public enum Form_IdType
    {
        POINT,
        FACE
    }

    public partial class Form_Id : Form
    {
        int Range;
        int Value = 0;
        SharpGLForm mainForm;
        Form_IdType form_IdType;
        public Form_Id(int range, Form_IdType form_IdType, SharpGLForm mainForm)
        {
            this.mainForm = mainForm;
            this.form_IdType = form_IdType;
            Range = range;
            InitializeComponent();
            numericUpDown1.Maximum = Range;
            numericUpDown1.Minimum = 0;

            label1.Text = "(0 ~ " + range.ToString() + ")";

            numericValueChanged(0);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Value = (int)numericUpDown1.Value;
            numericValueChanged(Value);
        }

        private void numericValueChanged(int Value)
        {
            if (form_IdType == Form_IdType.FACE)
            {
                mainForm.resetFaceNeighborColorMap(Value);
            }
            else
            {
                mainForm.resetPointNeighborColorMap(Value);
            }
        }

    }
}
