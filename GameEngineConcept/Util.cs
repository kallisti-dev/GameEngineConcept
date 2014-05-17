using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace GameEngineConcept
{
    using Graphics.VertexAttributes;

    public class Util
    {

        public const BindingFlags AllInstanceFields = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField;

        public static VertexAttributeSet vector2Attributes = VertexAttributeSet.FromType<Vector2>();

        //convenience function for debugging GL errors
        public static ErrorCode TraceGLError()
        {
            var err = GL.GetError();
            if (err != ErrorCode.NoError) {

                Debug.Print("GL Error found: " + err.ToString());
                Debug.Print(new StackTrace(1, true).ToString());
                Debugger.Break();
            }
            return err;
        }

        public static Comparer<A> CompareBy<A,B>(Func<A,B> f) where
            B : IComparable<B>
        {
            return Comparer<A>.Create((x, y) => f(x).CompareTo(f(y)));
        }

    }
}
