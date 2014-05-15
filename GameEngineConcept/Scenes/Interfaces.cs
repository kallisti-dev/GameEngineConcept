using System.Threading.Tasks;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Scenes
{
    //TODO: progress meters?
    public interface IScene
    {
        bool IsLoaded { get; }

        Task Load(Pool<VertexBuffer> pool);

        void Unload(Pool<VertexBuffer> pool);

        void Activate(EngineWindow window);

        void Deactivate(EngineWindow window);

    }
}
