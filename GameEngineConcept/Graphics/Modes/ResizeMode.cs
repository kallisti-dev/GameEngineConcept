using System;
using OpenTK.Graphics.OpenGL;
namespace GameEngineConcept.Graphics.Modes
{
    public class ResizeMode : IGraphicsMode
    {
        public MatrixMode? PrimaryMatrixMode { get { return MatrixMode.Projection; } }

        int width;
        int height;

        public ResizeMode(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void Initialize()
        {
            GL.LoadIdentity();
            double ar = (double)width / (double)height; //aspect ratio
            GL.Ortho(-2 * ar, 2 * ar, -2, 2, -1, 1);
            GL.Viewport(0, 0, width, height);
        }

        public void Uninitialize()
        { }
    }
}
