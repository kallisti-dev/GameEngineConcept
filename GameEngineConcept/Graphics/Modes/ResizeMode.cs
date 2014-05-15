using OpenTK.Graphics.OpenGL4;

namespace GameEngineConcept.Graphics.Modes
{
    public class ResizeMode : IGraphicsMode
    {
        int width;
        int height;

        public ResizeMode(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void Initialize()
        {
            double ar = (double)width / (double)height; //aspect ratio
            GL.Viewport(0, 0, width, height);
        }

        public void Uninitialize()
        { }
    }
}
