
namespace GameEngineConcept.Graphics.VertexAttributes
{
    public sealed class NumberOfComponents : AttributeDescriptor
    {
        int n;

        public NumberOfComponents(int nComponents)
        {
            n = nComponents;
        }

        public override void AddFields(VertexAttribute a)
        {
            a.nComponents = n;
        }
    }
}
