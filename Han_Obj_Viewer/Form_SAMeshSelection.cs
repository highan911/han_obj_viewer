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
    public partial class Form_SAMeshSelection : Form
    {
        GeometryRoot geometryRoot;
        public string source = null;
        public string target = null;
        public int SourceSamples, TargetSamples;

        public Form_SAMeshSelection(GeometryRoot geometryRoot)
        {
            this.geometryRoot = geometryRoot;
            InitializeComponent();
            foreach (string geoName in geometryRoot.Keys)
            {
                comboBoxSource.Items.Add(geoName);
                comboBoxTarget.Items.Add(geoName);
            }
        }

        private void comboBoxSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            source = comboBoxSource.SelectedItem.ToString();
        }

        private void comboBoxTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            target = comboBoxTarget.SelectedItem.ToString();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (source != null && target != null)
            {
                this.SourceSamples = (int)numericSourceSamples.Value;
                this.TargetSamples = (int)numericTargetSamples.Value;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
