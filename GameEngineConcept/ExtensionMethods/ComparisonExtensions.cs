
namespace GameEngineConcept.ExtensionMethods
{
    using Graphics;
    using Graphics.VertexBuffers;

    //extension methods for in-library interfaces
    public static class GameEngineExtensions
    {
        //inverse comparators for some comparison methods defined in library
        public static int CompareTo(this int i, IVertexBuffer b)
        {
            return -b.CompareTo(i);
        }

        public static int CompareTo(this IVertexBuffer b, IDrawable d)
        {
            return -d.CompareTo(b);
        }
    }
}
