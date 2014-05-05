using System;
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
            public Rectangle rect;
            public Vector2 pos;
            public PrimitiveType type;
            public int index;
            public int depth;
            public int[] enabledAttribs;
        }

        public SpriteLoader(BufferUsageHint hint, IBindableVertexBuffer buffer) 
            : base(hint, new AttributedVertexBuffer(buffer, TexturedVertex2.vAttributes)) { }

        public void Add(Texture tex, Rectangle rect, Vector2 pos, int depth = 0, int[] enabledAttribs = null)
        {
            AddState(new State {
                tex = tex,
                rect = rect,
                pos = pos,
                depth = depth,
                index = currentIndex,
                enabledAttribs = enabledAttribs
            });
            AddVerticies(new[] {
                new TexturedVertex2(pos,                                                  new Vector2(rect.Left, rect.Top)),
                new TexturedVertex2(new Vector2(pos.X, pos.Y + rect.Height),              new Vector2(rect.Left, rect.Bottom)),
                new TexturedVertex2(new Vector2(pos.X + rect.Width, pos.Y + rect.Height), new Vector2(rect.Right, rect.Bottom)),
                new TexturedVertex2(new Vector2(pos.X + rect.Width, pos.Y),               new Vector2(rect.Right, rect.Top))
            });
        }

        protected override Sprite CreateVertexOutput(State state)
        {
            return new Sprite(state.tex, buffer, state.index, state.depth);
        }
    }
}
