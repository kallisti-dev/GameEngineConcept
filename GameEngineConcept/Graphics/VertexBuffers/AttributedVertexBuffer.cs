﻿using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept.Graphics.VertexBuffers
{
    class AttributedVertexBuffer : IAttributedVertexBuffer, IHasVertexBuffer<IBindableVertexBuffer>, IHasVertexAttributes
    {
        public IBindableVertexBuffer VBuffer { get; private set; }
        public VertexAttribute[] VertexAttributes { get; private set; }

        public AttributedVertexBuffer(IBindableVertexBuffer buffer, VertexAttribute[] attributes)
        {
            VBuffer = buffer;
            VertexAttributes = attributes;
        }

        public void LoadData<T>(BufferUsageHint hint, T[] data) where T : struct
        {
            VBuffer.LoadData(hint, data);
        }

        public void Bind(BufferTarget target, IEnumerable<int> enabledAttributes, Action<IBoundVertexBuffer> inner)
        {
            if (enabledAttributes == null)
            {
                Bind(target, inner);
                return;
            }
            VBuffer.Bind(target, (b) =>
            {
                b.WithAttributes(VertexAttributes, enabledAttributes, () => inner(b));
            });
        }

        public void Bind(BufferTarget target, Action<IBoundVertexBuffer> inner)
        {
            VBuffer.Bind(target, (b) =>
            {
                b.WithAttributes(VertexAttributes, () => inner(b));
            });
        }
    }
}
