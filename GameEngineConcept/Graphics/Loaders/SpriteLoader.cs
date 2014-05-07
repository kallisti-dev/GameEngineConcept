using System;
using System.Collections.Generic;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics.Loaders
{
    public class SpriteLoader : BaseVertexLoader<AttributedVertexBuffer, TexturedVertex2, SpriteLoader.State, Sprite>
    {

        public class State
        {
            public Texture tex;
            //public Rectangle rect;
            //public Vector2 pos;
            public PrimitiveType type;
            public int index;
            public int depth;
            public int[] enabledAttribs;
        }

        public SpriteLoader(BufferUsageHint hint, IBindableVertexBuffer buffer) 
            : base(hint, new AttributedVertexBuffer(buffer, TexturedVertex2.vAttributes)) { }

        public void AddSprite(Texture tex, Vector2 pos, Rectangle rect, int depth = 0, int[] enabledAttribs = null)
        {
            AddState(new State {
                tex = tex,
                //rect = rect,
                //pos = pos,
                depth = depth,
                index = currentIndex,
                enabledAttribs = enabledAttribs
            });
            AddVertices(Sprite.CreateVertices(pos, rect));
        }

        protected override Sprite CreateVertexOutput(State state)
        {
            return new Sprite(state.tex, VBuffer, state.index, state.depth);
        }
    }
}
