using System;
using OpenTK;

namespace GameEngineTest
{
    public interface ITester
    {

        event Action<bool> TestComplete;

        void OnLoad();
        void OnUnload();
        void OnRenderFrame(FrameEventArgs e);
        void OnUpdateFrame(FrameEventArgs e);
        void OnResize();
    }
}
