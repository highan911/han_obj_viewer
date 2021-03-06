﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Han_Obj_Viewer
{
    public partial class Form_ICPMeshSelection : Form
    {
        GeometryRoot geometryRoot;
        public string source = null;
        public string target = null;
        public int SVDLoops, SourceSamples, TargetSamples;
        
        public Form_ICPMeshSelection(GeometryRoot geometryRoot)
        {
            this.geometryRoot = geometryRoot;
            InitializeComponent();
            foreach (string geoName in geometryRoot.Keys)
            {
                comboBoxSource.Items.Add(geoName);
                comboBoxTarget.Items.Add(geoName);
            }
        }

        public void DisableSVD()
        {
            numericSVD.Enabled = false;
            numericSourceSamples.Enabled = false;
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
                this.SVDLoops = (int)numericSVD.Value;
                this.SourceSamples = (int)numericSourceSamples.Value;
                this.TargetSamples = (int)numericTargetSamples.Value;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
