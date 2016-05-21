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
    public partial class Form_SetTransform : Form
    {
        double Rx, Ry, Rz, S, X, Y, Z;
        public Transform Transform;
        public Form_SetTransform()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Rx = Math.PI * (double)numericRx.Value / 180;
            Ry = Math.PI * (double)numericRy.Value / 180;
            Rz = Math.PI * (double)numericRz.Value / 180;
            S = (double)numericS.Value;
            X = (double)numericX.Value;
            Y = (double)numericY.Value;
            Z = (double)numericZ.Value;
            Transform = new Transform();
            Transform.DoScale(S);
            Transform.DoRotateX(Rx);
            Transform.DoRotateY(Ry);
            Transform.DoRotateZ(Rz);
            Transform.DoMove(X, Y, Z);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
