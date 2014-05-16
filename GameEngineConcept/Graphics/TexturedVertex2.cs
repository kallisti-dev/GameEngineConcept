﻿using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{

    using VertexAttributes;

    [StructLayout(LayoutKind.Sequential)]
    [VertexAttributeDefaults]
    public struct TexturedVertex2 : IHasVertexAttributes
    {

        public static VertexAttributeSet vAttributes = VertexAttributeSet.Create(typeof(TexturedVertex2));

        public VertexAttributeSet VertexAttributes { get { return vAttributes; } }

        [NormalizeComponents(true)]
        public Vector2 position;

        [NormalizeComponents(false)]
        public Point texel;

        public TexturedVertex2(Vector2 pos, Point tex) {
            position = pos;
            texel = tex;
        }

        public TexturedVertex2(float posX, float posY, int texX, int texY)
        {
            position = new Vector2(posX, posY);
            texel    = new Point(texX, texY);
        }
    }
}
