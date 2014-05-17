using GameEngineConcept;
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

        public override void OnLoad(GameState s)
        {
            base.OnLoad(s);
            //create a component structure that references our test component
            //at multiple levels of nesting.
            var c = new ComponentCollection() {
                this,
                new ComponentCollection() { 
                    this,
                    new ComponentCollection() { this }
                }
            };
            s.AddComponents(new[] { c });
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
