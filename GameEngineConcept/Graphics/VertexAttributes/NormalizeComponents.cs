
namespace GameEngineConcept.Graphics.VertexAttributes
{
    public sealed class NormalizeComponents : AttributeDescriptor
    {
        bool n;

        public NormalizeComponents(bool normalized)
        {
            n = normalized;
        }

        public override void AddFields(VertexAttribute a)
        {
            a.normalized = n;
        }
    }
}
