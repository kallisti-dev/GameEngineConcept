using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
