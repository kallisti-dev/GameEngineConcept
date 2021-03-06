﻿using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace GameEngineConcept.Graphics.Loaders
{
    using VertexAttributes;
    using VertexBuffers;

    public class SpriteLoader : BaseVertexLoader<IAttributedVertexBuffer, TexturedVertex2, Sprite>
    {

        public SpriteLoader(BufferUsageHint hint, IBindableVertexBuffer buffer) 
            : base(hint, new AttributedVertexBuffer(buffer, VertexAttributeSet.FromType<TexturedVertex2>())) { }

        public void AddSprite(Texture tex, Vector2 pos, Rectangle rect, int depth = 0)
        {
            AddState(new Sprite(tex, VBuffer, currentIndex, depth));
            AddVertices(Sprite.CreateVertices(pos, rect));
        }
    }
}
