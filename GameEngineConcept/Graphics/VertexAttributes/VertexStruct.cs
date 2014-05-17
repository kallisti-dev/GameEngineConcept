using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineConcept.Graphics.VertexAttributes
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class VertexStruct : Attribute
    {
        public bool DefaultNormalization { get; private set; }

        public VertexStruct() : this(true) { }

        public VertexStruct(bool defaultNormalization)
        {
            DefaultNormalization = defaultNormalization;
        }
    }
}
