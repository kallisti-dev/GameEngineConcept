﻿using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept
{
    public struct TexturedVertex2
    {

        public static VertexAttribute[] vertexAttributes;

        //static constructor
        static TexturedVertex2() {
            Type t = typeof(TexturedVertex2);
            int size = Marshal.SizeOf(t);
            vertexAttributes = new[] {
                new VertexAttribute(0, 2, VertexAttribPointerType.Float, false, size, (int)Marshal.OffsetOf(t, "position")),
                new VertexAttribute(1, 2, VertexAttribPointerType.Float, false, size, (int)Marshal.OffsetOf(t, "texel"))
            };
        }

        public Vector2 position;
        public Vector2 texel;

        public TexturedVertex2(Vector2 pos, Vector2 tex) {
            position = pos;
            texel = tex;
        }

        public TexturedVertex2(float posX, float posY, float texX, float texY)
        {
            position = new Vector2(posX, posY);
            texel    = new Vector2(texX, texY);
        }
    }
}
