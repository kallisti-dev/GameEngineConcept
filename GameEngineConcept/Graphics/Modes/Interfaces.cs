using System;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept.Graphics.Modes
{
    //an interface for handling different openGL initializations
    public interface IGraphicsMode
    {
        //when applying graphics modes to an EngineWindow, matrix modes are automatically handled.
        //the graphics mode must specify its primary matrix mode, or null if none is used.
        MatrixMode? PrimaryMatrixMode { get; }

        //any initialization code for the graphics mode.
        void Initialize();
        //any uninitialization code for the graphics mode.
        void Uninitialize();
    }
}
