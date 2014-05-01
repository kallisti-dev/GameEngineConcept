using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Drawing;
using Img = System.Drawing.Imaging;

namespace GameEngineConcept
{
    struct Texture2D
    {

        protected int textureId;

        private Texture2D(int id)
        {
            textureId = id;
        }

        public static void InitializeGL()
        {
            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
        }

        private static int AllocateTexture()
        {
            int tex = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, tex);
            return tex;
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
            return FromBitmap(new Bitmap(Bitmap.FromFile(path)));
        }

        public static Texture2D FromBitmap(Bitmap bitmap)
        {
          Img.BitmapData data = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            Img.ImageLockMode.ReadOnly,
            Img.PixelFormat.Format32bppArgb);
          var tex = AllocateTexture();
          GL.BindTexture(TextureTarget.Texture2D, tex);
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
          return new Texture2D(tex);
        }
    }
}
