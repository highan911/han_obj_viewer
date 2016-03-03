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

namespace Han_Obj_Viewer
{
    public enum DisplayMode
    {
        DEFAULT = 0,
        POINTCOLORMAP = 1,
        FACECOLORMAP = 2
    }



    /// <summary>
    /// The main form class.
    /// </summary>
    public partial class SharpGLForm : Form
    {
        Model_Loader loader;

        GeometryRoot geometryRoot = new GeometryRoot();

        double currentScale = 5.0;
        bool mouseDown = false;
        int mouseX = 0;
        int mouseY = 0;
        double cameraDist = 5;
        float angleV = 45, angleH = 45;

        PointColorMap colorMap;
        GeometryObject currentGeometryObject;

        public DisplayMode displayMode = DisplayMode.DEFAULT;

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
            string path = null;
            //string dir = System.Environment.CurrentDirectory;
            string dir = @"D:\workspace_cs\Han_Obj_Viewer\refer\hw1_realse\";
            if (!MeshFileSelect(out path, dir))
            {
                return false;
            }
            //loader = new Off_Loader(@"D:\workspace_cs\Han_Obj_Viewer\refer\hw1_realse\281.off");
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
                return false;
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

            //colorMap = new ColorMap(currentGeometryObject.Points);
            //int n = currentGeometryObject.Points.Count;
            //int i = 0;
            //foreach (int point in currentGeometryObject.Points.Keys)
            //{
            //    colorMap.SetData(point, (double)i / (double)n);
            //    i++;
            //}
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
                        }
                        catch
                        {
                            displayMode = DisplayMode.DEFAULT;
                            MessageBox.Show("Error: No Color Map");
                        }
                        break;
                }
                
                //
                
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
            //gl.ShadeModel(OpenGL.GL_SMOOTH);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            //  TODO: Set the projection matrix here.

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


        static bool MeshFileSelect(out string filename, string folder = null)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "选择Mesh文件";
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            //dlg.RestoreDirectory = true;
            if(folder != null)
                dlg.InitialDirectory = folder;
            dlg.Filter = "Supported Mesh Files (*.obj, *.off)|*.obj;*.off";
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
            currentGeometryObject = geometryRoot[filename];
            foreach (ToolStripItem item in currentMeshToolStripMenuItem.DropDownItems)
            {
                if (item.Text == filename) item.BackColor = SystemColors.ControlDarkDark;
                else item.BackColor = SystemColors.Control;
            }
        }

        private void loadLabelToolStripMenuItem_Click(object sender, EventArgs e)
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
            StreamReader reader = new StreamReader(path);
            colorMap = new PointColorMap(currentGeometryObject.Points);

            for (int i = 0; i < colorMap.Count; i++)
            {
                try
                {
                    string line = reader.ReadLine();
                }
                catch
                {
                    break;
                }
            }

        }

    }
}
