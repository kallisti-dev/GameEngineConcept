using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngineConcept.Components;

namespace GameEngineTest.Tests
{

    /* tests message broadcasting functionality of ComponentCollection */
    public class BroadcastTestComponent : BaseTester, IReceiver<BroadcastTestComponent.TestMessage>
    {
        int count = 0;

        //dummy message (message content not important)
        public class TestMessage : Message { }
        
        //dummy component update (not used)
        public void Update() { }

        public override void OnLoad(TestWindow window)
        {
            //create a component structure that references our test component
            //at multiple levels of nesting.
            var c = new ComponentCollection() {
                this,
                new ComponentCollection() { 
                    this,
                    new ComponentCollection() { this }
                }
            };
            window.AddComponent(c);
            c.Broadcast(new TestMessage());
        }

        public void Receive(TestMessage m)
        {
            if (++count == 3) {
                TestSuccess();
            }
        }
    }
}
