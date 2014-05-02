using System;
using System.Drawing;

namespace GameEngineConcept
{
    public class Sprite
    {
        Texture tex;
        VertexBuffer buffer;
        Rectangle texRect;
        Rectangle vertexRect;

        //TODO property: bool isLoaded


        public Sprite(Texture tex, Rectangle texRect, Rectangle vertexRect)
        {
            this.tex = tex;
            this.texRect = texRect;
            this.vertexRect = vertexRect;
        }
    }
}
