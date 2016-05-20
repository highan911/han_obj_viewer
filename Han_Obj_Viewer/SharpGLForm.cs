using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpGL;
using System.IO;
using MathNet.Numerics.LinearAlgebra.Double;


namespace Han_Obj_Viewer
{
    public enum DisplayMode
    {
        DEFAULT = 0,
        POINTCOLORMAP = 1,
        FACECOLORMAP = 2,
        TWO_MESHS = 3
    }



    /// <summary>
    /// The main form class.
    /// </summary>
    public partial class SharpGLForm : Form
    {
        GeometryRoot geometryRoot = new GeometryRoot();

        double currentScale = 5.0;
        bool mouseDown = false;
        int mouseX = 0;
        int mouseY = 0;
        double cameraDist = 5;
        float angleV = 45, angleH = 45;

        /* Global Settings */
        public DisplayMode displayMode = DisplayMode.DEFAULT;
        GeometryObject currentGeometryObject;

        // Colors and Marks
        ColorMap colorMap;
        List<int> MarkedPoints = new List<int>();
        List<Line> MarkedLines = new List<Line>(); 
        
        // TWO_MESHS
        GeometryObject TWO_MESHS_sourceObj;
        GeometryObject TWO_MESHS_targetObj;

        /* End Global Settings */

        Form_Id form_Id;


        /// <summary>
        /// Initializes a new instance of the <see cref="SharpGLForm"/> class.
        /// </summary>
        public SharpGLForm()
        {
            InitializeComponent();
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.onMouseWheel);
            //displayModeToolStripMenuItem.DropDownItems[0].BackColor = SystemColors.ControlDarkDark;
        }

        public bool Load_Mesh()
        {
            Model_Loader loader;

            string[] paths = null;
            string dir = System.Environment.CurrentDirectory;
            //string dir = @"D:\workspace_cs\Han_Obj_Viewer\refer\hw1_realse\";
            if (!MeshFileSelect(out paths, dir))
            {
                return false;
            }
            //loader = new Off_Loader(@"D:\workspace_cs\Han_Obj_Viewer\refer\hw1_realse\281.off");
            foreach (string path in paths)
            {
                string extension = Path.GetExtension(path);
                string filename = Path.GetFileName(path);
                if (extension == ".obj")
                {
                    loader = new Obj_Loader(path);
                }
                else if (extension == ".off")
                {
                    loader = new Off_Loader(path);
                }
                else
                {
                    MessageBox.Show("Error loading: " + path);
                    continue;
                }

                displayMode = DisplayMode.DEFAULT;

                while (geometryRoot.ContainsKey(filename)) filename += "_";
                geometryRoot.Add(filename, loader.GeometryObject);
                currentGeometryObject = geometryRoot[filename];
                ToolStripItem newitem = currentMeshToolStripMenuItem.DropDownItems.Add(filename);

                foreach (ToolStripItem item in currentMeshToolStripMenuItem.DropDownItems)
                {
                    item.BackColor = SystemColors.Control;
                }
                newitem.BackColor = SystemColors.ControlDarkDark;

                MarkedLines.Clear();
                MarkedPoints.Clear();
            }
            return true;
        }

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RenderEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            gl.Rotate(angleV, angleH, 0);
            gl.Scale(currentScale, currentScale, currentScale);
            DrawAxis(gl);



