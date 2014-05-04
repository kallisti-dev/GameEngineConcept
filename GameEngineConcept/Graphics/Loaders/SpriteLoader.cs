using System;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics.Loaders
{
    public class SpriteLoader : AbstractVertexLoader<AttributedVertexBuffer, TexturedVertex2, SpriteLoader.State, Sprite>
    {

        public class State
        {
            public Texture tex;
            public Rectangle rect;
            public PrimitiveType type;
            public int index;
            public int depth;
            public int[] enabledAttribs;
        }

        public SpriteLoader(BufferUsageHint hint, IBindableVertexBuffer buffer) 
            : base(hint, new AttributedVertexBuffer(buffer, TexturedVertex2.vAttributes)) { }

        public void AddSprite(Texture tex, Rectangle r, Vector2 v, int depth = 0, int[] enabledAttribs = null)
        {
            AddState(new State {
                tex = tex,
                rect = r,
                depth = depth,
                index = currentIndex,
                enabledAttribs = enabledAttribs
            });
            AddVerticies(new[] {
                new TexturedVertex2(v,                                          new Vector2(r.Left, r.Top)),
                new TexturedVertex2(new Vector2(v.X, v.Y + r.Height),           new Vector2(r.Left, r.Bottom)),
                new TexturedVertex2(new Vector2(v.X + r.Width, v.Y + r.Height), new Vector2(r.Right, r.Bottom)),
                new TexturedVertex2(new Vector2(v.X + r.Width, v.Y),            new Vector2(r.Right, r.Top))
            });
        }

        protected override Sprite CreateVertexOutput(State state)
        {
            return new Sprite(state.tex, buffer, state.index, state.depth, state.enabledAttribs);
        }
    }
}
