
namespace GameEngineTest
{
    //abstract base class for draw tests
    public abstract class DrawTester : BaseTester
    {
        protected DrawTester()
            : base()
        {
            SucceedOnTimeout = true;
        }
    }
}
