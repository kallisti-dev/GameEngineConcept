using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngineConcept;

namespace GameEngineTest
{
    /* tests message broadcasting functionality of ComponentCollection */
    public class BroadcastTestComponent : IComponent, IReceiverComponent<IComponentCollection>
    {
        static bool success = false;

        public void Update() { }
        public void Receive(IComponentCollection c)
        {
            success = true;
        }

        public static bool Test()
        {
            ComponentCollection collection = new ComponentCollection();
            ComponentCollection collection2 = new ComponentCollection();
            BroadcastTestComponent c = new BroadcastTestComponent();
            collection.Add(collection2);
            collection2.Add(c);
            collection.Broadcast(collection);
            return success;
        }
    }
}
