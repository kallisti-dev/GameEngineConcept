
namespace GameEngineConcept.Components.Physics
{
    //simple 2-dimensional particle physics
    class ParticlePhysics2
    {
        IHasPosition<float> pos;

        public ParticlePhysics2(IHasPosition<float> position) {
            pos = position;
        }
    }
}
