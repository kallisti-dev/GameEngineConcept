using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngineConcept.Components;

namespace GameEngineTest.Tests
{

    /* tests message broadcasting functionality of ComponentCollection */
    public class BroadcastTestComponent : BaseTester, IComponent, IReceiverComponent<BroadcastTestComponent.TestMessage>
    {
        public class TestMessage : Message { }
        public void Update() { }
        public void Receive(TestMessage m)
        {
            TestSuccess();
        }

        public override void OnLoad(TestWindow window)
        {
            ComponentCollection collection = new ComponentCollection();
            BroadcastTestComponent c = new BroadcastTestComponent();
            collection.Add(c);
            window.AddComponents(new[] {c});
            collection.Broadcast(new TestMessage());
        }
    }
}
