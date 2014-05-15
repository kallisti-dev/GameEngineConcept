using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using Img = System.Drawing.Imaging;

namespace GameEngineConcept.Graphics
{
    public class Texture : IRelease, IHasDimensions<int>
    {
        private int textureId;

        public int Width {get; protected set;}
        public int Height {get; protected set;}
        public TextureTarget? Target { get; set; }

        public bool IsNull { get { return textureId == 0; } }

        private Texture(int id, TextureTarget? target = null, int width = 0, int height = 0)
        {
            textureId = id;
            Width = width;
            Height = height;
            Target = target;
        }

        public static Texture Allocate()
        {
            return new Texture(GL.GenTexture());
        }

        public static IEnumerable<Texture> Allocate(int n)
        {
            int[] texIds = new int[n];
            GL.GenBuffers(n, texIds);
            return texIds.Select((id) => new Texture(id));
        }

        public static IEnumerable<Texture> Allocate(uint n)
        {
            return Allocate(Convert.ToInt32(n));
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
            GL.BindTexture(Target.Value, textureId);
            try { inner(); }
            finally { GL.BindTexture(Target.Value, 0); }
        }


        public void LoadImageFile(string path)
        {
            LoadBitmap(new Bitmap(path));
        }

        public void LoadBitmap(Bitmap bitmap)
        {
          Img.BitmapData data = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            Img.ImageLockMode.ReadOnly,
            Img.PixelFormat.Format32bppArgb);
          Target = TextureTarget.Texture2D;
          Bind(() => {
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
