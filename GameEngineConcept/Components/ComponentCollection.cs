using System.Collections.Generic;

namespace GameEngineConcept.Components
{
    public class ComponentCollection : HashSet<IComponent>, IComponentCollection
    {
        public void Update()
        {
            foreach (IComponent c in this) { c.Update(); }
        }
    }
}
