
namespace GameEngineConcept.Graphics.Modes
{
    //an interface for handling different openGL initializations
    public interface IGraphicsMode
    {
        //any initialization code for the graphics mode.
        void Initialize();
        //any uninitialization code for the graphics mode.
        void Uninitialize();
    }
}
