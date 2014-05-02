using System;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Drawing;
using Img = System.Drawing.Imaging;

namespace GameEngineConcept
{
    public class Texture : IDisposable
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

        public static void Initialize2DTexturing()
        {
            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
        }

        public static void Uninitialize2DTexturing()
        {
            GL.Disable(EnableCap.Texture2D);
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


        public static Texture FromBitmap(string path)
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

        //delete texture from video memory when struct is garbage collected
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
