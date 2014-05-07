using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    public class Sprite : TexturedVertexSet, IDrawableDepth, IHasPosition<Vector2>, IHasDimensions<int>
    {
        public static VertexAttribute[] vAttributes = TexturedVertex2.vAttributes;

        public static TexturedVertex2[] CreateVertices (Vector2 pos, Rectangle texRect)
        {
            return new[] {
                new TexturedVertex2(pos,                                                        new Point(texRect.Left, texRect.Top)),
                new TexturedVertex2(new Vector2(pos.X, pos.Y + texRect.Height),                 new Point(texRect.Left, texRect.Bottom)),
                new TexturedVertex2(new Vector2(pos.X + texRect.Width, pos.Y),                  new Point(texRect.Right, texRect.Top)),
                new TexturedVertex2(new Vector2(pos.X + texRect.Width, pos.Y + texRect.Height), new Point(texRect.Right, texRect.Bottom))
            };
        }

        public int DrawDepth { get; protected set; }

        int index;
        protected int BufferIndex
        {
            get { return index; }
            set
            {
                index = value;
                indices =  VertexIndices.Create(value, 4);
            }
        }

        public Vector2 Position
        {
            get { return Vertices[0].position; }
            set
            {
                TexturedVertex2[] vs = Vertices;
                Vector2 pos = vs[0].position;
                Vector2 posDiff = pos - value;
                vs[0].position = value;
                for(int i = 1; i < 4; ++i)
                {
                    vs[i].position += posDiff;
                }
                Vertices = vs;
            }
        }

        int? width = null, height = null;
        public int Width 
        { 
            get {
                if (!width.HasValue) CalcDimensions();
                return width.Value;

            } 
        }
        public int Height 
        { 
            get {
                if (!height.HasValue) CalcDimensions();
                return height.Value;

            } 
        }

        TexturedVertex2[] vertices;
        TexturedVertex2[] Vertices
        {
            get
            {
                if (vertices != null)
                    return vertices;
                return vertices = VBuffer.GetData<TexturedVertex2>(BufferIndex, 4);
            }
            set
            {
                Debug.Assert(value.Length == 4);
                vertices = value;
                VBuffer.SetData<TexturedVertex2>(BufferIndex, vertices);
            }
        }

        public Sprite(Texture tex, IBindableVertexBuffer buffer, int bufferInd, int depth = 0)
            : base(tex, PrimitiveType.Quads, new AttributedVertexBuffer(buffer, vAttributes), VertexIndices.Create(bufferInd, 4), null)
        {
            DrawDepth = depth;
            index = bufferInd;
        }

        //translates texture coordinates of the sprite by a given coordinate pair
        public void TranslateTexels(Point v)
        {
            TexturedVertex2[] vs = Vertices;
            for(int i = 0; i < 4; ++i)
            {
                vs[i].texel.Offset(v);
            }
            Vertices = vs;
        }

        public void SetTexCoord(Point coord)
        {
            TexturedVertex2[] vs = Vertices;
            Point tex = vs[0].texel;
            Point texDiff = tex.Subtract(coord);
            vs[0].texel = coord;
            for (int i = 1; i < 4; ++i)
            {
                vs[i].texel.Offset(texDiff);
            }
            Vertices = vs;
        }

        //basically the same as sprite.Position += v, but slightly more efficient
        public void TranslatePosition(Vector2 v)
        {
            {
                TexturedVertex2[] vs = Vertices;
                for (int i = 0; i < 4; ++i)
                {
                    vs[i].position += v;
                }
                Vertices = vs;
            }
        }

        private void CalcDimensions()
        {
            var t1 = Vertices[0].texel;
            var t2 = Vertices[3].texel;
            width = Math.Abs(t1.X - t2.X);
            height = Math.Abs(t1.Y - t2.Y);
        }
    }
}
