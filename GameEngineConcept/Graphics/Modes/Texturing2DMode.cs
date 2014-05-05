using System;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept.Graphics.Modes
{
    class Texturing2DMode : IGraphicsMode
    {   
        public MatrixMode? PrimaryMatrixMode
        {
            get { return MatrixMode.Modelview; }
        }

        public void Initialize() 
        {
            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
        }

        public void Uninitialize()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
        }
    }
}
