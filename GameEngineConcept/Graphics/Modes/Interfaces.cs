using System;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept.Graphics.Modes
{
    public interface IGraphicsMode
    {
        MatrixMode? PrimaryMatrixMode { get; }
        void Initialize();
        void Uninitialize();
    }
}
