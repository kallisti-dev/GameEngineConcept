using System;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Drawing;
using Img = System.Drawing.Imaging;

namespace GameEngineConcept
{
    struct Texture2D : IDisposable
    {

        private int textureId;

        public int Width {get; set;}
        public int Height {get; set;}

        public bool IsNull { get { return textureId == 0; } }

        private Texture2D(int width, int height) : this()
        {
            textureId = GL.GenTexture();
            Width = width;
            Height = height;
        }

        public static void InitializeGL()
        {
            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
        }

        public static void UninitializeGL()
        {
            GL.Disable(EnableCap.Texture2D);
        }

        private static void SetTextureParameters()
        {
            GL.TexParameter(
              TextureTarget.Texture2D,
              TextureParameterName.TextureMinFilter,
              (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D,
              TextureParameterName.TextureMagFilter,
              (int)TextureMagFilter.Nearest);
        }

        public static Texture2D FromFile(string path)
        {
            return FromBitmap(new Bitmap(path));
        }

        /* bind this texture for use in openGL calls */
        private void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, textureId);
        }

        public static Texture2D FromBitmap(Bitmap bitmap)
        {
          Img.BitmapData data = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            Img.ImageLockMode.ReadOnly,
            Img.PixelFormat.Format32bppArgb);
          Texture2D tex = new Texture2D(data.Width, data.Height);
          tex.Bind();
          GL.TexImage2D(
            TextureTarget.Texture2D,
            0,
            PixelInternalFormat.Rgba,
            data.Width, data.Height,
            0,
            PixelFormat.Bgra,
            PixelType.UnsignedByte,
            data.Scan0);
          bitmap.UnlockBits(data);
          SetTextureParameters();
          return tex;
        }

        private void BeginDraw(PrimitiveType t)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            Bind();
            GL.Begin(t);
        }

        private void EndDraw()
        {
            GL.End();
        }

        public void Draw(Rectangle rec)
        {
            BeginDraw(PrimitiveType.Quads);
            GL.TexCoord2(0, 0); GL.Vertex2(rec.X, rec.Y);
            GL.TexCoord2(Width, 0); GL.Vertex2(rec.Right, rec.Y);
            GL.TexCoord2(Width, Height); GL.Vertex2(rec.Right, rec.Bottom);
            GL.TexCoord2(0, Height); GL.Vertex2(rec.X, rec.Bottom);
            EndDraw();
        }

        public void Dispose()
        {
            if (textureId != 0) {
                GL.DeleteTexture(textureId);
                textureId = 0;
            }
            GC.SuppressFinalize(this);
        }
    }
}