            if (currentGeometryObject != null)
            {
                switch (displayMode)
                {
                    case DisplayMode.DEFAULT:
                        currentGeometryObject.Show(gl);
                        currentGeometryObject.ShowEdge(gl);
                        break;
                    case DisplayMode.POINTCOLORMAP:
                        try
                        {
                            currentGeometryObject.ShowPointColorMap(gl, colorMap);
                            currentGeometryObject.ShowEdge(gl);
                        }
                        catch (Exception ex)
                        {
                            displayMode = DisplayMode.DEFAULT;
                            MessageBox.Show(ex.Message + ex.StackTrace);
                        }
                        break;
                    case DisplayMode.FACECOLORMAP:
                        try
                        {
                            currentGeometryObject.ShowFaceColorMap(gl, colorMap);
                            currentGeometryObject.ShowEdge(gl);
                            currentGeometryObject.DrawMarkedPoints(gl, MarkedPoints);
                        }
                        catch (Exception ex)
                        {
                            displayMode = DisplayMode.DEFAULT;
                            MessageBox.Show(ex.Message + ex.StackTrace);
                        }
                        break;
                    case DisplayMode.TWO_MESHS:
                        try
                        {
                            float[] color1 = { 0.5f, 0.5f, 0.0f };
                            float[] color2 = { 0.0f, 0.5f, 0.5f };

                            TWO_MESHS_sourceObj.Show(gl, color1);
                            TWO_MESHS_sourceObj.ShowEdge(gl);
                            TWO_MESHS_targetObj.Show(gl, color2);
                            TWO_MESHS_targetObj.ShowEdge(gl);
                        }
                        catch (Exception ex)
                        {
                            displayMode = DisplayMode.DEFAULT;
                            MessageBox.Show(ex.Message + ex.StackTrace);
                        }
                        break;
                }
                DrawMarkedLines(gl);
            }
        }

        private void DrawAxis(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_LINES);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(0, 0, 0);
            gl.Vertex(1, 0, 0);
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(0, 0, 0);
            gl.Vertex(0, 1, 0);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(0, 0, 0);
            gl.Vertex(0, 0, 1);
            gl.End();
        }

        private void DrawMarkedLines(OpenGL gl)
        {
            foreach (Line line in MarkedLines)
            {
                gl.Begin(OpenGL.GL_LINES);
                gl.Color(1.0f, 1.0f, 0.0f);
                foreach (XYZ p in line)
                {
                    gl.Vertex(p.ToArray());
                }
                gl.End();
            }
        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            //  TODO: Initialise OpenGL here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the clear color.
            gl.ClearColor(0.1f, 0.1f, 0.2f, 1.0f);
            gl.ShadeModel(OpenGL.GL_SMOOTH);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            //  TODO: Set the projection DenseMatrix here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            gl.LoadIdentity();

            //  Create a perspective transformation.
            gl.Perspective(60.0f, (double)Width / (double)Height, 0.01, 100.0);

            //  Use the 'look at' helper function to position and aim the camera.
            gl.LookAt(0, 0, cameraDist, 0, 0, 0, 0, 1, 0);

            //  Set the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        private void onMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                currentScale *= 1.2;
            }
            else
            {
                currentScale /= 1.2;
            }
        }

        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
        }

        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                int daltaX = e.X - mouseX;
                int daltaY = e.Y - mouseY;
                angleV += 0.3f * daltaY;
                angleH += 0.3f * daltaX;
                //while (angleH >= 360) angleH -= 360;
                //while (angleH < 0) angleH += 360;
                if (angleV > 90) angleV = 90;
                if (angleV < -90) angleV = -90;
            }
            mouseX = e.X;
            mouseY = e.Y;
        }


        static bool MeshFileSelect(out string[] filenames, string folder = null)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "选择Mesh文件";
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            dlg.Multiselect = true;
            //dlg.RestoreDirectory = true;
            if(folder != null)
                dlg.InitialDirectory = folder;
            dlg.Filter = "Supported Mesh Files (*.obj, *.off)|*.obj;*.off";
            bool rc = (DialogResult.OK == dlg.ShowDialog());
            filenames = dlg.FileNames;
            return rc;
        }

        static bool MeshFileSave(out string filename, string folder = null)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "保存Mesh文件";
            dlg.CheckPathExists = true;
            //dlg.RestoreDirectory = true;
            if (folder != null)
                dlg.InitialDirectory = folder;
            dlg.Filter = "Obj Files (*.obj)|*.obj|Off Files (*.off)|*.off";
            bool rc = (DialogResult.OK == dlg.ShowDialog());
            filename = dlg.FileName;
            return rc;
        }

        static bool FileSelect(out string filename, string folder = null)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "选择文件";
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            //dlg.RestoreDirectory = true;
            if (folder != null)
                dlg.InitialDirectory = folder;
            dlg.Filter = "All Files (*.*)|*.*";
            bool rc = (DialogResult.OK == dlg.ShowDialog());
            filename = dlg.FileName;
            return rc;
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Load_Mesh();
        }

        private void currentMeshToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string filename = e.ClickedItem.Text;
            changeCurrentMesh(filename);
        }

        private void changeCurrentMesh(string filename)
        {
            displayMode = DisplayMode.DEFAULT;
            currentGeometryObject = geometryRoot[filename];
            foreach (ToolStripItem item in currentMeshToolStripMenuItem.DropDownItems)
            {
                if (item.Text == filename) item.BackColor = SystemColors.ControlDarkDark;
                else item.BackColor = SystemColors.Control;
            }
        }

        private void loadFaceLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentGeometryObject == null)
            {
                MessageBox.Show("Please load a mesh");
                return;
            }
            string path = null;
            if (!FileSelect(out path))
            {
                return;
            }
            MarkedPoints.Clear();
            MarkedLines.Clear();
            StreamReader reader = new StreamReader(path);
            colorMap = new ColorMap(currentGeometryObject.Triangles);

            List<double> dataset = new List<double>();

            for (int i = 0; i < colorMap.Count; i++)
            {
                try
                {
                    string line = reader.ReadLine();
                    dataset.Add(double.Parse(line));
                }
                catch
                {
                    break;
                }
            }
            colorMap.SetDataArray(dataset);
            displayMode = DisplayMode.FACECOLORMAP;
        }



        private void loadPointLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentGeometryObject == null)
            {
                MessageBox.Show("Please load a mesh");
                return;
            }
            string path = null;
            if (!FileSelect(out path))
            {
                return;
            }
            MarkedPoints.Clear();
            MarkedLines.Clear();

            StreamReader reader = new StreamReader(path);
            colorMap = new ColorMap(currentGeometryObject.Points);

            List<double> dataset = new List<double>();
            for (int i = 0; i < colorMap.Count; i++)
            {
                try
                {
                    string line = reader.ReadLine();
                    dataset.Add(double.Parse(line));
                }
                catch
                {
                    break;
                }
            }
            colorMap.SetDataArray(dataset);
            displayMode = DisplayMode.POINTCOLORMAP;
        }

        private void loadPointSelectionLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentGeometryObject == null)
            {
                MessageBox.Show("Please load a mesh");
                return;
            }
            string path = null;
            if (!FileSelect(out path))
            {
                return;
            }
            MarkedPoints.Clear();
            MarkedLines.Clear();
            StreamReader reader = new StreamReader(path);
            colorMap = new ColorMap(currentGeometryObject.Triangles);
            MarkedPoints.Add(-1);
            for (int i = 0; i < colorMap.Count; i++)
            {
                try
                {
                    string line = reader.ReadLine();
                    MarkedPoints.Add(int.Parse(line));
                }
                catch
                {
                    break;
                }
            }
            List<double> dataset = new List<double>();

            foreach (Triangle tri in currentGeometryObject.Triangles)
            {
                int c = 0;
                if (MarkedPoints.Contains(tri.P0.Id)) c++;
                if (MarkedPoints.Contains(tri.P1.Id)) c++;
                if (MarkedPoints.Contains(tri.P2.Id)) c++;
                if (c >= 2)
                {
                    dataset.Add(1);
                }
                else
                {
                    dataset.Add(0);
                }
            }
            colorMap.SetDataArray(dataset);
            displayMode = DisplayMode.FACECOLORMAP;
        }

        private void pointNeighborToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentGeometryObject == null)
            {
                MessageBox.Show("Please load a mesh");
                return;
            }
            MarkedPoints.Clear();
            MarkedLines.Clear();
            form_Id = new Form_Id(currentGeometryObject.Points.Count, Form_IdType.POINT_NEIGHBOR, this);
            form_Id.Text = "点Id";
            form_Id.Show();
        }

        private void faceNeighborToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentGeometryObject == null)
            {
                MessageBox.Show("Please load a mesh");
                return;
            }
            MarkedPoints.Clear();
            MarkedLines.Clear();
            form_Id = new Form_Id(currentGeometryObject.Triangles.Count, Form_IdType.FACE_NEIGHBOR, this);
            form_Id.Text = "面Id";
            form_Id.Show();
        }

        private void faceNormalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentGeometryObject == null)
            {
                MessageBox.Show("Please load a mesh");
                return;
            }
            MarkedPoints.Clear();
            MarkedLines.Clear();
            form_Id = new Form_Id(currentGeometryObject.Triangles.Count, Form_IdType.FACE_NORMAL, this);
            form_Id.Text = "面Id";
            form_Id.Show();
        }

        internal void resetFaceNeighborColorMap(int tid)
        {
            if (currentGeometryObject == null)
            {
                MessageBox.Show("Please load a mesh");
                return;
            }
            List<int> tids = new List<int>();
            tids.Add(tid);
            tids.AddRange(currentGeometryObject.Triangles[tid].GetNeighborFaces());
            colorMap = new ColorMap(currentGeometryObject.Triangles, tids);
            displayMode = DisplayMode.FACECOLORMAP;
        }

        internal void resetPointNeighborColorMap(int pid)
        {
            if (currentGeometryObject == null)
            {
                MessageBox.Show("Please load a mesh");
                return;
            }

            List<int> tids = new List<int>();
            tids.Add(-1);
            tids.AddRange(currentGeometryObject.Points[pid].GetNeighborFaces());
            colorMap = new ColorMap(currentGeometryObject.Triangles, tids);
            MarkedPoints.Clear();
            MarkedPoints.Add(pid);
            MarkedPoints.AddRange(currentGeometryObject.Points[pid].GetNeighborPoints());
            displayMode = DisplayMode.FACECOLORMAP;
        }

        internal void resetFaceNormalColorMap(int tid)
        {
            if (currentGeometryObject == null)
            {
                MessageBox.Show("Please load a mesh");
                return;
            }
            List<int> tids = new List<int>();
            tids.Add(tid);
            colorMap = new ColorMap(currentGeometryObject.Triangles, tids);

            Triangle tri = currentGeometryObject.Triangles[tid];
            XYZ P0 = XYZ.Mean(tri.P0.XYZ, tri.P1.XYZ, tri.P2.XYZ);
            XYZ P1 = XYZ.Add(P0, tri.Normal().Scale(0.1));
            Line line = new Line();
            line.Add(P0);
            line.Add(P1);
            MarkedLines.Clear();
            MarkedLines.Add(line);

            displayMode = DisplayMode.FACECOLORMAP;
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ICPMeshSelection form_Selection = new Form_ICPMeshSelection(geometryRoot);
            form_Selection.DisableSVD();
            if (form_Selection.ShowDialog() != DialogResult.OK) return;
            TWO_MESHS_sourceObj = geometryRoot[form_Selection.source];
            TWO_MESHS_targetObj = geometryRoot[form_Selection.target];

            TWO_MESHS_sourceObj.Transform = new Transform();
            TWO_MESHS_targetObj.Transform = new Transform();

            displayMode = DisplayMode.TWO_MESHS;
        }

