using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

using GameEngineConcept;
using GameEngineConcept.Graphics.Loaders;
using GameEngineConcept.Graphics;
using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineTest.Tests
{
    class Triangle2DDrawTest : BaseTester
    {
        IEnumerable<VertexSet> loadedVertexSets;

        public override void OnLoad(TestWindow window) 
        {
            base.OnLoad(window);
            //a vertex buffer is an array of vertices within video memory
            //
            //In this test we allocate a VertexBuffer directly. However, for performance in-game we use a Pool<VertexBuffer>
            //to request/release VertexBuffers. Pooling vertexbuffers allows us to recycle buffers rather than wasting time
            //destroying and allocating them.
            VertexBuffer buffer = VertexBuffer.Allocate();

            //in order for openGL to know the size and structure of our vertex components, we associate
            //an array of VertexAttributes to our buffer. each element of the array specifies a component
            //of a vertex within the buffer. 
            AttributedVertexBuffer aBuffer = new AttributedVertexBuffer(buffer, Util.vector2Attributes);
            //for vertex types defined within the engine, the VertexAttribute array that describes the struct layout
            //is specified as a static variable on the struct named vAttributes. Util class
            //holds vertex attributes for OpenTK types
            //
            //in the future it would be nice if this step is automatically handled somewhere in the engine. Unfortunately,
            //since interfaces cannot have static members we would have to use reflection libraries to auto-generate VertexAttributes.

            //we use a loader to load vertices into an vertex buffer.
            VertexLoader<Vector2> loader = new VertexLoader<Vector2>(BufferUsageHint.DynamicDraw, aBuffer);
            //For VertexLoader, we must manually specify attributes for the buffer. However, higher-level loaders
            //such as SpriteLoader will automatically associate attributes to the buffer.
            //
            //the "dynamic draw" usage hint tells openGL that we only intend to write data to the buffer.
            //the usage hint only affects the performance of the buffer, not its behavior.

            //add a triangle.
            loader.AddVertexSet(
                PrimitiveType.Triangles,
                new[] {
                    new Vector2(0, 0),  //for simple position vertices, we use Vector2 struct from OpenTK library.
                    new Vector2(0, 1),  //
                    new Vector2(1, 1)   //for textured vertices we would use the engine's TexturedVertex2 struct
            });

            //add a second triangle
            loader.AddVertexSet(
                PrimitiveType.Triangles,
                new[] {
                    new Vector2(0, 0),
                    new Vector2(0, -1),
                    new Vector2(-1, -1)
            });

            //once all the vertices have been added to the loader, we load the vertex buffer
            loadedVertexSets = loader.Load();
            //The result is an enumeration of VertexSet objects, which represents
            //a drawable set of vertices within the vertex buffer.
            //In this test, a VertexSet represents a single 2D triangle.
        }
        public override void OnRenderFrame(FrameEventArgs e) 
        {
            base.OnRenderFrame(e);
            //our loaded vertex sets now represent on-screen objects that can be drawn
            foreach (VertexSet v in loadedVertexSets) {
                v.Draw();
            }
        }
    }
}
