using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngineConcept.Components;

namespace GameEngineTest
{
    /* tests message broadcasting functionality of ComponentCollection */
    public class BroadcastTestComponent : IComponent, IReceiverComponent<ComponentCollection>
    {
        static bool success = false;

        public void Update() { }
        public void Receive(ComponentCollection c)
        {
            success = true;
        }

        public static Task<bool> Test()
        {
            ComponentCollection collection = new ComponentCollection();
            ComponentCollection collection2 = new ComponentCollection();
            BroadcastTestComponent c = new BroadcastTestComponent();
            collection.Add(collection2);
            collection2.Add(c);
            collection.Broadcast(collection);
            return Task.FromResult(success);
        }
    }
}
