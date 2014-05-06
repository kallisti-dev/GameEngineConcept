using System;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Drawing;
using Img = System.Drawing.Imaging;

namespace GameEngineConcept.Graphics
{
    public class Texture : IRelease
    {

        private int textureId;

        public int Width {get; set;}
        public int Height {get; set;}
        public TextureTarget Target { get; set; }

        public bool IsNull { get { return textureId == 0; } }

        private Texture(int width, int height, TextureTarget target)
        {
            textureId = GL.GenTexture();
            Width = width;
            Height = height;
            Target = target;
        }

        private static void Set2DTextureParameters()
        {
            GL.TexParameter(
              TextureTarget.Texture2D,
              TextureParameterName.TextureMinFilter,
              (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D,
              TextureParameterName.TextureMagFilter,
              (int)TextureMagFilter.Nearest);
        }

        /* bind this texture for use in openGL calls */
        public void Bind(TextureUnit unit, Action inner)
        {
            GL.ActiveTexture(unit);
            Bind(inner);
        }

        public void Bind(Action inner)
        {
            GL.BindTexture(Target, textureId);
            try { inner(); }
            finally { GL.BindTexture(Target, 0); }
        }


        public static Texture FromFile(string path)
        {
            return FromBitmap(new Bitmap(path));
        }

        public static Texture FromBitmap(Bitmap bitmap)
        {
          Img.BitmapData data = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            Img.ImageLockMode.ReadOnly,
            Img.PixelFormat.Format32bppArgb);
          Texture tex = new Texture(data.Width, data.Height, TextureTarget.Texture2D);
          tex.Bind(() => {
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
            Set2DTextureParameters();
          });
          return tex;
        }

        public void Release()
        {
            GL.DeleteTexture(textureId);
        }

        public void Dispose()
        {
            EngineWindow.ReleaseOnMainThread(this);
            GC.SuppressFinalize(this);
        }
    }
}
