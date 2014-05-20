using System.Collections.Generic;

namespace GameEngineConcept.Components
{
    public class ComponentCollection : List<IComponent>, IComponentCollection
    {
        public void Update(GameState state)
        {
            foreach (IComponent c in this) { c.Update(state); }
        }
    }
}
