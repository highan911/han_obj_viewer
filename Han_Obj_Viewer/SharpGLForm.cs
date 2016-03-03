using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpGL;

namespace Han_Obj_Viewer
{
    /// <summary>
    /// The main form class.
    /// </summary>
    public partial class SharpGLForm : Form
    {
        Model_Loader loader;
        double currentScale = 5.0;
        bool mouseDown = false;
        int mouseX = 0;
        int mouseY = 0;
        double cameraDist = 5;
        float angleV = 45, angleH = 45;

        ColorMap colorMap;


        /// <summary>
        /// Initializes a new instance of the <see cref="SharpGLForm"/> class.
        /// </summary>
        public SharpGLForm()
        {
            InitializeComponent();
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.onMouseWheel);
            loader = new Off_Loader(@"D:\workspace_cs\Han_Obj_Viewer\refer\hw1_realse\281.off");



            colorMap = new ColorMap(loader.Points);
            int n = loader.Points.Count;
            int i=0;
            foreach (int point in loader.Points.Keys)
            {
                colorMap.SetData(point, (double)i / (double)n);
                //colorMap.SetData(point, 0.9);
                i++;
            }


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

            //loader.Show(gl);
            
            loader.ShowColorMap(gl, colorMap);
            //loader.ShowEdge(gl);

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
            gl.LookAt(0, 0, 5, 0, 0, 0, 0, 1, 0);

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

    }
}
