using GameEngineConcept.Graphics.VertexBuffers;
using System.Threading;
using System.Threading.Tasks;

namespace GameEngineConcept.Scenes
{


    //TODO: progress meters?
    public interface IScene
    {
        Task Load(ResourcePool pool, CancellationToken token);
        void Activate(GameState state);
    }
}