#region ICP/SVD





        private void iCPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ICP form_ICP = new Form_ICP(geometryRoot, 100);
            form_ICP.ShowDialog();
            if (form_ICP.Finished)
            {

                TWO_MESHS_sourceObj = form_ICP.TWO_MESHS_sourceObj;
                TWO_MESHS_targetObj = form_ICP.TWO_MESHS_targetObj;

                displayMode = DisplayMode.TWO_MESHS;
            }
        }


#endregion



#region SA

        
        

        private void sAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_SA form_SA = new Form_SA(geometryRoot, 100);
            form_SA.ShowDialog();
            if (form_SA.Finished)
            {

                TWO_MESHS_sourceObj = form_SA.TWO_MESHS_sourceObj;
                TWO_MESHS_targetObj = form_SA.TWO_MESHS_targetObj;

                displayMode = DisplayMode.TWO_MESHS;
            }

        }




#endregion



        private void doTransformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentGeometryObject == null) return;
            Form_SetTransform form_SetTransform = new Form_SetTransform();
            if (form_SetTransform.ShowDialog() != DialogResult.OK) return;
            DenseMatrix newMat = form_SetTransform.Transform.GetMatrix() * currentGeometryObject.Transform.GetMatrix();
            currentGeometryObject.Transform = new Transform(newMat);

            displayMode = DisplayMode.DEFAULT;
        }

        private void saveTransformedMeshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Model_Writer writer;

            if (currentGeometryObject == null) return;
            string path;
            if (!MeshFileSave(out path))
            {
                return;
            }

            string extension = Path.GetExtension(path);
            if (extension == ".obj")
            {
                writer = new Obj_Writer(path, currentGeometryObject);
            }
            else if (extension == ".off")
            {
                writer = new Off_Writer(path, currentGeometryObject);
            }
            else
            {
                MessageBox.Show("Error writing: " + path);
            }
        }
    }
}
