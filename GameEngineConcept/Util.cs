using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;

namespace GameEngineConcept
{
    using Graphics;
    using Graphics.VertexBuffers;

    public class Util
    {

        public static VertexAttribute[] vector2Attributes = new[] { TexturedVertex2.vAttributes[0] } ;

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
    }
}
