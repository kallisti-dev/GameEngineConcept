using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept
{
    class Util
    {
        //convenience function for debugging GL errors
        public static ErrorCode TraceGLError()
        {
            var err = GL.GetError();
            if (err != ErrorCode.NoError) {

                Debug.Print("GL Error found: err.ToString()");
                Debug.Print(new StackTrace(1, true).ToString());
                Debugger.Break();
            }
            return err;
        }
    }
}
