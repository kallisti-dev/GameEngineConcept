
namespace GameEngineConcept.Components
{
    //abstract base class for components that are not categorized by other base component types
    //children of this class will be executed last in the update chain
    public abstract class MiscellaneousComponent : IComponent
    {
        public abstract void Update();
    }
}
