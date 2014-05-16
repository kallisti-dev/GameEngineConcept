using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineConcept.Graphics.VertexAttributes
{

    public interface IHasVertexAttributes
    {
        VertexAttributeSet VertexAttributes { get; }
    }
}
